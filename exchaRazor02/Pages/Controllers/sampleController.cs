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
    public class sampleController : ControllerBase
    {
        private readonly ExchaDContext9 _context;

        public sampleController(ExchaDContext9 context)
        {
            _context = context;
        }

        // GET: api/sample
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Diary>>> Getdiaries()
        {
            return await _context.diaries.ToListAsync();
        }

        // GET: api/sample/5
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

        // PUT: api/sample/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiary(string id, Diary diary)
        {
            if (id != diary.Id)
            {
                return BadRequest();
            }

            _context.Entry(diary).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiaryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/sample
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Diary>> PostDiary(Diary diary)
        {
            _context.diaries.Add(diary);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DiaryExists(diary.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDiary", new { id = diary.Id }, diary);
        }

        // DELETE: api/sample/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Diary>> DeleteDiary(string id)
        {
            var diary = await _context.diaries.FindAsync(id);
            if (diary == null)
            {
                return NotFound();
            }

            _context.diaries.Remove(diary);
            await _context.SaveChangesAsync();

            return diary;
        }

        private bool DiaryExists(string id)
        {
            return _context.diaries.Any(e => e.Id == id);
        }
    }
}
