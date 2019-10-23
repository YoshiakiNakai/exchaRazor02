using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

//ボツ
namespace PasswordHash.Lib
{
	//パスワードのハッシュ化を提供するインタフェース
	interface IPasswordService
	{
		public (string hashedPassword, byte[] salt) HashPassword(string rawPassword);
		public bool VerifyPassword(string hashedPassword, string rawPassword, byte[] salt);
	}

	//パスワードのハッシュ化を提供するクラス
	public class PasswordService : IPasswordService
	{
		//ハッシュ化する
		//引数rawPassword：平文パスワード
		//戻り値：ハッシュ値、使用したsalt
		public (string hashedPassword, byte[] salt) HashPassword(string rawPassword)
		{
			byte[] salt = GetSalt();
			string hashed = hash(rawPassword, salt);
			return (hashed, salt);
		}

		//検証する
		//引数１ hashedPassword：ハッシュ値
		//引数２ rawPassword：平文パスワード
		//引数３ salt：使用するsalt
		public bool VerifyPassword(string hashedPassword, string rawPassword, byte[] salt) =>
		  hashedPassword == hash(rawPassword, salt);

		//PBKDF2により、パスワードをハッシュ化する。Base64に変換して返す。
		//引数１ rawPassword：平文パスワード
		//引数２ salt：使用するsalt
		//戻り値：ハッシュ値
		private string hash(string rawPassword, byte[] salt) =>
		  Convert.ToBase64String(
			KeyDerivation.Pbkdf2(   //Pbkdf2：Password-Based Key Derivation Function 2
			  password: rawPassword,
			  salt: salt,
			  prf: KeyDerivationPrf.HMACSHA512,
			  iterationCount: 999,
			  numBytesRequested: 64));

		//saltを生成する
		//戻り値：salt
		private byte[] GetSalt()
		{
			using (var gen = RandomNumberGenerator.Create())
			{
				var salt = new byte[32];
				gen.GetBytes(salt);	//saltにランダムな値を詰め込む
				return salt;
			}
		}
	}
}