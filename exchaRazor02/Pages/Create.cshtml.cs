using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using exchaRazor02.Data;

namespace exchaRazor02.Pages
{
	[AutoValidateAntiforgeryToken]
	public class CreateModel : PageModel
    {
        private readonly exchaRazor02.Data.ExchaDContext5 _context;
		
		//コンストラクタ
        public CreateModel(exchaRazor02.Data.ExchaDContext5 context)
        {
            _context = context;
			this.Diary = new Diary(null, null, null, DateTime.Now, PUBLICITY.pub, EXCHA.disable, WRITA.able, DateTime.Now, null);
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

			this.Diary.last = DateTime.Now;
			this.Diary.retTime = DateTime.Now;

			_context.diaries.Add(Diary);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Login");
        }
	}
}
