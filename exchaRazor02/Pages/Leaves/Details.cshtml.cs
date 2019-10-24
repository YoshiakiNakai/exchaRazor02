using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using exchaRazor02.Data;

namespace exchaRazor02.Pages.Leaves
{
    public class DetailsModel : PageModel
    {
        private readonly exchaRazor02.Data.ExchaDContext5 _context;

        public DetailsModel(exchaRazor02.Data.ExchaDContext5 context)
        {
            _context = context;
        }

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
            return Page();
        }
    }
}
