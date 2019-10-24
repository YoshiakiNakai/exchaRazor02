using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;
using exchaRazor02.Data;

namespace exchaRazor02.Pages.Leaves
{
    public class IndexModel : PageModel
    {
        private readonly exchaRazor02.Data.ExchaDContext5 _context;

        public IndexModel(exchaRazor02.Data.ExchaDContext5 context)
        {
            _context = context;
        }

        public IList<Leaf> leaves { get;set; }

		//指定したidの日記の目次を表示する
		public async Task<IActionResult> OnGetAsync(string id)
        {
			bool openFlag = false;  //公開フラグ

			ClaimsPrincipal user = HttpContext.User;

			//日記の情報を取得する
			Diary diary = await _context.diaries.FindAsync(id);
			if (diary == null) return Redirect("/Error?code=404");

			//日記を開く権限があるか、確認する
			// 交換中、かつ、交換者：表示
			// 交換中：非表示
			// 公開：表示
			// 非公開、かつ、自分の：表示
			// 非公開：非表示

			//交換中か
			if (diary.exid != null) {
				//交換中のとき、交換相手にのみ表示する
				if (!user.Identity.IsAuthenticated) {
					//未ログインのとき、表示しない
				}//ログイン中のとき
				else if(diary.exid == user.FindFirst(ClaimTypes.NameIdentifier).Value) {
					//交換者のとき、表示する
					openFlag = true;
				}
			}//交換中でないとき
			//公開か
			else if(diary.pub == PUBLICITY.pub) {
				//公開のとき、表示する
				openFlag = true;
			}//非公開のとき
			//ログイン中か
			else if(user.Identity.IsAuthenticated) {
				//ログイン中のとき、自分の日記なら表示する
				if (diary.Id == user.FindFirst(ClaimTypes.NameIdentifier).Value) {
					openFlag = true;
				}
			}

			if (openFlag) {
				leaves = await _context.leaves.Where(l => l.diaryId == id).ToListAsync();
				return Page();
			} else {
				return Redirect("/Error?code=403");
			}
		}
	}
}
