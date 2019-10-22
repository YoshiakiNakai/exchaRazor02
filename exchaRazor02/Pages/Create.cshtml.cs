﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using exchaRazor02.Data;

namespace exchaRazor02.Pages
{
    public class CreateModel : PageModel
    {
        private readonly exchaRazor02.Data.ExchaDContext5 _context;

        public CreateModel(exchaRazor02.Data.ExchaDContext5 context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Diary Diary { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.diaries.Add(Diary);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
