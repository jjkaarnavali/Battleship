using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain
{
    public class Player
    {
        public int PlayerId { get; set; }
        [MaxLength(128)] public string Name { get; set; } = null!;

        public EPlayerType EPlayerType { get; set; }

        // option A
        // dont reference back to GAME at all - OK
        
        // option B
        // have single optional reference back to GAME - OK
        // you need to update the Players after game and players are created in db already

        
        public int? GameId { get; set; } // is this optional? at least initially, before game is created
        public Game Game { get; set; } = null!;
        
        
        // option C
        // have two references to GAME
        /*
        public int? GameAId { get; set; } // is this optional? at least initially, before game is created
        public Game GameA { get; set; } = null!;
        public int? GameBId { get; set; } // is this optional? at least initially, before game is created
        public Game GameB { get; set; } = null!;
        */
        
        public ICollection<GameBoat> GameBoats { get; set; } = null!;

        public ICollection<PlayerBoardState> PlayerBoardStates { get; set; } = null!;
    }
}