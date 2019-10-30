using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using exchaRazor02.Data;
using PasswordHashing;
using exchaRazor02.Pages.Leaves;

namespace exchaRazor02.Pages.Controllers
{
	//役割:
	// 交換申請
	// 交換承諾
	// 交換拒否
	//[AutoValidateAntiforgeryToken]
	[Route("api/[controller]")]
    [ApiController]
	[Authorize]
	public class AppliController : ControllerBase
    {
        private readonly ExchaDContext9 _context;

        public AppliController(ExchaDContext9 context)
        {
            _context = context;
        }

		//POST: api/Appli/{action}
		//申請する
		//引数１：申請先の日記ID
		//引数２：交換期間
		//戻り値：true 成功、false 申請済み
		[HttpPost]
		[Route("apply")]
		//[ValidateAntiForgeryToken]
        public async Task<bool> apply(string diaryId, int exchaPeriod, string token)
        {
			//POSTデータを取得する
			// 引数で受け取る方法がわからないので、HttpContextから取得する
			var form = HttpContext.Request.Form;
			Microsoft.Extensions.Primitives.StringValues value;
			form.TryGetValue("diaryId", out value);
			diaryId = value.ToString();
			form.TryGetValue("exchaPeriod", out value);
			exchaPeriod = int.Parse(value.ToString());
			form.TryGetValue("token", out value);
			token = value.ToString();
			if (!PBKDF2.Verify(HttpContext.User.FindFirst(ClaimTypes.Sid).Value, token)) return false;
			string authId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

			//最新のleafの日時を取得
			DateTime latest = await _context.leaves
				.Where(l => l.diaryId == diaryId)
				.MaxAsync(l => l.time);

			Appli appli = new Appli(diaryId, latest, authId, EXCHA_ACCEPT.yet, exchaPeriod);
			_context.appli.Add(appli);

			try {
				await _context.SaveChangesAsync();
			} catch (DbUpdateException) {

				//申請済みか確認
				if (_context.appli.Any(a => (
						(a.diaryId == appli.diaryId)
						&& (a.leafTime == appli.leafTime)
						&& (a.apid == authId)
						))) {
					return false;	//Conflict();
				} else {
					throw;
				}
			}
			return true;
        }


		//POST: api/Appli/{action}
		//交換申請に対する返答
		//引数１：承諾、拒否
		//引数２：交換相手
		//戻り値：true 成功
		[HttpPost]
		[Route("accept")]
		public async Task<bool> reply(EXCHA_ACCEPT excha, string exid, string token)
		{
			//POSTデータを取得する
			var form = HttpContext.Request.Form;
			Microsoft.Extensions.Primitives.StringValues value;
			form.TryGetValue("excha", out value);
			if (value.ToString() == "accept") {
				excha = EXCHA_ACCEPT.accept;
			} else if(value.ToString() == "reject") {
				excha = EXCHA_ACCEPT.reject;
			}
			form.TryGetValue("exid", out value);
			exid = value.ToString();
			form.TryGetValue("token", out value);
			token = value.ToString();
			if (!PBKDF2.Verify(HttpContext.User.FindFirst(ClaimTypes.Sid).Value, token)) return false;

			//日記の情報を取得する
			Diary diary = await _context.diaries.FindAsync(exid);
			if (diary == null) return false;
			//日記が存在するとき
			//交換可能か
			if (!await DiaryAuth.authExcha(HttpContext.User, _context, diary)) return false;
			//交換可能なとき

			//appliへ承諾を登録
			//お互いの日記に交換相手を記録
			//返却日時の記録

			//appliへ承諾を登録
			string authId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			DateTime latest = await _context.leaves
				.Where(l => l.diaryId == authId)
				.MaxAsync(l => l.time);
			Appli appli = await _context.appli
				.Where(a => 
					(a.diaryId == authId)
					&& (a.leafTime == latest)
					&& (a.apid == exid)
				)
				.FirstOrDefaultAsync();
			appli.accept = excha;
			_context.Attach(appli).State = EntityState.Modified;

			if(excha == EXCHA_ACCEPT.accept) {
				//承諾のとき
				//お互いの日記に交換を記録
				Diary my = await _context.diaries.FindAsync(authId);
				Diary your = await _context.diaries.FindAsync(appli.apid);
				my.exid = your.Id;
				my.retTime = DateTime.Now.AddHours(appli.period);
				your.exid = my.Id;
				your.retTime = DateTime.Now.AddHours(appli.period);
				_context.Attach(my).State = EntityState.Modified;
				_context.Attach(your).State = EntityState.Modified;
			}

			await _context.SaveChangesAsync();

			return true;
		}
	}
}
