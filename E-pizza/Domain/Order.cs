using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Order
    {
        public int OrderId { get; set; }
        
        [MaxLength(32)]
        public string BuyerName { get; set; } = default!;

        public int Price { get; set; }

        
        public ICollection<Pizza> Pizzas { get; set; } = null!;
    }
}