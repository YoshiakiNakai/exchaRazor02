using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using exchaRazor02.Data;

namespace exchaRazor02.Pages
{
	[Authorize]
	public class appliModel : PageModel
    {
        private readonly exchaRazor02.Data.ExchaDContext8 _context;

        public appliModel(exchaRazor02.Data.ExchaDContext8 context)
        {
            _context = context;
        }

        public IList<Appli> appli { get; set; }


        public async Task OnGetAsync()
        {
			string authId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			IQueryable<Leaf> leaves = _context.leaves.Where(l => l.diaryId == authId);
			if (leaves.Count() == 0) return;

			DateTime latest = await leaves.MaxAsync(l => l.time);
			appli = await _context.appli
				.Where(a =>
					(a.diaryId == authId)
					&& (a.leafTime == latest)
					&& (a.accept == EXCHA_ACCEPT.yet))
				.ToListAsync();
		}
	}
}
