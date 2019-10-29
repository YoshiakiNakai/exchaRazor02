using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Security.Claims;
using exchaRazor02.Data;

namespace exchaRazor02.Pages.Leaves
{
	//役割：
	// leaf閲覧
	// leaf新規作成
	// leafコメント
	public class WRModel : PageModel
    {
		private readonly ILogger<WRModel> _logger;
		private readonly exchaRazor02.Data.ExchaDContext9 _context;

        public WRModel(exchaRazor02.Data.ExchaDContext9 context, ILogger<WRModel> logger)
        {
            _context = context;
			_logger = logger;
        }

        [BindProperty]
        public Leaf leaf { get; set; }

		public string message { get; set; }
		public bool commentFlag { get; set; }   //コメント権限ありなし
		public bool createFlag { get; set; }  //作成権限ありなし
		public bool editFlag { get; set; }	//編集権限ありなし


		//leafを表示する
		//引数１：日記ID
		//引数２：日時
		public async Task<IActionResult> OnGetAsync(string id, string time)
        {
            if (id == null || time == null) return NotFound();

			ClaimsPrincipal user = HttpContext.User;

			//日記の情報を取得する
			Diary diary = await _context.diaries.FindAsync(id);
			if (diary == null) return StatusCode(404);
			//日記が存在するとき

			//閲覧権限の確認
			if (!DiaryAuth.authRead(user, diary)) return StatusCode(403);
			//閲覧権限があるとき、ページを表示する

			//バインドデータの設定
			//leaf = _context.leaves
			//	.Where(l => 
			//		(l.diaryId == id)
			//		&& (l.time.ToString() == time)
			//	).FirstOrDefault();
			//
			//DateTimeの条件をWhere()でうまく設定できないため、IListで検索する
			IList<Leaf> Lleaves = _context.leaves.Where(l => l.diaryId == id).ToList();
			for (int i = 0; i < Lleaves.Count(); i++)
			{
				if (Lleaves[i].time.ToString() == time) {
					leaf = Lleaves[i];
					break;
				}
			}

			if (leaf == null) {
				//leafが存在しないとき
				//leafを作成
				leaf = new Leaf();
				leaf.diaryId = id;

				createFlag = DiaryAuth.authCreateLeaf(user, diary);     //作成権限を取得
				editFlag = false;
				commentFlag = false;
			} else {
				createFlag = false;
				editFlag = await DiaryAuth.authEditLeaf(user, _context, leaf);   //編集権限を取得
				commentFlag = await DiaryAuth.authCommentLeaf(user, _context, leaf);   //コメント権限を取得
			}

			if (createFlag) {
				ViewData["Title"] = "新規作成";
			} else if (commentFlag) {
				ViewData["Title"] = "コメント";
			} else if (editFlag) {
				ViewData["Title"] = "編集";
			} else {
				ViewData["Title"] = "閲覧";
			}

			return Page();
        }

		//leafの作成、編集を行う
        public async Task<IActionResult> OnPostAsync()
        {
			if (!ModelState.IsValid) {
				message = "エラー：入力が正しくありません";
				return Page();
			}
			//基本情報の取得
			ClaimsPrincipal user = HttpContext.User;
			Diary objDiary = await _context.diaries.FindAsync(leaf.diaryId);
			if (objDiary == null) return StatusCode(404);
			//Leaf dbLeaf = _context.leaves.Where(l => (l.diaryId == leaf.diaryId && l.time == leaf.time)).FirstOrDefault();
			Leaf dbLeaf = null;
			IList<Leaf> Lleaves = _context.leaves.Where(l => l.diaryId == leaf.diaryId).ToList();
			for (int i = 0; i < Lleaves.Count(); i++)
			{
				if (Lleaves[i].time.ToString() == leaf.time.ToString()) {
					dbLeaf = Lleaves[i];
					break;
				}
			}

			if (dbLeaf == null) {
				createFlag = DiaryAuth.authCreateLeaf(user, objDiary);     //作成権限を取得
				editFlag = false;
				commentFlag = false;
			} else {
				createFlag = false;
				editFlag = await DiaryAuth.authEditLeaf(user, _context, dbLeaf);   //編集権限を取得
				commentFlag = await DiaryAuth.authCommentLeaf(user, _context, dbLeaf);   //コメント権限を取得
			}

			//権限に従い、処理を行う
			//作成か
			if (createFlag) {
				//作成のとき
				//leafの作成
				leaf.diaryId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
				leaf.time = DateTime.Now;
				leaf.exid = null;
				leaf.comment = null;
				_context.leaves.Add(leaf);
				//日記フラグの変更
				Diary my = await _context.diaries.FindAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
				my.excha = EXCHA.able;
				my.writa = WRITA.disable;
				_context.Attach(my).State = EntityState.Modified;
			}//コメントか
			else if (commentFlag) {
				//コメントのとき
				dbLeaf.exid = user.FindFirst(ClaimTypes.NameIdentifier).Value;
				dbLeaf.comment = leaf.comment;
				_context.Attach(dbLeaf).State = EntityState.Modified;
				//日記フラグの変更
				Diary your = await _context.diaries.FindAsync(user.FindFirst(ClaimTypes.NameIdentifier).Value);
				your.writa = WRITA.able;
				objDiary.excha = EXCHA.disable;
				_context.Attach(your).State = EntityState.Modified;
				_context.Attach(objDiary).State = EntityState.Modified;
			}//編集か
			else if(editFlag) {
				//編集のとき
				dbLeaf.title = leaf.title;
				dbLeaf.contents = leaf.contents;
				_context.Attach(dbLeaf).State = EntityState.Modified;
			}
			else {
				//変更権限なしのとき
				return StatusCode(403);
			}

			await _context.SaveChangesAsync();
			//catch (DbUpdateConcurrencyException) {/* 存在しないときのエラー */}

            return Redirect("~/Leaves/Index?id=" + leaf.diaryId);
        }
    }
}
