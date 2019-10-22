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


namespace exchaRazor02.Pages
{
	[Authorize]	//ログイン中でなければ見れない
    public class MyModel : PageModel
    {
        public void OnGet()
        {
			//var a = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
		}
	}
}