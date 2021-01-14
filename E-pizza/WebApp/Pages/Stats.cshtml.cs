
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages
{
    public class Stats : PageModel
    {
        private readonly DAL.ApplicationDbContext _context;

        public Stats(DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Order> OrderList { get;set; } = null!;
        
        public ICollection<Pizza> PizzaList { get;set; } = null!;
        
        public IList<AddComponent> AddComponentList { get;set; } = null!;
        
        public int PizzasSold { get;set; }
        public int Revenue { get;set; }
        
        [BindProperty]
        public Order Order { get; set; } = null!;
        

        public async Task OnGetAsync()
        {
            
            PizzaList = _context.Pizzas.ToList();
            OrderList = await _context.Orders.ToListAsync();
            AddComponentList = await _context.AddComponents.ToListAsync();
            PizzaList = _context.Pizzas.ToList();
            PizzasSold = 0;
            Revenue = 0;
            
            foreach (var order in OrderList)
            {
                Order = await _context.Orders.FirstOrDefaultAsync(m => m.OrderId == order.OrderId);
                
                if (Order.BuyerName.Length > 0 && Order.Pizzas != null)
                {
                    PizzasSold += Order.Pizzas.Count;
                }
            }
            
            
            foreach (var order in OrderList)
            {
                if (order.BuyerName.Length > 0)
                {
                    Revenue += order.Price;
                }
                
            }
            
        }
    }
}
