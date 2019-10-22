using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

//DB用テーブル各行のデータ クラス宣言。
namespace exchaRazor02.Data
{
	//日記の記述ができるか
	public enum WRITA
	{
		able,   //できる
		disable,//できない
	}
	//日記交換できるか
	public enum EXCHA
	{
		able,   //できる
		disable,//できない
	}
	//日記の公開非公開
	public enum PUBLICITY
	{
		pub,	//公開
		pri,	//非公開
	}

	//日記データ
	public class Diary
	{
		const int maxId = 255;
		const int maxNote = 255;

		//コンストラクタ
		public Diary(){}
		public Diary(string Id, string pass, string note, DateTime last, PUBLICITY pub, EXCHA excha, WRITA writa, DateTime retTime, string exid)
		{
			this.Id = Id;
			this.pass = pass;
			this.note = note;
			this.last = last;
			this.pub = pub;
			this.excha = excha;
			this.writa = writa;
			this.retTime = retTime;
			this.exid = exid;
		}

		[MaxLength(maxId)]
		public string Id { get; set; }   //主キー	//慣例により「なんたらId」はキーとなる。注意。

		[Required]
		public string pass { get; set; }

		[MaxLength(maxNote)]
		public string note { get; set; }

		public DateTime last { get; set; }
		public PUBLICITY pub { get; set; }
		public EXCHA excha { get; set; }
		public WRITA writa { get; set; }
		public DateTime retTime { get; set; }
		public string exid { get; set; }

		//Navigation Property
		public List<Leaf> leaves { get; set; }   //leavesは、制約される。
	}

	//日記の１ページ１ページ
	public class Leaf
	{
		const int maxTitle = 255;
		const int maxContents = 65535;
		const int maxComment = 65535;

		//コンストラクタ
		public Leaf(){}
		public Leaf(string diaryId, DateTime time, string title, string contents, string exid, string comment)
		{
			this.diaryId = diaryId;
			this.time = time;
			this.title = title;
			this.contents = contents;
			this.exid = exid;
			this.comment = comment;
		}

		public string diaryId { get; set; }
		public DateTime time { get; set; }
		[MaxLength(maxTitle)]
		public string title { get; set; }
		[MaxLength(maxContents)]
		public string contents { get; set; }
		public string exid { get; set; }
		[MaxLength(maxComment)]
		public string comment { get; set; }

		//Navigation Property
		public Diary diary { get; set; }            //Diaryに、制約される。
		public List<Appli> appli { get; set; }   //appliは、制約される。
	}

	//交換の承諾結果
	public enum EXCHA_ACCEPT
	{
		accept, //承諾
		refuze,	//拒否
	}
	//交換申し込みの記録
	public class Appli
	{
		//コンストラクタ
		public Appli() { }
		public Appli(string diaryId, DateTime leafTime, string apid, EXCHA_ACCEPT accept )
		{
			this.diaryId = diaryId;
			this.leafTime = leafTime;
			this.apid = apid;
			this.accept = accept;
		}
		public string diaryId { get; set; }
		public DateTime leafTime { get; set; }
		public string apid { get; set; }
		public EXCHA_ACCEPT accept { get; set; }

		//Navigation Property
		public Leaf leaf { get; set; }         //Leafに、制約される。
	}
}
