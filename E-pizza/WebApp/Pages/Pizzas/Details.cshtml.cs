using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_Pizzas
{
    public class DetailsModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public DetailsModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public Pizza Pizza { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pizza = await _context.Pizzas.FirstOrDefaultAsync(m => m.PizzaId == id);

            if (Pizza == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
