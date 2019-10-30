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
        private readonly exchaRazor02.Data.ExchaDContext9 _context;

        public IndexModel(exchaRazor02.Data.ExchaDContext9 context)
        {
            _context = context;
        }

        public IList<Leaf> leaves { get; set; }
		public bool exchaFlag { get; set; }  //交換申請可能か
		public int? appliPeriod { get; set; }  //交換申請されているか
		public bool createFlag { get; set; }  //作成権限ありなし
		public bool commentFlag { get; set; }   //コメント権限ありなし

		//指定したidの日記の目次を表示する
		public async Task<IActionResult> OnGetAsync(string id)
		{
			ClaimsPrincipal user = HttpContext.User;

			//日記の情報を取得する
			Diary diary = await _context.diaries.FindAsync(id);
			if (diary == null) return StatusCode(404);
			//日記が存在するとき

			//閲覧権限の確認
			if (!DiaryAuth.authRead(user, diary)) return StatusCode(403);
			//閲覧権限があるとき
			//内容を表示する
			leaves = await _context.leaves.Where(l => l.diaryId == id).ToListAsync();
			if (user.Identity.IsAuthenticated) {
				string authId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
				//ログイン中のとき
				exchaFlag = await DiaryAuth.authExcha(user, _context, diary);
				createFlag = DiaryAuth.authCreateLeaf(user, diary);
				commentFlag = (
					(diary.exid == authId)
					&& (diary.retTime > DateTime.Now)
					);
				appliPeriod = await DiaryAuth.applied(user, _context, diary);
			}
			return Page();
		}

	}
}
