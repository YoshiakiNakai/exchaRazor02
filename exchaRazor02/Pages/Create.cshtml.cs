using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using exchaRazor02.Data;
using Microsoft.Extensions.Logging;
using System.Data.Common;	//DbException

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

			_context.diaries.Add(Diary);
			await _context.SaveChangesAsync();

			//DBへ保存する
			try
			{
			}
			catch (Exception ex)
			{
				//DbException
				//抽象クラス、これを継承してDBのエラーごとに具体クラスを作って使う。
				//https://docs.microsoft.com/ja-jp/dotnet/api/system.data.common.dbexception?view=netframework-4.8
				_logger.Log(LogLevel.Error, ex.Message);
				return RedirectToPage("/Error");
			}
			return RedirectToPage("/Account/Login");
        }
	}
}
