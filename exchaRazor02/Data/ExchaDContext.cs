using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace exchaRazor02.Data
{
	//DB定義
	public class ExchaDContext9 : DbContext
	{
		//コンストラクタ
		public ExchaDContext9(DbContextOptions<ExchaDContext9> options)
			: base(options)
		{
		}

		//テーブル定義
		public DbSet<Diary> diaries { get; set; }
		public DbSet<Leaf> leaves { get; set; }
		public DbSet<Appli> appli { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//複合主キーの定義
			modelBuilder.Entity<Leaf>()
				.HasKey(o => new { o.diaryid, o.time });	//get, setをつけていないとここでバグる

			modelBuilder.Entity<Appli>()
				.HasKey(o => new { o.diaryid, o.leaftime, o.apid });

			//外部制約の定義
			modelBuilder.Entity<Leaf>()	//制約される側
				.HasOne(leaf => leaf.diary) //Navigation Property（制約される側）
				.WithMany(diary => diary.leaves)	//Navigation Property（制約する側）
				.HasForeignKey(leaf => new { leaf.diaryid });   //foreign key

			modelBuilder.Entity<Appli>() //制約される側
				.HasOne(a => a.leaf) //Navigation Property（制約される側）
				.WithMany(l => l.appli)    //Navigation Property（制約する側）
				.HasForeignKey(a => new { a.diaryid, a.leaftime });   //foreign key
		}
	}
}
