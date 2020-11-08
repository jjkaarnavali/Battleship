using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameOptionBoat
    {
        public int GameOptionBoatId { get; set; }
        
        [Range(1, int.MaxValue)]
        public int Amount { get; set; }

        // TODO: add unique index over BoatId and GameOptionId
        public int BoatId { get; set; }
        public Boat Boat { get; set; } = null!;

        public int GameOptionId { get; set; }
        public GameOption GameOption { get; set; } = null!;
    }
}