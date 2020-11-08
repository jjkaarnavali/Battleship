using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameBoat
    {
        public int GameBoatId { get; set; }

        [Range(1, int.MaxValue)]
        public int Size { get; set; }

        [MaxLength(32)] public string Name { get; set; } = null!;

        public bool IsSunken { get; set; }
        
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;

    }
    
}