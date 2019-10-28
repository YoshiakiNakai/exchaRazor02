using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PasswordHashing
{
	//Pbkdf2：Password-Based Key Derivation Function 2
	public static class PBKDF2
	{
		private const int DerivedKeyLength = 64;
		//ハッシュ化する
		//戻り値：ハッシュ結果
		public static PBKDF2Hash Hash(string password, int saltSize = 16, int iterations = 999)
		{
			//ハッシュ化する
			using (var rfc2898 = new Rfc2898DeriveBytes(password, saltSize: saltSize, iterations: iterations))
			{
				var dk = rfc2898.GetBytes(DerivedKeyLength);	//ハッシュ結果から必要な分だけ切り出す
				return new PBKDF2Hash(rfc2898.IterationCount, rfc2898.Salt, dk);	//保存形式に変換する
			}
		}

		//検証する
		//引数１：平文パスワード
		//引数２：ハッシュ値
		public static bool Verify(string password, string hashStr)
		{
			if (password == null) throw new ArgumentNullException(nameof(password));
			if (hashStr == null) throw new ArgumentNullException(nameof(hashStr));
			if (PBKDF2Hash.TryParse(hashStr, out var hash) == false) throw new FormatException(nameof(hashStr));

			return Verify(password, hash);
		}

		//検証する
		//引数１：平文パスワード
		//引数２：PBKDF2Hashクラス
		public static bool Verify(string password, PBKDF2Hash hash)
		{
			if (password == null) throw new ArgumentNullException(nameof(password));
			if (hash == null) throw new ArgumentNullException(nameof(hash));

			using (var deriveBytes = new Rfc2898DeriveBytes(password, salt: hash.Salt, iterations: hash.Iterations))
			{
				var dk = deriveBytes.GetBytes(DerivedKeyLength);
				return hash.DerivedKey.SequenceEqual(dk);
			}
		}
	}

	//ハッシュ結果を管理しやすい形式にする
	//ModularCryptFormat
	public class PBKDF2Hash
	{
		public const string Identifier = "pbkdf2";  //ハッシュ化方式の識別子
		public const char SPRT = '$';   //separator
		public readonly int Iterations; //stretching回数
		public readonly byte[] Salt;    //salt
		public readonly byte[] DerivedKey;	//導出結果

		//コンストラクタ
		public PBKDF2Hash(int iterations, byte[] salt, byte[] derivedKey)
		{
			Iterations = iterations;
			Salt = salt;
			DerivedKey = derivedKey;
		}

		//ハッシュ結果をMCF形式の文字列にして返す
		//区切り文字は'$'を使用する
		public override string ToString()
		{
			return $"{SPRT}{Identifier}{SPRT}{Iterations}{SPRT}{ConvertByteToString(Salt)}{SPRT}{ConvertByteToString(DerivedKey)}";
		}

		//文字列の形式がMCFの形式と一致しているか確認する
		//一致していれば、文字列をPBKDF2Hashクラスに変換して出力する
		//引数１：ハッシュ結果文字列
		//引数２：出力。PBKDF2Hash
		//戻り値：形式が正しいか
		public static bool TryParse(string hashStr, out PBKDF2Hash result)
		{
			result = null;

			//入力形式チェック
			if (hashStr == null) return false;
			if (hashStr.StartsWith(SPRT) == false) return false;
			var elems = hashStr.Split(new[] { SPRT }, StringSplitOptions.RemoveEmptyEntries);
			if (elems.Length != 4) return false;

			//内容チェック
			if (elems[0] != Identifier) return false;
			if (int.TryParse(elems[1], out var iterations) == false) return false;

			//文字列をバイト配列に変換
			var salt = ConvertStringToByte(elems[2]);
			var dk = ConvertStringToByte(elems[3]);

			//出力
			result = new PBKDF2Hash(iterations, salt, dk);
			return true;
		}

		//バイト配列をBase64っぽい文字列に変換する
		//ハッシュ結果を保存しやすい形式に変換するために用いる
		//引数１：バイト配列
		//戻り値：MCFに従った文字列
		//備考：MCFでは、通常のBase64と異なり、'+'ではなく、'.'を使用する
		protected static string ConvertByteToString(byte[] byteArr)
		{
			return Convert.ToBase64String(byteArr).Replace('+', '.');
		}

		//ConvertByteToStringで文字列に変換したデータをバイト配列に戻す
		//文字列に変換したSaltをバイト配列に戻すために用いる
		protected static byte[] ConvertStringToByte(string str)
		{
			return Convert.FromBase64String(str.Replace('.', '+').ToString());
		}
	}
}