using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Components
{
    public class AddCompToPizza : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public AddCompToPizza(DAL.ApplicationDbContext context)
        {
            _context = context;
        }
        
        [BindProperty]
        public int OrderId { get; set; }
        [BindProperty]
        public int PizzaId { get; set; }

        [BindProperty]
        public string IsSearch { get; set; } = null!;

        [BindProperty] 
        public string ToppingName { get; set; } = null!;
        [BindProperty] 
        public int CorrectToppingId { get; set; }

        [BindProperty]
        public Pizza Pizza { get; set; } = null!;
        
        [BindProperty]
        public int CompId { get; set; }
        [BindProperty]
        public string AddToPizza { get; set; } = null!;
        
        [BindProperty]
        public string AddToOrder { get; set; } = null!;
        
        [BindProperty]
        public string CompIds { get; set; } = null!;
        
        public IList<AddComponent> AddComponent { get;set; } = null!;
        
        

        public async Task OnGetAsync(int orderId, int pizzaId, string? ids, int? correctToppingId)
        {
            AddComponent = await _context.AddComponents.ToListAsync();

            OrderId = orderId;
            
            PizzaId = pizzaId;
            Pizza = _context.Pizzas.Find(PizzaId);

            if (correctToppingId != null)
            {
                CorrectToppingId = correctToppingId.Value;
            }
            else
            {
                CorrectToppingId = 0;
            }
            
            
            

            if (ids != null)
            {
                CompIds = ids;
            }
            
            
        }
        
        public async Task<IActionResult> OnPostAsync()
        {

            if (IsSearch == "yes")
            {
                AddComponent = await _context.AddComponents.ToListAsync();

                foreach (var component in AddComponent)
                {
                    if (component.CompName == ToppingName)
                    {
                        CorrectToppingId = component.AddComponentId;
                    }
                }
                return RedirectToPage("AddCompToPizza", new { ids = CompIds, orderId = OrderId, pizzaId = PizzaId, correctToppingId = CorrectToppingId});
            }
            
            if (AddToPizza == "yes")
            {
                
                if (CompIds != null && !CompIds.Contains(CompId.ToString()))
                {
                    CompIds += ",";
                    CompIds += CompId;
                }else if (CompIds == null)
                {
                    CompIds += CompId;
                }

                return RedirectToPage("AddCompToPizza", new { orderId = OrderId, pizzaId = PizzaId, ids = CompIds});
            }

            if (AddToOrder == "yes")
            {
                return RedirectToPage("../Index", new { orderId = OrderId, pizzaId = PizzaId, compIds = CompIds});
            }
            return RedirectToPage("AddCompToPizza");
        }
    }
}