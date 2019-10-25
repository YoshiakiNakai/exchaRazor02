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
	//責務：
	// 閲覧
	// 新規作成
	// コメント
	public class WRModel : PageModel
    {
        private readonly exchaRazor02.Data.ExchaDContext5 _context;

        public WRModel(exchaRazor02.Data.ExchaDContext5 context)
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
		public bool commentFlag { get; set; }	//コメント権限ありなし

		[TempData]
		public bool createFlag { get; set; }	//作成権限ありなし


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

			//閲覧権限があるか
			if (DiaryAuth.authRead(user, diary)) return StatusCode(403);
			//閲覧権限があるとき、ページを表示する

			//バインドデータの設定
			dispId = id;
			leaf = await _context.leaves
				.Where(l => l.diaryId == id)
				.FirstOrDefaultAsync(l => l.time == time);
			createFlag = DiaryAuth.authCreateLeaf(user, diary);		//作成権限を取得
			commentFlag = await DiaryAuth.authCommentLeaf(user, _context, leaf);   //コメント権限を取得

			//●表示内容、要確認
            if (leaf == null) {
                return Page();
            }
			ViewData["diaryId"] = new SelectList(_context.diaries, "Id", "Id");
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
			createFlag = DiaryAuth.authCreateLeaf(user, diary);     //作成権限を取得
			commentFlag = await DiaryAuth.authCommentLeaf(user, _context, leaf);   //コメント権限を取得

			//●権限に従って、leafを作成、更新する
			//作成か
			if (createFlag) {
				//作成のとき

			}//コメントか
			else if (commentFlag) {
				//コメントのとき

			} else {
				//変更権限なしのとき
				return StatusCode(403);
			}



			//_context.Attach(leaf).State = EntityState.Modified;

			try
			{
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeafExists(leaf.diaryId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool LeafExists(string id)
        {
            return _context.leaves.Any(e => e.diaryId == id);
        }
    }
}
