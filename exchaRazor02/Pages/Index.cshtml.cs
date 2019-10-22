using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace exchaRazor02.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }
		
		//POST
		public async Task<IActionResult> OnPostAsync()
		{
			//ログアウト処理
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			// ログアウト後はトップページへリダイレクト
			return LocalRedirect(Url.Content("~/"));
		}
	}
}