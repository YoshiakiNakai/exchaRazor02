using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using exchaRazor02.Data;

namespace exchaRazor02.Pages
{
    public class DiariesModel : PageModel
    {
        private readonly exchaRazor02.Data.ExchaDContext9 _context;

        public DiariesModel(exchaRazor02.Data.ExchaDContext9 context)
        {
            _context = context;
        }

        public IList<Diary> Diaries { get;set; }

        public async Task OnGetAsync()
        {
            Diaries = await _context.diaries.OrderByDescending(d => d.last).ToListAsync();
        }
    }
}
