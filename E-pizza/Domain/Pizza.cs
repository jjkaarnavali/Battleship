using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Pizza
    {
        public int PizzaId { get; set; }
        
        [MaxLength(32)]
        public string Name { get; set; } = default!;
        
        [MaxLength(32)]
        public string Category { get; set; } = default!;
        
        [MaxLength(32)]
        public string Description { get; set; } = default!;
        
        public int Price { get; set; }

        public ICollection<AddComponent> AddComponents { get; set; } = null!;
    }
}