using System;
using System.Runtime.Serialization.Json;

namespace Domain
{
    public class PlayerBoardState
    {
        public int PlayerBoardStateId { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;


        // serialized to json
        public string BoardState { get; set; } = null!;
    }
}