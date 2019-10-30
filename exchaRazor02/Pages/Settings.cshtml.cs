using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using exchaRazor02.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;


namespace exchaRazor02.Pages
{
	[Authorize]
	[AutoValidateAntiforgeryToken]
	public class SettingsModel : PageModel
    {
        private readonly exchaRazor02.Data.ExchaDContext9 _context;

        public SettingsModel(exchaRazor02.Data.ExchaDContext9 context)
        {
            _context = context;
			form = new FormModel();
        }

		//フォームのデータとバインド
		[BindProperty]
		public FormModel form { get; set; }

		public class FormModel
		{
			[Required]
			public string note { get; set; }

			[Required]
			public PUBLICITY pub { get; set; }
		}

		//日記情報を取得する
		public async Task<IActionResult> OnGetAsync()
        {
			//認証情報から日記を取得
			string id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			Diary diary = await _context.diaries.FirstOrDefaultAsync(m => m.id == id);

			if (diary == null) return NotFound();

			form.note = diary.note;
			form.pub = diary.pub;

            return Page();
        }

		//日記の設定変更を行う
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

			//認証情報から日記を取得
			string id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			Diary diary = await _context.diaries.FirstOrDefaultAsync(m => m.id == id);

			//POSTデータを適用
			diary.note = form.note;
			diary.pub = form.pub;

			//DBへ保存
			_context.Attach(diary).State = EntityState.Modified;
			try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!_context.diaries.Any(e => e.id == diary.id)) return NotFound();
                else throw;
            }

            return RedirectToPage("/My");
        }
    }
}
