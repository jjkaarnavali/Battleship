namespace Domain
{
    public class BoardSquareState
    {
        // this is a value from GameBoat.GameBoatId
        public int? BoatId { get; set; }
        
        // 0 - no bomb yet here, 1..X - bomb placements in numbered order
        public int Bomb { get; set; }  }
}