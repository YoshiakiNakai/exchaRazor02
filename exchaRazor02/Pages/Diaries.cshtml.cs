﻿using System;
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
        private readonly exchaRazor02.Data.ExchaDContext5 _context;

        public DiariesModel(exchaRazor02.Data.ExchaDContext5 context)
        {
            _context = context;
        }

        public IList<Diary> Diary { get;set; }

        public async Task OnGetAsync()
        {
            Diary = await _context.diaries.ToListAsync();
        }
    }
}