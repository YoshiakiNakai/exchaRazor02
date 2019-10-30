using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using exchaRazor02.Data;

namespace exchaRazor02.Pages
{
	[Authorize]	//ログイン中でなければ見れない
    public class MyModel : PageModel
    {

		private readonly exchaRazor02.Data.ExchaDContext9 _context;

		public Diary diary;

		public MyModel(exchaRazor02.Data.ExchaDContext9 context)
		{
			_context = context;
			diary = new Diary();
		}

		public async Task OnGetAsync()
        {
			//HttpContext.User.Identity.IsAuthenticated
			string authId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			diary = await _context.diaries.FindAsync(authId);
		}
	}
}
