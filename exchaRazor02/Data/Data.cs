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

		[Key]
		[Required]
		[MaxLength(255)]
		[Display(Name = "日記ID")]
		public string Id { get; set; }   //主キー	//慣例により「なんたらId」はキーとなる。注意。

		[Required]
		[MinLength(4)]
		[Display(Name = "日記の鍵")]
		public string pass { get; set; }

		[MaxLength(255)]
		[Display(Name = "付箋")]
		public string note { get; set; }

		[Display(Name = "最後の日付")]
		public DateTime last { get; set; }	//最終ログイン

		[Display(Name = "公開非公開")]
		public PUBLICITY pub { get; set; }

		public EXCHA excha { get; set; }	//交換可不可
		public WRITA writa { get; set; }    //記述可不可

		[Display(Name = "返す日")]
		public DateTime retTime { get; set; }	//
		public string exid { get; set; }	//交換相手

		//Navigation Property
		public List<Leaf> leaves { get; set; }   //leavesは、制約される。
	}

	//日記の１ページ１ページ
	public class Leaf
	{
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

		[MinLength(1)]
		[MaxLength(255)]
		[Display(Name = "タイトル")]
		public string title { get; set; }

		[MinLength(1)]
		[MaxLength(65535)]
		[Display(Name = "内容")]
		public string contents { get; set; }
		public string exid { get; set; }
		[MaxLength(65535)]
		[Display(Name = "コメント")]
		public string comment { get; set; }

		//Navigation Property
		public Diary diary { get; set; }            //Diaryに、制約される。
		public List<Appli> appli { get; set; }   //appliは、制約される。
	}

	//交換の承諾結果
	public enum EXCHA_ACCEPT
	{
		accept, //承諾
		reject,	//拒否
		yet,	//未
	}
	//交換申し込みの記録
	public class Appli
	{
		//コンストラクタ
		public Appli() { }
		public Appli(string diaryId, DateTime leafTime, string apid, EXCHA_ACCEPT accept, double period)
		{
			this.diaryId = diaryId;
			this.leafTime = leafTime;
			this.apid = apid;
			this.accept = accept;
			this.period = period;
		}
		public string diaryId { get; set; }
		public DateTime leafTime { get; set; }
		public string apid { get; set; }
		public EXCHA_ACCEPT accept { get; set; }
		public double period { get; set; }

		//Navigation Property
		public Leaf leaf { get; set; }         //Leafに、制約される。
	}
}
