using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

using exchaRazor02.Data;
using PasswordHashing;

namespace exchaRazor02.Pages.Account
{
	[AutoValidateAntiforgeryToken]
	public class LoginModel : PageModel
    {
		private readonly exchaRazor02.Data.ExchaDContext9 _context;

		//コンストラクタ
		public LoginModel(exchaRazor02.Data.ExchaDContext9 context)
		{
			_context = context;
		}

		public void OnGet() { }

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

		//[TempData]
		public string message { get; set; }


		//Post
		public async Task<IActionResult> OnPostAsync()
		{
			//必須入力がないなどの場合、処理しない
			if (!ModelState.IsValid) return Page();

			//認証処理
			Diary diary = await _context.diaries.FindAsync(Input.diaryId);
			if(diary == null) {
				message = "エラー：日記が見つかりません";
				return Page();
			} else if(!PBKDF2.Verify(Input.pass, diary.pass)) {
				message = "エラー：鍵が一致しません";
				return Page();
			}
			//認証成功

			//ログイン日時をDBへ保存
			diary.last = DateTime.Now;
			_context.Attach(diary).State = EntityState.Modified;
			//try {
			await _context.SaveChangesAsync();
			//} catch (Exception ex) { }

			//ログイン処理
			//認証情報の登録
			var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);	//Cookie認証を利用する
			identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Input.diaryId)); //Idを格納
			var salt = new byte[8];
			RandomNumberGenerator.Create().GetBytes(salt);
			identity.AddClaim(new Claim(ClaimTypes.Sid, Convert.ToBase64String(salt))); //csrf用id

			//ログイン
			await HttpContext.SignInAsync(
			  CookieAuthenticationDefaults.AuthenticationScheme,
			  new ClaimsPrincipal(identity),
			  new AuthenticationProperties
			  {
				  IsPersistent = false,  //ブラウザを閉じたとき、ログインを維持するか
				  ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
			  });
			return LocalRedirect(Url.Content("~/"));	/* heroku上ではreturnUrlでエラーが発生する */
		}
	}
}
