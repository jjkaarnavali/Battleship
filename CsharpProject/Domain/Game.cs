using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Game
    {
        public int GameId { get; set; }

        public int GameOptionId { get; set; }
        public GameOption GameOption { get; set; } = null!;

        [MaxLength(512)]
        public string Description { get; set; } = DateTime.Now.ToLongDateString();

        public int PlayerAId { get; set; }
        //[ForeignKey(nameof(PlayerAId))]
        public Player PlayerA { get; set; } = null!;

        public int PlayerBId { get; set; }
        //[ForeignKey(nameof(PlayerBId))]
        public Player PlayerB { get; set; } = null!;
        
        // serialized to json
        public string BoardState { get; set; } = null!;
        
    }
}