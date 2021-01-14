using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_Orders
{
    public class DetailsModel : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public DetailsModel(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; } = null!;
        public Pizza Pizza { get; set; } = null!;
        public IList<AddComponent> AddComponentList { get; set; } = null!;
        
        public IList<Order> OrderList { get;set; } = null!;
        
        public IList<Pizza> PizzaList { get;set; } = null!;
        
        public IList<Pizza> OrderPizzaList { get;set; } = null!;

        public async Task OnGetAsync(int? id)
        {
            

            OrderList = await _context.Orders.ToListAsync();
            PizzaList = await _context.Pizzas.ToListAsync();
            AddComponentList = await _context.AddComponents.ToListAsync();
            
            Order = await _context.Orders.FirstOrDefaultAsync(m => m.OrderId == id);
            
            if (Order.Pizzas != null)
            {
                OrderPizzaList = new Collection<Pizza>();
                
                foreach (var pizza in Order.Pizzas)
                {
                   
                    foreach (var corPizza in PizzaList)
                    {
                        if (pizza.PizzaId == corPizza.PizzaId)
                        {
                            OrderPizzaList.Add(corPizza);
                        }
                    }
                    
                    
                    
                }
            }
            
            

            
        }
    }
}
