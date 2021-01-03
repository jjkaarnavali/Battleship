﻿using System;
using System.Text.Json;

namespace GameBrain
{
    public class Battleships
    {

        public static int s { get; set; } // size of the board
        

        private CellState[,] _P1board = new CellState[s,s];
        
        private CellState[,] _P2board = new CellState[s,s];

        

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



        

        public bool PlaceShipP1(bool horizontal, int shipSize, int x, int y, bool canShipsTouch)
        {
            var width = s;
            var height = s;

            
            
            if (horizontal)
            {
                for (int i = 0; i < shipSize; i++)
                {
                    if (_P1board[x + i, y] == CellState.Ship)
                    {
                        return false;
                    }
                }
                
                if (!canShipsTouch)
                {
                    for (int i = 0; i < shipSize + 2; i++)
                    {
                        if (x - 1 + i > width || y - 1 > height || x - 1 + i < 0 || y - 1 < 0)
                        {
                        }
                        else if (_P1board[x - 1 + i, y - 1] == CellState.Ship)
                        {
                            return false;
                        }
                    }
                    
                    for (int i = 0; i < shipSize + 2; i++)
                    {
                        if (x - 1 + i > width || x - 1 + i < 0)
                        {
                        }
                        else if (_P1board[x - 1 + i, y] == CellState.Ship)
                        {
                            return false;
                        }
                    }
                    
                    for (int i = 0; i < shipSize + 2; i++)
                    {
                        if (x - 1 + i > width || y + 1 > height || x - 1 + i < 0 || y + 1 < 0)
                        {
                        }
                        else if (_P1board[x - 1 + i, y + 1] == CellState.Ship)
                        {
                            return false;
                        }
                    }
                }
                
                for (int i = 0; i < shipSize; i++)
                {
                    if (_P1board[x + i, y] == CellState.Empty)
                    { 
                        _P1board[x + i, y] = CellState.Ship;
                    }
                }
             
                
                
            }
            if (!horizontal)
            {
              
                for (int i = 0; i < shipSize; i++)
                {
                    if (_P1board[x, y + i] == CellState.Ship)
                    {
                        return false;
                    }
                }
                
                if (!canShipsTouch)
                {
                    for (int i = 0; i < shipSize + 2; i++)
                    {
                        if (x - 1 > width || y - 1 + i > height || x - 1 < 0 || y - 1 + i < 0)
                        {
                        }
                        else if (_P1board[x - 1, y - 1 + i] == CellState.Ship)
                        {
                            return false;
                        }
                    }
                    
                    for (int i = 0; i < shipSize + 2; i++)
                    {
                        if (y - 1 + i > height || y - 1 + i < 0)
                        {
                        }
                        else if (_P1board[x, y - 1 + i] == CellState.Ship)
                        {
                            return false;
                        }
                    }
                    
                    for (int i = 0; i < shipSize + 2; i++)
                    {
                        if (x + 1 > width || y - 1 + i > height || x + 1 < 0 || y - 1 + i < 0)
                        {
                        }
                        else if (_P1board[x + 1, y - 1 + i] == CellState.Ship)
                        {
                            return false;
                        }
                    }
                }
                
                for (int i = 0; i < shipSize; i++)
                {
                    if (_P1board[x, y + i] == CellState.Empty) 
                    { 
                        _P1board[x, y + i] = CellState.Ship; 
                    }
                }
               
            }

            return true;
        }
        
        public bool PlaceShipP2(bool horizontal, int shipSize, int x, int y, bool canShipsTouch)
        {
            if (horizontal)
            {
                
                for (int i = 0; i < shipSize; i++)
                {
                    if (_P2board[x + i, y] == CellState.Ship)
                    {
                        return false;
                    }
                    
                }

                if (!canShipsTouch)
                {
                    for (int i = 0; i < shipSize + 2; i++)
                    {
                        if (_P2board[x - 1 + i, y - 1] == CellState.Ship)
                        {
                            return false;
                        }
                    }
                    
                    for (int i = 0; i < shipSize + 2; i++)
                    {
                        if (_P2board[x - 1 + i, y] == CellState.Ship)
                        {
                            return false;
                        }
                    }
                    
                    for (int i = 0; i < shipSize + 2; i++)
                    {
                        if (_P2board[x - 1 + i, y + 1] == CellState.Ship)
                        {
                            return false;
                        }
                    }
                }
              
                for (int i = 0; i < shipSize; i++)
                {
                    if (_P2board[x + i, y] == CellState.Empty)
                    {
                        _P2board[x + i, y] = CellState.Ship;
                    }
                }
               
                
            }
            if (!horizontal)
            {
              
                for (int i = 0; i < shipSize; i++)
                {
                    if (_P2board[x, y + i] == CellState.Ship)
                    {
                        return false;
                    }
                }
                
                if (!canShipsTouch)
                {
                    for (int i = 0; i < shipSize + 2; i++)
                    {
                        if (_P2board[x - 1, y - 1 + i] == CellState.Ship)
                        {
                            return false;
                        }
                    }
                    
                    for (int i = 0; i < shipSize + 2; i++)
                    {
                        if (_P2board[x, y - 1 + i] == CellState.Ship)
                        {
                            return false;
                        }
                    }
                    
                    for (int i = 0; i < shipSize + 2; i++)
                    {
                        if (_P2board[x + 1, y - 1 + i] == CellState.Ship)
                        {
                            return false;
                        }
                    }
                }
                
                for (int i = 0; i < shipSize; i++)
                {
                    if (_P2board[x, y + i] == CellState.Empty)
                    {
                        _P2board[x, y + i] = CellState.Ship;
                    }
                }
             
                
            }
            
            return true;
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

        public bool IsGameOver()
        {
            bool gameOver = true;
            bool p1lost = true;
            bool p2lost = true;
            foreach (CellState cell in _P1board)
            {
                if (cell == CellState.Ship)
                {
                    p1lost = false;
                }
            }
            foreach (CellState cell in _P2board)
            {
                if (cell == CellState.Ship)
                {
                    p2lost = false;
                }
            }

            if (!p1lost && !p2lost)
            {
                gameOver = false;
            }

            return gameOver;
        }

      
    }
    
}