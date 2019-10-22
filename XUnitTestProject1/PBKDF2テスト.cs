using System;
using Xunit;

namespace PasswordHashing
{
	public class PBKDF2�e�X�g
	{
		//�����Ȃ�
		//System.Diagnostics.Trace.WriteLine("�e�X�g���b�Z�[�W\n\n\n");
		//System.Diagnostics.Debug.WriteLine(hash);

		private const string HUNDRED_STR = "1234567890223456789032345678904234567890523456789062345678907234567890823456789092345678909234567890";
		private const string HUNDRED_ONE_STR = HUNDRED_STR + "1";
		private const string SPECIAL_STR = "\"#$%&'()=^~\\|[{@`]}:*;+_/?.>,<";

		//�ϓ_�F���ؐ������邱��
		//���͂P�F�o�^���̃p�X���[�h
		//���͂Q�F���O�C�����̃p�X���[�h
		[Theory]
		[InlineData("abc", "abc")]
		[InlineData("", "")]
		[InlineData(HUNDRED_STR, HUNDRED_STR)]
		[InlineData(SPECIAL_STR, SPECIAL_STR)]
		[InlineData("\0", "")]
		[InlineData("\0a\0", "\0a")]	//�Ō��'\0'�͏I�[�����Ƃ��Ĉ�����B
		public void PBKDF2�e�X�g�P(string pass1, string pass2)
		{
			string hash = PBKDF2.Hash(pass1).ToString();
			Assert.True(PBKDF2.Verify(pass2, hash));
		}
		//�ϓ_�F���؎��s���邱��
		//���͂P�F�o�^���̃p�X���[�h
		//���͂Q�F���O�C�����̃p�X���[�h
		[Theory]
		[InlineData("abc", "abcd")]
		[InlineData("", "0")]
		[InlineData(HUNDRED_STR, HUNDRED_ONE_STR)]
		[InlineData(".", "+")]
		[InlineData("+", ".")]
		[InlineData(" ", "\t")]
		[InlineData("\0a\0a", "\0aa")]
		public void PBKDF2�e�X�g�Q(string pass1, string pass2)
		{
			string hash = PBKDF2.Hash(pass1).ToString();
			Assert.False(PBKDF2.Verify(pass2, hash));
		}
	}
}