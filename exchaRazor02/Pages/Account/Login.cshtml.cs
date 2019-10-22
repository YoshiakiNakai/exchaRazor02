using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace exchaRazor02.Pages.Account
{
    public class LoginModel : PageModel
    {
		public void OnGet()
		{

		}

		//ログインフォームのデータとバインド
		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Required]
			public string diaryId { get; set; }

			[Required]
			[DataType(DataType.Password)]
			public string pass { get; set; }
		}

		[TempData]
		public string ErrorMessage { get; set; }


		//Post
		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			//必須入力がないなどの場合、処理しない
			if (!ModelState.IsValid) return Page();

			//認証処理

			//if (!isValid) return Page();

			//ログイン処理
			//認証情報の登録
			var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);	//Cookie認証を利用する
			identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Input.diaryId)); //Idを格納

			//ログイン
			await HttpContext.SignInAsync(
			  CookieAuthenticationDefaults.AuthenticationScheme,
			  new ClaimsPrincipal(identity),
			  new AuthenticationProperties
			  {
				  IsPersistent = false	//ブラウザを閉じたとき、ログインを維持するか
			  });
			return LocalRedirect(returnUrl ?? Url.Content("~/"));
		}
	}
}