using System;
using System.Text.Json;

namespace GameBrain
{
    public class Battleships
    {

        public static int s { get; set; }
        

        private CellState[,] _P1board = new CellState[15,15];
        
        private CellState[,] _P2board = new CellState[15,15];

        

        private bool _nextMoveByP1 = true;
        
        public bool NextMoveByP1 => _nextMoveByP1;
        public int bSize = 10;

        

        public CellState[,] GetP1Board(int dim)
        {
            
            var p1board = new CellState[dim,dim];
            bSize = dim;
            Array.Copy(_P1board, p1board, _P1board.Length);
            return p1board;
        }
        public CellState[,] GetP2Board(int dim)
        {
            var p2board = new CellState[dim,dim];
            bSize = dim;
            Array.Copy(_P2board, p2board, _P2board.Length);
            return p2board;
        }

       

        public bool TakeAShot(int x, int y, bool p)
        {
            if (!p)
            {
                if (_P1board[x, y] == CellState.Empty)
                {
                    _P1board[x, y] = CellState.Miss;
                    _nextMoveByP1 = !_nextMoveByP1;
                    return true;
                }
                if (_P1board[x, y] == CellState.Ship)
                {
                    _P1board[x, y] = CellState.Hit;
                    return true;
                }
            }

            if (p)
            {
                if (_P2board[x, y] == CellState.Empty)
                {
                    _P2board[x, y] = CellState.Miss;
                    _nextMoveByP1 = !_nextMoveByP1;
                    return true;
                }
                if (_P2board[x, y] == CellState.Ship)
                {
                    _P2board[x, y] = CellState.Hit;
                    return true;
                }
            }

            return false;
        }

        public string GetSerializedGameState()
        {
            var state = new GameState
            {
                NextMoveByP1 = _nextMoveByP1,
                Width = _P1board.GetLength(0),
                Height = _P1board.GetLength(1)
            };
            state.P1Board = new CellState[state.Width][];
            for (var i = 0; i < state.P1Board.Length; i++)
            {
                state.P1Board[i] = new CellState[state.Height];
            }
            state.P2Board = new CellState[state.Width][];
            for (var i = 0; i < state.P2Board.Length; i++)
            {
                state.P2Board[i] = new CellState[state.Height];
            }

            for (var x = 0; x < state.Width; x++)
            {
                for (var y = 0; y < state.Height; y++)
                {
                    state.P1Board[x][y] = _P1board[x, y];
                    state.P2Board[x][y] = _P2board[x, y];
                }
            }

            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(state, jsonOptions);
        }

        public void SetGameStateFromJsonString(string jsonString)
        {
            var state = JsonSerializer.Deserialize<GameState>(jsonString);
            _nextMoveByP1 = state.NextMoveByP1;
            _P1board = new CellState[state.Width, state.Height];
            _P2board = new CellState[state.Width, state.Height];
            for (var x = 0; x < state.Width; x++)
            {
                for (var y = 0; y < state.Height; y++)
                {
                    _P1board[x, y] = state.P1Board[x][y];
                    _P2board[x, y] = state.P2Board[x][y];
                }
            }
            
        }

      
    }
    
}