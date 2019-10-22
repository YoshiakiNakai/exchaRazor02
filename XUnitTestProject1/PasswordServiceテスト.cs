using System;
using Xunit;

namespace PasswordHash.Lib.Test
{
	public class PasswordServiceテスト
	{
		//観点：
		//[Theory]
		//[InlineData("abc")]
		////[InlineData(null)]	//error
		//[InlineData("")]
		//[InlineData("123456789022345678903234567890423456789052345678906234567890723456789082345678909234567890")]
		//[InlineData("1234567890223456789032345678904234567890523456789062345678907234567890823456789092345678909234567890")]
		public void PasswordServiceテスト１(string rawPassword)
		{
			//テスト対象
			var pswSrv = new PasswordService();

			//パスワードをハッシュ化、使用したソルトを得る
			var (hashed, salt) = pswSrv.HashPassword(rawPassword);

			//System.Diagnostics.Debug.WriteLine(hashed);

			//「ハッシュ」と「パスワード・ソルトから作成したハッシュ」が一致するかテスト
			Assert.True(pswSrv.VerifyPassword(hashed, rawPassword, salt));
		}
	}
}