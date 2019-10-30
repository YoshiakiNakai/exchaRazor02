
//交換申請する
//引数１：この関数を呼び出した要素
function apply(button) {
    button.disabled = true;
    var data = {
        "diaryId": l('diaryId').value,
		"exchaPeriod": l('exchaPeriod').value,
		"token": l('token').value,
    };

	//XMLHttpRequestの設定
	var xhr = new XMLHttpRequest();
	xhr.open("POST", '/api/Appli/apply', true);
	xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

	//イベントハンドラの登録
	xhr.onreadystatechange = function () { // 状態が変化すると関数が呼び出されます。
        if (this.readyState === XMLHttpRequest.DONE) {
			console.log("response: " + this.responseText);
            if (this.responseText === "true") {
                //console.log("成功");
                alert('交換申請を出しました');
            } else {
                //console.log("失敗");
                button.disabled = false;
                alert('通信に失敗しました');
			}
		}
	}
	//送信
	console.log("send: " + encodeHTMLForm(data));
	xhr.send(encodeHTMLForm(data));
}

//交換承諾する
function reply(button) {
    l('repAccept').disabled = true;
    l('repReject').disabled = true;

    var data = {
        "excha": button.value,
        "exid": l('diaryId').value,
		"token": l('token').value,
    };

	//XMLHttpRequestの設定
	var xhr = new XMLHttpRequest();
	xhr.open("POST", '/api/Appli/accept', true);
	xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

	//イベントハンドラの登録
	xhr.onreadystatechange = function () { // 状態が変化すると関数が呼び出されます。
        if (this.readyState === XMLHttpRequest.DONE) {
			console.log("response: " + this.responseText);
            if (this.responseText === "true") {
                alert('回答しました');
            } else {
                alert('通信に失敗しました');
			}
		}
	}
	//送信
	console.log("send: " + encodeHTMLForm(data));
	xhr.send(encodeHTMLForm(data));
}
