using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class AddComponent
    {
        public int AddComponentId { get; set; }

        [MaxLength(32)]
        public string CompName { get; set; } = default!;
        
        public int Price { get; set; }
        

    }
}