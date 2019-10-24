using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using exchaRazor02.Data;

namespace exchaRazor02.Pages.Leaves
{
    public class EditModel : PageModel
    {
        private readonly exchaRazor02.Data.ExchaDContext5 _context;

        public EditModel(exchaRazor02.Data.ExchaDContext5 context)
        {
            _context = context;
        }

        [BindProperty]
        public Leaf Leaf { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Leaf = await _context.leaves
                .Include(l => l.diary).FirstOrDefaultAsync(m => m.diaryId == id);

            if (Leaf == null)
            {
                return NotFound();
            }
           ViewData["diaryId"] = new SelectList(_context.diaries, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Leaf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeafExists(Leaf.diaryId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool LeafExists(string id)
        {
            return _context.leaves.Any(e => e.diaryId == id);
        }
    }
}
