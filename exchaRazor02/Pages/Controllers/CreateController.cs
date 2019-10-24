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
        private readonly ExchaDContext5 _context;

        public CreateController(ExchaDContext5 context)
        {
            _context = context;
        }

        // GET: api/Create
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Diary>>> Getdiaries()
        {
            return await _context.diaries.ToListAsync();
        }

        // GET: api/Create/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Diary>> GetDiary(string id)
        {
            var diary = await _context.diaries.FindAsync(id);

            if (diary == null)
            {
                return NotFound();
            }

            return diary;
        }

		// POST: api/Create
		[HttpPost]
		public async Task<ActionResult<bool>> exist(string id)
		{
			var form = HttpContext.Request.Form;
			Microsoft.Extensions.Primitives.StringValues value;
			form.TryGetValue("id", out value);
			id = value.ToString();

			return _context.diaries.Any(e => e.Id == id);
		}

		private bool DiaryExists(string id)
		{
			return _context.diaries.Any(e => e.Id == id);
		}

	}
}
