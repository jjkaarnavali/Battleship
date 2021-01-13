using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public string AddToOrder { get; set; } = null!;
        
        [BindProperty]
        public List<int> PizzaIds { get; set; }  = new List<int>();
        
        [BindProperty]
        public int PizzaId { get; set; }

        [BindProperty]
        public int OrderId { get; set; } = 0;
        
        [BindProperty]
        public Order Order { get; set; } = null!;
        [BindProperty]
        public string OrderConfirmed { get; set; } = null!;
        [BindProperty]
        public string BuyerName { get; set; } = "Random";
        [BindProperty]
        public string OrderPlaced { get; set; } = null!;
        
        public async Task OnGetAsync(List<int>? ids, int? orderId, int? pizzaId, string? compIds, string? orderPlaced)
        {

            if (orderId != null && orderPlaced == "yes")
            {
                OrderId = orderId.Value;
                OrderConfirmed = "yes";
                Order = await _context.Orders.FirstOrDefaultAsync(m => m.OrderId == OrderId);
            }
            if (ids != null)
            {
                PizzaIds = ids;
            }
            if (orderId != null)
            {
                OrderId = orderId.Value;
            }
            Pizzas = await _context.Pizzas
                .OrderBy(x => x.PizzaId)
                .ToListAsync();
            if (orderId != null && pizzaId != null)
            {
                Order = await _context.Orders.FirstOrDefaultAsync(m => m.OrderId == orderId);
                Pizza pizza = _context.Pizzas.Find(pizzaId);
                Pizza pizzaClone = new Pizza();
                
                pizzaClone.Category = pizza.Category;
                pizzaClone.Description = "Ordered pizza";
                pizzaClone.Price = pizza.Price;
                pizzaClone.Name = pizza.Name;
                pizzaClone.AddComponents = new Collection<AddComponent>();
                if (compIds != null)
                {
                    string[] extraComps = compIds.Split(",");
                    List<int> extraCompIds = new List<int>();
                    foreach (var s in extraComps)
                    {
                        extraCompIds.Add(int.Parse(s));
                    }

                    foreach (var i in extraCompIds)
                    {
                        AddComponent comp = _context.AddComponents.Find(i);
                        pizzaClone.AddComponents.Add(comp);
                        pizzaClone.Price += comp.Price;
                    }
                }
                if (Order.Pizzas == null)
                {
                    Order.Pizzas = new Collection<Pizza>();
                }
                Order.Pizzas.Add(pizzaClone);
                Order.Price += pizzaClone.Price;
                _context.Orders.Update(Order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Pizzas = await _context.Pizzas
                .OrderBy(x => x.PizzaId)
                .ToListAsync();

            if (OrderConfirmed == "yes")
            {
                Order = await _context.Orders.FirstOrDefaultAsync(m => m.OrderId == OrderId);
                Order.BuyerName = BuyerName;
                _context.Orders.Update(Order);
                await _context.SaveChangesAsync();
                OrderPlaced = "yes";
                return RedirectToPage("Index", new {orderId = OrderId, orderPlaced = OrderPlaced});
            }
            
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
                
                
                return RedirectToPage("Index", new { ids = PizzaIds, orderId = OrderId});
            }

            if (AddToOrder == "yes" && OrderId == 0)
            {
                Order order = new Order();
                order.Price = 0;
                order.BuyerName = "";
                
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                int id = order.OrderId;
                return RedirectToPage("./Components/AddCompToPizza", new { orderId = id, pizzaId = PizzaId });
            }else if (AddToOrder == "yes" && OrderId != 0)
            {
                return RedirectToPage("./Components/AddCompToPizza", new { orderId = OrderId, pizzaId = PizzaId });
            }
            return RedirectToPage("Index");
        }
    }
}