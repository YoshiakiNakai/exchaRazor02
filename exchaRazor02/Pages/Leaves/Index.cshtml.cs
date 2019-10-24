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
    public class IndexModel : PageModel
    {
        private readonly exchaRazor02.Data.ExchaDContext5 _context;

        public IndexModel(exchaRazor02.Data.ExchaDContext5 context)
        {
            _context = context;
        }

        public IList<Leaf> Leaf { get;set; }

        public async Task OnGetAsync()
        {
            Leaf = await _context.leaves
                .Include(l => l.diary).ToListAsync();
        }
    }
}
