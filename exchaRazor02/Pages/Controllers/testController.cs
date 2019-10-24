using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using exchaRazor02.Data;

namespace exchaRazor02.Pages
{
    [Route("api/[controller]")]
    [ApiController]
    public class testController : ControllerBase
    {
        private readonly ExchaDContext5 _context;

        public testController(ExchaDContext5 context)
        {
            _context = context;
        }

        // GET: api/test
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Diary>>> Getdiaries()
        {
            return await _context.diaries.ToListAsync();
        }

        // GET: api/test/5
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

        private bool DiaryExists(string id)
        {
            return _context.diaries.Any(e => e.Id == id);
        }
    }
}
