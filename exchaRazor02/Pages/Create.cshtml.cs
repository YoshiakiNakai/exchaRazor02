using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using exchaRazor02.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using System.Security.Cryptography;
using PasswordHashing;

namespace exchaRazor02.Pages
{
	[AutoValidateAntiforgeryToken]
	public class CreateModel : PageModel
    {
		private readonly ILogger<CreateModel> _logger;
		private readonly exchaRazor02.Data.ExchaDContext9 _context;
		
		//コンストラクタ
        public CreateModel(ILogger<CreateModel> logger, exchaRazor02.Data.ExchaDContext9 context)
        {
			_logger = logger;
			_context = context;
			Diary = new Diary();
			message = null;
		}

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Diary Diary { get; set; }

		[TempData]
		public string message { get; set; }

		//POST
		public async Task<IActionResult> OnPostAsync()
        {
			if (!ModelState.IsValid) return Page();

			//パスワードをハッシュ化する
			this.Diary.pass = PBKDF2.Hash(this.Diary.pass).ToString();

			//日記、その他の項目を初期設定する。(id, noteは、POSTされた値を使用する)
			this.Diary.pub = PUBLICITY.pub;
			this.Diary.last = DateTime.Now;
			this.Diary.excha = EXCHA.disable;
			this.Diary.writa = WRITA.able;
			this.Diary.rettime = DateTime.Now;
			this.Diary.exid = null;

			//DBへ保存する
			_context.diaries.Add(Diary);
			try {
				await _context.SaveChangesAsync();
			} catch (DbUpdateException ex) {
				_logger.Log(LogLevel.Error, ex.Message);
				//id重複確認
				if (_context.diaries.Any(e => e.id == Diary.id)) {
					this.message = "エラー：既に使用されているIDです";
					return Page();
				} else {
					throw;
				}
			}
			return RedirectToPage("/Account/Login");
        }
	}
}
