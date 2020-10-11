namespace GameBrain
{
    public class GameState
    {
        public bool NextMoveByP1 { get; set; }
        public CellState[][] P1Board { get; set; } = null!;
        public CellState[][] P2Board { get; set; } = null!;
        
        public int Width { get; set; }
        public int Height { get; set; }
    }
}