using System;
using GameBrain;

namespace GameConsoleUI
{
    public static class BattleshipsConsoleUI
    {
        public static void DrawBoard(CellState[,] board, int p)
        {
            if (p == 1)
            {
                Console.WriteLine("Player 1's board");
            }
            if (p == 2)
            {
                Console.WriteLine("Player 2's board");
            }
            var width = board.GetUpperBound(0) + 1;
            var height = board.GetUpperBound(1) + 1;
            for (int colIndex = 0; colIndex < width; colIndex++)
            {
                if (colIndex + 1 >= 10)
                {
                    Console.Write($"  {colIndex + 1}");
                }
                else
                {
                    Console.Write($"  {colIndex + 1} ");
                }
                    
            }
            Console.Write("\n");
            
            for (int colIndex = 0; colIndex < width; colIndex++)
            {
                Console.Write("+---");
                if (colIndex == width - 1)
                {
                    Console.Write("+");
                }
            }
            Console.WriteLine();

            for (int rowIndex = 0; rowIndex < height; rowIndex++)
            {
                for (int colIndex = 0; colIndex < width; colIndex++)
                {
                    Console.Write($"|{CellString(board[colIndex, rowIndex])}");
                }
                Console.Write($"| {rowIndex + 1}");
                
                Console.WriteLine();
                for (int colIndex = 0; colIndex < width; colIndex++)
                {
                    Console.Write("+---");
                    if (colIndex == width - 1)
                    {
                        Console.Write("+");
                    }
                }
                Console.WriteLine();
                
            }
        }

        public static void SwitchPlayer(Battleships game)
        {
            Console.Clear();
            Console.WriteLine($"Player {(game.NextMoveByP1 ? "1" : "2")} press any key:");
            Console.ReadKey();
        }

        public static string CellString(CellState cellState)
        {
            switch (cellState)
            {
                case CellState.Empty:
                    return "   ";
                case CellState.Hit:
                    return " X ";
                case CellState.Miss:
                    return " M ";
                case CellState.Ship:
                    return " O ";
            }

            return "-";
        }
    }
}