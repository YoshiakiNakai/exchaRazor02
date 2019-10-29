using System;
using Xunit;

using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

using exchaRazor02.Pages;
using exchaRazor02.Data;
using PasswordHashing;

namespace XUnitTestProject1
{
	public class Create�e�X�g
	{
		private readonly ILogger<CreateModel> _logger;
		private CreateModel model;
		private ExchaDContext9 context;


		public Create�e�X�g()
		{
			var loggerFactory = LoggerFactory.Create(builder =>
			{
				builder
					.AddFilter("XUnitTestProject1", LogLevel.Information)
					.AddFilter("XUnitTestProject1.Create�e�X�g", LogLevel.Information)
					.AddFilter("XUnitTestProject1.Create�e�X�g.Create�e�X�g", LogLevel.Information)
					.AddConsole()
					.AddDebug()
					.AddEventLog();
			});
			_logger = loggerFactory.CreateLogger<CreateModel>();
			_logger.Log(LogLevel.Information, "Create�e�X�gLogger");
			_logger.Log(LogLevel.Warning, "Create�e�X�gLogger");
			_logger.Log(LogLevel.Error, "Create�e�X�gLogger");
			_logger.Log(LogLevel.Debug, "Create�e�X�gLogger");

			var options = new DbContextOptionsBuilder<ExchaDContext9>()
				.UseInMemoryDatabase(databaseName: "DbName")
				.Options;

			context = new ExchaDContext9(options);

			//id="dup"�����O�o�^����
			context.diaries.Add(new Diary("dup", "pass", "note", DateTime.Now, PUBLICITY.pub, EXCHA.able, WRITA.able, DateTime.Now, "exid"));

			model = new CreateModel(_logger, context);
		}

		//�ϓ_�FDB�ɓo�^�ł��邱��
		//���́FDiary�̃v���p�e�B
		[Theory]
		[InlineData("id", "pass", "note", null, PUBLICITY.pub, null, null, null, null)]
		[InlineData("_id", "pa55", null, null, PUBLICITY.pri, EXCHA.able, WRITA.disable, null, "exid")]
		public async void Create�e�X�g�P(string Id, string pass, string note, DateTime last, PUBLICITY pub, EXCHA excha, WRITA writa, DateTime retTime, string exid)
		{
			//POST�f�[�^�̃o�C���h
			model.Diary = new Diary(Id, pass, note, last, pub, excha, writa, retTime, exid);

			//POST���̏���
			await model.OnPostAsync();

			//DB����f�[�^�̎��o��
			var d = await context.diaries.FindAsync(Id);

			//���͂�DB�̒l����v���邩
			Assert.Equal(Id, d.Id);     //(expected, actual)
			Assert.Equal(note, d.note);
			Assert.Equal(DateTime.Today, d.last.Date);  //���t�����m�F
			Assert.Equal(pub, d.pub);

			//�p�X���[�h�̓n�b�V��������Ă��邩
			Assert.True(PBKDF2.Verify(pass, d.pass));

			//���͂Ɋւ�炸�A�����l�������Ă��邩
			Assert.Equal(EXCHA.disable, d.excha);
			Assert.Equal(WRITA.able, d.writa);
			Assert.Equal(DateTime.Today, d.retTime.Date);
			Assert.Null(d.exid);
		}

		//�ϓ_�FDB�ɓo�^����Ȃ�����
		//���́FDiary�̃v���p�e�B
		[Theory]
		[InlineData("dup", "pass", "note", PUBLICITY.pub)]
		[InlineData(null, "Id_Null", "note", PUBLICITY.pub)]
		[InlineData("Pass_Null", null, "note", PUBLICITY.pub)]
		public async void Create�e�X�g�Q(string Id, string pass, string note, PUBLICITY pub)
		{
			//POST�f�[�^�̃o�C���h
			model.Diary.Id = Id;
			model.Diary.pass = pass;
			model.Diary.note = note;
			model.Diary.pub = pub;

			//POST���̏���
			//await model.OnPostAsync();

			//Assert.Throws<Exception>(
			//delegate
			//{
			//	//POST���̏���
			//	model.OnPostAsync();
			//});

			//DB����f�[�^�̎��o��
			//var d = await context.diaries.FindAsync(Id);
		}
	}
}
