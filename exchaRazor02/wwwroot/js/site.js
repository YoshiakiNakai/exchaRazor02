
//document.getElementById()の省略
l = document.getElementById.bind(document);

//HTMLフォームの形式にデータを変換する  //"application/x-www-form-urlencoded"
//引数１：データ。例、{ param1: 'abc', param2: 100 }
//戻り値：変換後データ。例、"param1=abc&param2=100"
function encodeHTMLForm(data) {
	let params = [];    //データの入れ物を宣言
	//データ形式を変換する
	//例、param1=abc&param2=100
	for (let name in data) {
		//URI形式にエンコードして、'='で繋げる。
		let value = data[name];
		let param = encodeURIComponent(name) + '=' + encodeURIComponent(value);
		params.push(param);
	}
	//配列を'&'で結合する   //半角スペース%20を'+'に置き換える。
	return params.join('&').replace(/%20/g, '+');
}


//交換申請
function apply() {

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
                console.log("成功");
            } else {
                console.log("失敗");
			}
		}
	}
	//送信
	console.log("send: " + encodeHTMLForm(data));
	xhr.send(encodeHTMLForm(data));
}

//交換承諾する
function accept() {

    var data = {
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
                console.log("成功");
            } else {
                console.log("失敗");
			}
		}
	}
	//送信
	console.log("send: " + encodeHTMLForm(data));
	xhr.send(encodeHTMLForm(data));
}

//交換拒否する
function reject() {

    var data = {
        "exid": l('diaryId').value,
		"token": l('token').value,
    };

	//XMLHttpRequestの設定
	var xhr = new XMLHttpRequest();
	xhr.open("POST", '/api/Appli/reject', true);
	xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

	//イベントハンドラの登録
	xhr.onreadystatechange = function () { // 状態が変化すると関数が呼び出されます。
        if (this.readyState === XMLHttpRequest.DONE) {
			console.log("response: " + this.responseText);
            if (this.responseText === "true") {
                console.log("成功");
            } else {
                console.log("失敗");
			}
		}
	}
	//送信
	console.log("send: " + encodeHTMLForm(data));
	xhr.send(encodeHTMLForm(data));
}
