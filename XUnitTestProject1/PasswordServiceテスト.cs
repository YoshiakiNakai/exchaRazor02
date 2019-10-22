using System;
using Xunit;

namespace PasswordHash.Lib.Test
{
	public class PasswordService�e�X�g
	{
		//�ϓ_�F
		//[Theory]
		//[InlineData("abc")]
		////[InlineData(null)]	//error
		//[InlineData("")]
		//[InlineData("123456789022345678903234567890423456789052345678906234567890723456789082345678909234567890")]
		//[InlineData("1234567890223456789032345678904234567890523456789062345678907234567890823456789092345678909234567890")]
		public void PasswordService�e�X�g�P(string rawPassword)
		{
			//�e�X�g�Ώ�
			var pswSrv = new PasswordService();

			//�p�X���[�h���n�b�V�����A�g�p�����\���g�𓾂�
			var (hashed, salt) = pswSrv.HashPassword(rawPassword);

			//System.Diagnostics.Debug.WriteLine(hashed);

			//�u�n�b�V���v�Ɓu�p�X���[�h�E�\���g����쐬�����n�b�V���v����v���邩�e�X�g
			Assert.True(pswSrv.VerifyPassword(hashed, rawPassword, salt));
		}
	}
}