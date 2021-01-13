using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DAL.ApplicationDbContext _context;

        public IndexModel(DAL.ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Pizza> Pizzas { get;set; } = null!;
        
        [BindProperty]
        public string PizzaName { get; set; } = null!;
        [BindProperty]
        public string Category { get; set; } = null!;

        [BindProperty]
        public string Description { get; set; } = null!;
        [BindProperty]
        public string IsSearch { get; set; } = null!;
        
        [BindProperty]
        public List<int> PizzaIds { get; set; }  = new List<int>();
        
        public async Task OnGetAsync(List<int>? ids)
        {

            if (ids != null)
            {
                PizzaIds = ids;
            }
            Pizzas = await _context.Pizzas
                .OrderBy(x => x.PizzaId)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Pizzas = await _context.Pizzas
                .OrderBy(x => x.PizzaId)
                .ToListAsync();
            
            if (IsSearch == "new")
            {
                for (int i = 0; i < Pizzas.Count; i++)
                {
                    if (Category == null)
                    {
                        Category = "";
                    }
                    if (PizzaName == null)
                    {
                        PizzaName = "";
                    }
                    
                    if (Pizzas[i].Category == Category)
                    {
                        
                        PizzaIds.Add(Pizzas[i].PizzaId);
                    }

                    if (Pizzas[i].Name == PizzaName && !PizzaIds.Contains(Pizzas[i].PizzaId))
                    {
                        PizzaIds.Add(Pizzas[i].PizzaId);
                    }else if (PizzaName != "" && Pizzas[i].Name != PizzaName && PizzaIds.Contains(Pizzas[i].PizzaId))
                    {
                        PizzaIds.Remove(Pizzas[i].PizzaId);
                    }
                    
                    if (Description != null && Pizzas[i].Description.Contains(Description) && !PizzaIds.Contains(Pizzas[i].PizzaId))
                    {
                        PizzaIds.Add(Pizzas[i].PizzaId);
                    }else if (Description != null && !Pizzas[i].Description.Contains(Description) && PizzaIds.Contains(Pizzas[i].PizzaId))
                    {
                        PizzaIds.Remove(Pizzas[i].PizzaId);
                    }
                }
                
                
                return RedirectToPage("Index", new { ids = PizzaIds});
            }
            return RedirectToPage("Index");
        }
    }
}