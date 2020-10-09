using System;

namespace GameBrain
{
    public class Battleships
    {
        private readonly CellState[,] _board = new CellState[10,10];

        private bool _nextMoveByP1 = true;

        public CellState[,] GetBoard()
        {
            var res = new CellState[10,10];
            Array.Copy(_board, res, _board.Length);
            return res;
        }

        public bool NextMoveByP1 => _nextMoveByP1;

        public bool MakeAShot(int x, int y)
        {
            if (_board[x, y] == CellState.Empty)
            {
                _board[x, y] = CellState.Miss;
                _nextMoveByP1 = !_nextMoveByP1;
                return true;
            }
            if (_board[x, y] == CellState.Ship)
            {
                _board[x, y] = CellState.Hit;
                return true;
            }

            return false;
        }
        
    }
    
    
}