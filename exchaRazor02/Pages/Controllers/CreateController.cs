using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using exchaRazor02.Data;

namespace exchaRazor02.Pages.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateController : ControllerBase
    {
        private readonly ExchaDContext9 _context;

        public CreateController(ExchaDContext9 context)
        {
            _context = context;
        }

		//POST: api/Create
		//日記IDが存在するか
		//戻り値：true ある、false ない
		[HttpPost]
		public ActionResult<bool> exist(string id)
		{
			//POSTデータを取得する
			// 引数で受け取る方法がわからないので、HttpContextから取得する
			var form = HttpContext.Request.Form;
			Microsoft.Extensions.Primitives.StringValues value;
			form.TryGetValue("id", out value);
			id = value.ToString();

			return (_context.diaries.Any(e => e.Id == id));
		}
	}
}
