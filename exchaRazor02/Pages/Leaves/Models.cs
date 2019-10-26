using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;
using exchaRazor02.Data;


namespace exchaRazor02.Pages.Leaves
{
	//日記の権限確認を行う
	public static class DiaryAuth
	{
		//日記を閲覧する権限があるか
		//引数１：アクセスユーザ
		//引数２：日記
		//戻り値：true 読める、false 読めない
		public static bool authRead(ClaimsPrincipal user, Diary diary)
		{
			bool flag = false;  //戻り値：閲覧可不可フラグ

			//日記を閲覧権限があるか、確認する
			//処理概要：
			// 交換中、かつ、交換者：表示
			// 交換中：非表示
			// 公開：表示
			// 非公開、かつ、持ち主：表示
			// 非公開：非表示

			//交換相手がいるか
			if (diary.exid != null) {
				//交換中のとき、交換相手にのみ表示する
				//未ログインか
				if (!user.Identity.IsAuthenticated) {
					//未ログインのとき、表示しない
				}//ログイン中のとき
				//交換者か
				else if (diary.exid == user.FindFirst(ClaimTypes.NameIdentifier).Value) {
					flag = true;
				}
			}//交換中でないとき
			//公開か
			else if(diary.pub == PUBLICITY.pub) {
				//公開のとき、表示する
				flag = true;
			}//非公開のとき
			//未ログインか
			else if(!user.Identity.IsAuthenticated) {
				//未ログインのとき、表示しない
			}//ログイン中のとき
			//持ち主ならば、表示する
			if (diary.Id == user.FindFirst(ClaimTypes.NameIdentifier).Value){
				flag = true;
			}
			return flag;
		}

		//Leafを作成する権限があるか
		//引数１：アクセスユーザ
		//引数２：日記
		//戻り値：true 作成可能、false 不可能
		public static bool authCreateLeaf(ClaimsPrincipal user, Diary diary)
		{
			bool flag = false;  //戻り値：作成可不可フラグ

			//Leafを作成する権限があるか、確認する
			// 交換中でない、かつ、持ち主、かつ、作成許可フラグON、ならば可能

			//未ログインか
			if (!user.Identity.IsAuthenticated) {
				//未ログインのとき、不可能
			}//ログイン中のとき
			else if (
				(diary.Id == user.FindFirst(ClaimTypes.NameIdentifier).Value)	//日記の持ち主
				&& (diary.writa == WRITA.able)	//作成可能
				&& (diary.exid == null)	//交換相手なし
				) {
				flag = true;
			}

			return flag;
		}

		//Leafを編集する権限があるか
		//引数１：アクセスユーザ
		//引数２：日記
		//戻り値：true 編集可能、false 不可能
		public static async Task<bool> authEditLeaf(ClaimsPrincipal user, ExchaDContext5 context, Leaf leaf)
		{
			bool flag = false;  //戻り値：編集可不可フラグ

			Diary diary = await context.diaries.FindAsync(leaf.diaryId);    //日記を取得
			if (diary == null) return false;    //日記がないとき、不可能

			//最新のleafの日時を取得する
			DateTime latest = await context.leaves
				.Where(l => l.diaryId == leaf.diaryId)
				.MaxAsync(l => l.time);

			//Leafを編集する権限があるか、確認する
			// 持ち主、かつ、交換中でない、かつ、最新leaf、かつ、コメント者なし、ならば可能

			//未ログインか
			if (!user.Identity.IsAuthenticated) {
				//未ログインのとき、不可能
			}//ログイン中のとき
			else if (
				(diary.Id == user.FindFirst(ClaimTypes.NameIdentifier).Value)   //日記の持ち主
				&& (leaf.time == latest)    //最新leaf
				&& (diary.exid == null)		//交換相手なし
				&& (leaf.exid == null)		//コメント者なし
				) {
				flag = true;
			}

			return flag;
		}

		//Leafへコメントする権限があるか
		//引数１：アクセスユーザ
		//引数２：DB
		//引数３：コメントするleaf
		//戻り値：true コメント可能、false 不可能
		public static async Task<bool> authCommentLeaf(ClaimsPrincipal user, ExchaDContext5 context, Leaf leaf)
		{
			bool flag = false;  //戻り値：コメント可不可フラグ

			Diary diary = await context.diaries.FindAsync(leaf.diaryId);	//日記を取得
			if (diary == null) return false;    //日記がないとき、不可能

			//最新のleafの日時を取得する
			DateTime latest = await context.leaves
				.Where(l => l.diaryId == leaf.diaryId)
				.MaxAsync(l => l.time);

			//Leafへコメントする権限があるか、確認する
			//交換相手のとき、かつ、最新のLeaf、ならば可能

			if (!user.Identity.IsAuthenticated) {
				//未ログインのとき、不可能
			}//ログイン中のとき
			else if (
				(user.FindFirst(ClaimTypes.NameIdentifier).Value == diary.exid) //交換相手
				&& (leaf.time == latest)	//最新leaf
				) {
				flag = true;
			}

			return flag;
		}
	}
}
