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


namespace exchaRazor02.Pages
{
	[AutoValidateAntiforgeryToken]
	public class CreateModel : PageModel
    {
		private readonly ILogger<CreateModel> _logger;
		private readonly exchaRazor02.Data.ExchaDContext5 _context;
		
		//コンストラクタ
        public CreateModel(ILogger<CreateModel> logger, exchaRazor02.Data.ExchaDContext5 context)
        {
			_logger = logger;
			_context = context;
			Diary = new Diary();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Diary Diary { get; set; }

		//POST
		public async Task<IActionResult> OnPostAsync()
        {
			if (!ModelState.IsValid) return Page();

			//日記、その他の項目を初期設定する。(id, pass, note, pubは、POSTされた値を使用する)
			this.Diary.last = DateTime.Now;
			this.Diary.excha = EXCHA.disable;
			this.Diary.writa = WRITA.able;
			this.Diary.retTime = DateTime.Now;
			this.Diary.exid = null;

			//DBへ保存する
			_context.diaries.Add(Diary);
			await _context.SaveChangesAsync();
			try
			{

			}
			catch (DbUpdateException)
			{
				if (_context.diaries.Any(e => e.Id == Diary.Id))
				{
					//id重複
					return RedirectToPage("/Error");
				}
				else
				{
					throw;
				}
			}
			catch (Exception ex)
			{
				_logger.Log(LogLevel.Error, ex.Message);
				return RedirectToPage("/Error");
			}

			return RedirectToPage("/Account/Login");
        }
	}
}
