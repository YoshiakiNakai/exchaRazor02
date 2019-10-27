using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;
using exchaRazor02.Data;

namespace exchaRazor02.Pages.Controllers
{
	//役割:
	// 交換申請
	// 交換承諾
	// 交換拒否
	//[AutoValidateAntiforgeryToken]
	[Route("api/[controller]")]
    [ApiController]
	public class AppliController : ControllerBase
    {
        private readonly ExchaDContext7 _context;

        public AppliController(ExchaDContext7 context)
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
        public async Task<bool> apply(string diaryId, double period)
        {
			//最新のleafの日時を取得
			DateTime latest = await _context.leaves
				.Where(l => l.diaryId == diaryId)
				.MaxAsync(l => l.time);

			Appli appli = new Appli(diaryId, latest, HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value, EXCHA_ACCEPT.yet, period);
			_context.appli.Add(appli);

			try {
				await _context.SaveChangesAsync();
			} catch (DbUpdateException) {
				//申請済みか確認
				if (_context.appli.Any(a => (
						(a.diaryId == appli.diaryId)
						&& a.leafTime == appli.leafTime))) {
					return false;	//Conflict();
				} else {
					throw;
				}
			}
			return true;
        }


		//POST: api/Appli/{action}
		//承諾する
		//引数１：交換相手
		//戻り値：true 成功
		[HttpPost]
		public async Task<bool> accept(string exid)
		{
			//appliへ承諾を登録
			//お互いの日記に交換相手を記録
			//返却日時の記録

			//appliへ承諾を登録
			string authId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			DateTime latest = await _context.leaves
				.Where(l => l.diaryId == authId)
				.MaxAsync(l => l.time);
			Leaf leaf = new Leaf();
			leaf.diaryId = authId;
			leaf.time = latest;
			leaf.exid = exid;
			Appli appli = await _context.appli.FindAsync(leaf);
			if (appli == null) return false;
			appli.accept = EXCHA_ACCEPT.accept;
			_context.Attach(appli).State = EntityState.Modified;

			//お互いの日記に交換を記録
			Diary my = await _context.diaries.FindAsync(authId);
			Diary your = await _context.diaries.FindAsync(appli.apid);
			my.exid = your.Id;
			my.retTime = DateTime.Now.AddHours(appli.period);
			your.exid = my.Id;
			your.retTime = DateTime.Now.AddHours(appli.period);
			_context.Attach(my).State = EntityState.Modified;
			_context.Attach(your).State = EntityState.Modified;

			await _context.SaveChangesAsync();

			return true;
		}

		//POST: api/Appli/{action}
		//断る
		//引数１：断る相手
		//戻り値：true 成功
		[HttpPost]
		public async Task<bool> reject(string exid)
		{
			//appliへ拒絶を登録
			string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			DateTime latest = await _context.leaves
				.Where(l => l.diaryId == userId)
				.MaxAsync(l => l.time);
			Leaf leaf = new Leaf();
			leaf.diaryId = userId;
			leaf.time = latest;
			leaf.exid = exid;
			Appli appli = await _context.appli.FindAsync(leaf);
			if (appli == null) return false;
			appli.accept = EXCHA_ACCEPT.reject;
			_context.Attach(appli).State = EntityState.Modified;

			await _context.SaveChangesAsync();

			return true;
		}



	}
}
