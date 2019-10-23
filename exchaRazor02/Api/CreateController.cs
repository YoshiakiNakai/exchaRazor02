using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using exchaRazor02.Data;

namespace exchaRazor02.Api
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

        // GET: api/Create/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> GetDiary(string id)
        {
            var diary = await _context.diaries.FindAsync(id);

            if (diary == null)
            {
                return NotFound();
            }

            return diary.Id;
        }
    }
}
