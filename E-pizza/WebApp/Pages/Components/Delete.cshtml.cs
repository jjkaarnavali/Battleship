using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_Components
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public DeleteModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AddComponent AddComponent { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AddComponent = await _context.AddComponents.FirstOrDefaultAsync(m => m.AddComponentId == id);

            if (AddComponent == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AddComponent = await _context.AddComponents.FindAsync(id);

            if (AddComponent != null)
            {
                _context.AddComponents.Remove(AddComponent);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
