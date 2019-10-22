using Microsoft.AspNetCore.Mvc;

namespace WebApp.Pages.Components.NavbarControl
{
	//ViewComponentを継承し、以下を実装する
	//・Invoke()		//同期でよいとき
	//・InvokeAsync()	//非同期がよいとき
	//cshtmlでは、どちらの実装にせよ@await InvokeAsync()で呼び出す。
	public class NavbarControlViewComponent : ViewComponent
	{
		public NavbarControlViewComponent() { }
		public IViewComponentResult Invoke()
			// 引数を渡すことも可能
			// cshtmlのInvokeAsync()では、
			// 引数1:呼び出すViewComponent
			// 引数2以降がここで定義する引数と繋がる
		{
			return View("Default", HttpContext.User);
			// 引数1：表示するcshtml
			// 引数2：渡す@model
		}
	}
}