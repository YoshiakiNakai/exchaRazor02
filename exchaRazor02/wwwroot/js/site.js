
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
