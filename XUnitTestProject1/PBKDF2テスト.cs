using System;
using Xunit;
using Microsoft.Extensions.Logging;
using PasswordHashing;

namespace XUnitTestProject1
{
	public class PBKDF2テスト
	{
		private readonly ILogger<PBKDF2テスト> _logger;

		public PBKDF2テスト()
		{
			var loggerFactory = LoggerFactory.Create(builder =>
			{
				builder
					.AddFilter("Microsoft", LogLevel.Warning)
					.AddFilter("System", LogLevel.Warning)
					.AddConsole()
					.AddDebug()
					.AddEventLog();
			});
			_logger = loggerFactory.CreateLogger<PBKDF2テスト>();
			_logger.Log(LogLevel.Information, "PBKDF2テスト logger");
		}

		private const string HUNDRED_STR = "1234567890223456789032345678904234567890523456789062345678907234567890823456789092345678909234567890";
		private const string HUNDRED_ONE_STR = HUNDRED_STR + "1";
		private const string SPECIAL_STR = "\"#$%&'()=^~\\|[{@`]}:*;+_/?.>,<";

		//観点：検証成功すること
		//入力１：登録時のパスワード
		//入力２：ログイン時のパスワード
		[Theory]
		[InlineData("abc", "abc")]
		[InlineData("", "")]
		[InlineData(HUNDRED_STR, HUNDRED_STR)]
		[InlineData(SPECIAL_STR, SPECIAL_STR)]
		[InlineData("\0", "")]
		[InlineData("\0a\0", "\0a")]	//最後の'\0'は終端文字として扱われる。
		public void PBKDF2テスト１(string pass1, string pass2)
		{
			string hash = PBKDF2.Hash(pass1).ToString();
			Assert.True(PBKDF2.Verify(pass2, hash));
		}
		//観点：検証失敗すること
		//入力１：登録時のパスワード
		//入力２：ログイン時のパスワード
		[Theory]
		[InlineData("abc", "abcd")]
		[InlineData("", "0")]
		[InlineData(HUNDRED_STR, HUNDRED_ONE_STR)]
		[InlineData(".", "+")]
		[InlineData("+", ".")]
		[InlineData(" ", "\t")]
		[InlineData("\0a\0a", "\0aa")]
		public void PBKDF2テスト２(string pass1, string pass2)
		{
			string hash = PBKDF2.Hash(pass1).ToString();
			Assert.False(PBKDF2.Verify(pass2, hash));
		}
	}
}