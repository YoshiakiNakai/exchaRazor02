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
	public class Createテスト
	{
		private readonly ILogger<CreateModel> _logger;
		private CreateModel model;
		private ExchaDContext9 context;


		public Createテスト()
		{
			var loggerFactory = LoggerFactory.Create(builder =>
			{
				builder
					.AddFilter("XUnitTestProject1", LogLevel.Information)
					.AddFilter("XUnitTestProject1.Createテスト", LogLevel.Information)
					.AddFilter("XUnitTestProject1.Createテスト.Createテスト", LogLevel.Information)
					.AddConsole()
					.AddDebug()
					.AddEventLog();
			});
			_logger = loggerFactory.CreateLogger<CreateModel>();
			_logger.Log(LogLevel.Information, "CreateテストLogger");
			_logger.Log(LogLevel.Warning, "CreateテストLogger");
			_logger.Log(LogLevel.Error, "CreateテストLogger");
			_logger.Log(LogLevel.Debug, "CreateテストLogger");

			var options = new DbContextOptionsBuilder<ExchaDContext9>()
				.UseInMemoryDatabase(databaseName: "DbName")
				.Options;

			context = new ExchaDContext9(options);

			//id="dup"を事前登録する
			context.diaries.Add(new Diary("dup", "pass", "note", DateTime.Now, PUBLICITY.pub, EXCHA.able, WRITA.able, DateTime.Now, "exid"));

			model = new CreateModel(_logger, context);
		}

		//観点：DBに登録できること
		//入力：Diaryのプロパティ
		[Theory]
		[InlineData("id", "pass", "note", null, PUBLICITY.pub, null, null, null, null)]
		[InlineData("_id", "pa55", null, null, PUBLICITY.pri, EXCHA.able, WRITA.disable, null, "exid")]
		public async void Createテスト１(string Id, string pass, string note, DateTime last, PUBLICITY pub, EXCHA excha, WRITA writa, DateTime retTime, string exid)
		{
			//POSTデータのバインド
			model.Diary = new Diary(Id, pass, note, last, pub, excha, writa, retTime, exid);

			//POST時の処理
			await model.OnPostAsync();

			//DBからデータの取り出し
			var d = await context.diaries.FindAsync(Id);

			//入力とDBの値が一致するか
			Assert.Equal(Id, d.Id);     //(expected, actual)
			Assert.Equal(note, d.note);
			Assert.Equal(DateTime.Today, d.last.Date);  //日付だけ確認
			Assert.Equal(pub, d.pub);

			//パスワードはハッシュ化されているか
			Assert.True(PBKDF2.Verify(pass, d.pass));

			//入力に関わらず、初期値が入っているか
			Assert.Equal(EXCHA.disable, d.excha);
			Assert.Equal(WRITA.able, d.writa);
			Assert.Equal(DateTime.Today, d.retTime.Date);
			Assert.Null(d.exid);
		}

		//観点：DBに登録されないこと
		//入力：Diaryのプロパティ
		[Theory]
		[InlineData("dup", "pass", "note", PUBLICITY.pub)]
		[InlineData(null, "Id_Null", "note", PUBLICITY.pub)]
		[InlineData("Pass_Null", null, "note", PUBLICITY.pub)]
		public async void Createテスト２(string Id, string pass, string note, PUBLICITY pub)
		{
			//POSTデータのバインド
			model.Diary.Id = Id;
			model.Diary.pass = pass;
			model.Diary.note = note;
			model.Diary.pub = pub;

			//POST時の処理
			//await model.OnPostAsync();

			//Assert.Throws<Exception>(
			//delegate
			//{
			//	//POST時の処理
			//	model.OnPostAsync();
			//});

			//DBからデータの取り出し
			//var d = await context.diaries.FindAsync(Id);
		}
	}
}
