
//パスワード形式確認
function confirmPass2() {
	pass1 = l('Diary_pass');
	pass2 = l('Diary_pass2');
	alarm2 = l('pass_alarm2');

	//入力パスワード1, 2の一致チェック
	if (pass1.value != pass2.value) {
		//一致していないとき
		alarm2.innerText = "パスワードが一致していません";
	} else {
		//形式OKのとき
		alarm2.innerText = "";
	}
}

//ID重複確認
//引数１：この関数を呼び出した要素
function checkDuplicateKey(diaryId) {   //l()と同一のインスタンス

	var data = { "id": diaryId.value }; // POSTメソッドで送信するデータ

	//XMLHttpRequestの設定
	var xhr = new XMLHttpRequest();
	xhr.open("POST", '/api/Create', true);
	xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

	//イベントハンドラの登録
	xhr.onreadystatechange = function () { // 状態が変化すると関数が呼び出されます。
		switch (this.readyState) {
			case XMLHttpRequest.LOADING:
				console.log("loading");
				break;
			case XMLHttpRequest.DONE:
				console.log("response: " + this.responseText);
				break;
		}
	}
	//送信
	console.log("send: " + encodeHTMLForm(data));
	xhr.send(encodeHTMLForm(data));
}
