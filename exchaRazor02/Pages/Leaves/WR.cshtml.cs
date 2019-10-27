using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        private readonly exchaRazor02.Data.ExchaDContext7 _context;

        public WRModel(exchaRazor02.Data.ExchaDContext7 context)
        {
            _context = context;
        }

        [BindProperty]
        public Leaf leaf { get; set; }

		[BindProperty]
		public string dispId { get; set; }	//表示中の日記ID

		[TempData]
		public bool message { get; set; }

		[TempData]
		public bool commentFlag { get; set; }   //コメント権限ありなし

		[TempData]
		public bool createFlag { get; set; }  //作成権限ありなし

		[TempData]
		public bool editFlag { get; set; }	//編集権限ありなし


		//leafを表示する
		//引数１：日記ID
		//引数２：日時
		public async Task<IActionResult> OnGetAsync(string id, DateTime time)
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
			dispId = id;
			leaf = _context.leaves.Where(l => (l.diaryId == id && l.time == time)).FirstOrDefault();
			if(leaf == null) {
				createFlag = DiaryAuth.authCreateLeaf(user, diary);     //作成権限を取得
				editFlag = false;
				commentFlag = false;
			} else {
				createFlag = false;
				editFlag = await DiaryAuth.authEditLeaf(user, _context, leaf);   //編集権限を取得
				commentFlag = await DiaryAuth.authCommentLeaf(user, _context, leaf);   //コメント権限を取得
			}
            return Page();
        }

		//leafの作成、書き込みを行う
        public async Task<IActionResult> OnPostAsync()
        {
			if (!ModelState.IsValid) return Page();

			//基本情報の取得
			ClaimsPrincipal user = HttpContext.User;
			Diary diary = await _context.diaries.FindAsync(dispId);
			if (diary == null) return StatusCode(404);
			Leaf dbLeaf = _context.leaves.Where(l => (l.diaryId == leaf.diaryId && l.time == leaf.time)).FirstOrDefault();
			if (dbLeaf == null) {
				createFlag = DiaryAuth.authCreateLeaf(user, diary);     //作成権限を取得
				editFlag = false;
				commentFlag = false;
			} else {
				createFlag = false;
				editFlag = await DiaryAuth.authEditLeaf(user, _context, dbLeaf);   //編集権限を取得
				commentFlag = await DiaryAuth.authCommentLeaf(user, _context, dbLeaf);   //コメント権限を取得
			}

			//権限に従い、leafの処理を行う
			//作成か
			if (createFlag) {
				//作成のとき
				leaf.diaryId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
				leaf.time = DateTime.Now;
				leaf.exid = null;
				leaf.contents = null;
				_context.leaves.Add(leaf);
			}//コメントか
			else if (commentFlag) {
				//コメントのとき
				dbLeaf.exid = user.FindFirst(ClaimTypes.NameIdentifier).Value;
				dbLeaf.comment = leaf.comment;
				_context.Attach(leaf).State = EntityState.Modified;
			}//編集か
			else if(editFlag) {
				//編集のとき
				dbLeaf.time = DateTime.Now;
				dbLeaf.title = leaf.title;
				dbLeaf.contents = leaf.contents;
				_context.Attach(leaf).State = EntityState.Modified;
			}
			else {
				//変更権限なしのとき
				return StatusCode(403);
			}

			await _context.SaveChangesAsync();
			//catch (DbUpdateConcurrencyException) {/* 存在しないときのエラー */}

            return Page();
        }
    }
}
