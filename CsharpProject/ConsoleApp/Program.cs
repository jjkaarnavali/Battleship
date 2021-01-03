﻿using System;
 using System.Diagnostics;
 using System.Linq;
using System.Xml.Schema;
using GameBrain;
using GameConsoleUI;
using MenuSystem;
using DAL;
using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;


namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            /*var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(
                @"
                    Server=barrel.itcollege.ee,1533;
                    User Id=student;
                    Password=Student.Bad.password.0;
                    Database=jakaar_battleship_db;
                    MultipleActiveResultSets=true;
                    "
            ).Options;
            
            using var dbCtx = new ApplicationDbContext(dbOptions);
            Console.WriteLine("Deleting DB");
            dbCtx.Database.EnsureDeleted();
            Console.WriteLine("Migrating DB");
            dbCtx.Database.Migrate();
            Console.WriteLine("Adding data to DB");
            
            
            var playerA = new Player()
            {
                Name = "A"
            };
            
            var playerB = new Player()
            {
                Name = "B"
            };
            var game = new Game()
            {
                Description = "A vs B"
            };

            game.PlayerA = playerA;
            game.PlayerB = playerB;
            //playerA.Game = game;
            //playerB.Game = game;

            
            var gameOption = new GameOption()
            {
                Name = "Standard 10x10"
            };
            game.GameOption = gameOption;
            game.BoardState = "";
            
            dbCtx.Games.Add(game);
            dbCtx.SaveChanges();

            playerA.GameId = game.GameId;
            playerB.GameId = game.GameId;
            dbCtx.SaveChanges();*/
            
            Console.WriteLine("===========> BATTLESHIPS <===========");
            var menuD = new Menu(MenuLevel.Level2Plus);
            menuD.AddMenuItem(new MenuItem("Sub 5", "1", DefaulMenuAction));
            menuD.AddMenuItem(new MenuItem("Exit",
                userChoice: "e",
                DefaulMenuAction));
            
            var menuC = new Menu(MenuLevel.Level2Plus);
            menuC.AddMenuItem(new MenuItem("Sub 4", "1", menuD.RunMenu));
            menuC.AddMenuItem(new MenuItem("Exit",
                userChoice: "e",
                DefaulMenuAction));
            
            var menuB = new Menu(MenuLevel.Level2Plus);
            menuB.AddMenuItem(new MenuItem("Sub 3", "1", menuC.RunMenu));
            menuB.AddMenuItem(new MenuItem("Exit",
                userChoice: "e",
                DefaulMenuAction));
           
            
            var menuA = new Menu(MenuLevel.Level1);
            menuA.AddMenuItem(new MenuItem("Sub 2", "1", menuB.RunMenu));
            menuA.AddMenuItem(new MenuItem("testing", "2", DefaulMenuAction));
            menuA.AddMenuItem(new MenuItem("Exit",
                userChoice: "e",
                DefaulMenuAction));
            
            var menu = new Menu(MenuLevel.Level0);
            menu.AddMenuItem(new MenuItem("Go to submenu 1", "s", menuA.RunMenu));
            menu.AddMenuItem(new MenuItem("New game human vs human.", "1", Battleships));
            menu.AddMenuItem(new MenuItem("New game puny human vs mighty AI", "2", DefaulMenuAction));
            menu.AddMenuItem(new MenuItem("New game mighty AI vs superior AI", "3", DefaulMenuAction));
            menu.AddMenuItem(new MenuItem("Exit",
                userChoice: "e",
                DefaulMenuAction));
            
           
            
            menu.RunMenu();
        }

        static string DefaulMenuAction()
        {
            Console.WriteLine("Not implemented yet!");
            return "";
        }
        static int BoardSize(int userChoice)
        {
            return userChoice;
        }

        

        static string Battleships()
        {
            var battleshipGame = new Battleships();
            var bSize = 10;

            var loadGameMenu = new Menu(MenuLevel.Custom);
            loadGameMenu.AddMenuItem(new MenuItem("Load game",
                userChoice: "l", () => { return LoadGameAction(); })
            );
            loadGameMenu.AddMenuItem(new MenuItem("New Game",
                userChoice: "n", () => { return DefaulMenuAction(); })
            );
            var userChoiceFirst = loadGameMenu.RunMenu();
            

            if (userChoiceFirst.Length > 2)
            {
                dynamic d = JObject.Parse(userChoiceFirst);
            
                GameBrain.Battleships.s = d.Height;
            
                battleshipGame = new Battleships();
                battleshipGame.SetGameStateFromJsonString(userChoiceFirst);
            
                
            }
            else
            {
                Console.WriteLine(userChoiceFirst);
                bSize = GetBoardSize();
               
                int nrOfCarriers = GetCarriers();

                int nrOfBattleships = GetBattleships();
                
                int nrOfSubs = GetSubs();
                
                int nrOfCruisers = GetCruisers();
                
                int nrOfPatrols = GetPatrols();

                bool canShipsTouch = GetTouchRule();
                
                if (bSize == null)
                {
                    bSize = 10;
                }
                GameBrain.Battleships.s = bSize;
                battleshipGame = new Battleships();
                Console.WriteLine(GameBrain.Battleships.s);

                for (int i = 0; i < nrOfCarriers; i++)
                {
                    PlaceShipAction(battleshipGame, 1, 5, canShipsTouch);
                }
                for (int i = 0; i < nrOfBattleships; i++)
                {
                    PlaceShipAction(battleshipGame, 1, 4, canShipsTouch);
                }
                for (int i = 0; i < nrOfSubs; i++)
                {
                    PlaceShipAction(battleshipGame, 1, 3, canShipsTouch);
                }
                for (int i = 0; i < nrOfCruisers; i++)
                {
                    PlaceShipAction(battleshipGame, 1, 2, canShipsTouch);
                }
                for (int i = 0; i < nrOfPatrols; i++)
                {
                    PlaceShipAction(battleshipGame, 1, 1, canShipsTouch);
                }
              
                

                Console.Clear();
                Console.WriteLine($"Player 2 press any key:");
                Console.ReadKey();
                
                for (int i = 0; i < nrOfCarriers; i++)
                {
                    PlaceShipAction(battleshipGame, 2, 5, canShipsTouch);
                }
                for (int i = 0; i < nrOfBattleships; i++)
                {
                    PlaceShipAction(battleshipGame, 2, 4, canShipsTouch);
                }
                for (int i = 0; i < nrOfSubs; i++)
                {
                    PlaceShipAction(battleshipGame, 2, 3, canShipsTouch);
                }
                for (int i = 0; i < nrOfCruisers; i++)
                {
                    PlaceShipAction(battleshipGame, 2, 2, canShipsTouch);
                }
                for (int i = 0; i < nrOfPatrols; i++)
                {
                    PlaceShipAction(battleshipGame, 2, 1, canShipsTouch);
                }
                
            }
            
            
            var over = false;
            
            do
            {
                if (battleshipGame.NextMoveByP1)
                {
                    BattleshipsConsoleUI.DrawBoard(battleshipGame.GetP1Board(bSize), 1);
                    
                    CellState[,] copyOfP2Board = battleshipGame.GetP2Board(bSize);
                    var width = copyOfP2Board.GetUpperBound(0) + 1;
                    var height = copyOfP2Board.GetUpperBound(1) + 1;
                    
                    for (int rowIndex = 0; rowIndex < height; rowIndex++)
                    {
                        for (int colIndex = 0; colIndex < width; colIndex++)
                        {
                            if (copyOfP2Board[rowIndex, colIndex] == CellState.Ship)
                            {
                                copyOfP2Board[rowIndex, colIndex] = CellState.Empty;
                            }
                        }
                        
                    }
                    
                    BattleshipsConsoleUI.DrawBoard(copyOfP2Board, 2);
                    
                }
                else
                {
                    CellState[,] copyOfP1Board = battleshipGame.GetP1Board(bSize);
                    var width = copyOfP1Board.GetUpperBound(0) + 1;
                    var height = copyOfP1Board.GetUpperBound(1) + 1;
                    
                    for (int rowIndex = 0; rowIndex < height; rowIndex++)
                    {
                        for (int colIndex = 0; colIndex < width; colIndex++)
                        {
                            if (copyOfP1Board[rowIndex, colIndex] == CellState.Ship)
                            {
                                copyOfP1Board[rowIndex, colIndex] = CellState.Empty;
                            }
                        }
                        
                    }
                    
                    BattleshipsConsoleUI.DrawBoard(copyOfP1Board, 1);
                    
                    BattleshipsConsoleUI.DrawBoard(battleshipGame.GetP2Board(bSize), 2);
                }
               
                var menu = new Menu(MenuLevel.Custom);
                menu.AddMenuItem(new MenuItem("Save game",
                    userChoice: "s", () => { return SaveGameAction(battleshipGame); })
                );
                menu.AddMenuItem(new MenuItem("Save game to database",
                    userChoice: "d", () => { return SaveGameActionDB(battleshipGame); })
                );
                
                menu.AddMenuItem(new MenuItem("Load game",
                    userChoice: "l", () => { return LoadGameAction(); })
                );
                
                menu.AddMenuItem(new MenuItem($"Player {(battleshipGame.NextMoveByP1 ? "1" : "2")} make a move",
                    userChoice: "p",
                    DefaulMenuAction));
                menu.AddMenuItem(new MenuItem("Exit game",
                    userChoice: "e",
                    DefaulMenuAction));
                
                var userChoice = menu.RunMenu();

                if (userChoice == "p")
                {
                    GameAction(battleshipGame);
                    
                    if (battleshipGame.IsGameOver())
                    {
                        over = true;

                        if (battleshipGame.NextMoveByP1)
                        {
                            Console.WriteLine("Player 1 has won!");
                        }
                        else
                        {
                            Console.WriteLine("Player 2 has won!");
                        }
                    }
                    else
                    {
                        BattleshipsConsoleUI.SwitchPlayer(battleshipGame);
                    }
                    
                    
                }
                if (userChoice == "e" || userChoice == "x")
                {
                    over = true;
                }

                
                
            } while (over == false);
            
            return "x";
        }

        public static (int x, int y) GetShotCoordinates(Battleships game)
        {
            
           
            var coords = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(coords) || !coords.Contains(","))
            {

                if (string.IsNullOrWhiteSpace(coords))
                {

                    Console.WriteLine("You have to enter coordinates for your shot! Try again: ");
                    coords = Console.ReadLine();


                }
                else if (!coords.Contains(","))
                {
                    Console.WriteLine("Coordinates have to be separated by a coma! Try again: ");
                    coords = Console.ReadLine();

                }
            }

            var tmp = coords!.Split(",");
            var intX = 0;
            var intY = 0;
            bool xIsNr = int.TryParse(tmp[0].Trim(), out intX);
            bool yIsNr = int.TryParse(tmp[1].Trim(), out intY);
            if (!xIsNr || !yIsNr)
            {
                Console.WriteLine("Coordinates have to be numbers! Try again: ");
                return GetShotCoordinates(game);
            }

            
            
            if (int.Parse(tmp[0].Trim()) > game.GetP1Board(game.bSize).GetLength(0) ||
                   int.Parse(tmp[1].Trim()) > game.GetP1Board(game.bSize).GetLength(1))
            {
                Console.WriteLine($"Given coordinates are out of range! Max coords are" +
                                  $" {game.GetP1Board(game.bSize).GetLength(0)},{game.GetP1Board(game.bSize).GetLength(0)}" +
                                  $" Try again: ");
                return GetShotCoordinates(game);
            }
            
            
            var userValue = coords!.Split(",");
            

            
            

            var x = int.Parse(userValue[0].Trim()) - 1;
            var y = int.Parse(userValue[1].Trim()) - 1;
            
                    
            
            return (x, y);
        }
        
        static string LoadGameAction()
        {
            var files = System.IO.Directory.EnumerateFiles(".", "*.json").ToList();
            for (int i = 0; i < files.Count; i++)
            {
                Console.WriteLine($"{i} - {files[i]}");
            }

            var fileNo = Console.ReadLine();
            var fileName = files[int.Parse(fileNo!.Trim())];
            var jsonString = System.IO.File.ReadAllText(fileName);
            
           
            
            
            return jsonString;
        }
        
        static string LoadGameActionDB(Battleships game)
        {
            /*var files = System.IO.Directory.EnumerateFiles(".", "*.json").ToList();
            for (int i = 0; i < files.Count; i++)
            {
                Console.WriteLine($"{i} - {files[i]}");
            }

            var fileNo = Console.ReadLine();
            var fileName = files[int.Parse(fileNo!.Trim())];
            var jsonString = System.IO.File.ReadAllText(fileName);*/
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(
                @"
                    Server=barrel.itcollege.ee,1533;
                    User Id=student;
                    Password=Student.Bad.password.0;
                    Database=jakaar_battleship_db;
                    MultipleActiveResultSets=true;
                    "
            ).Options;
            
            using var dbCtx = new ApplicationDbContext(dbOptions);

            var saves = dbCtx.Games.ToList();
            for (int i = 0; i < saves.Count(); i++)
            {
                Console.WriteLine($"{saves[i].GameId} - {saves[i].Description}");
            }

            var saveId = Console.ReadLine();
            foreach (var gamee in saves)
            {
                if (gamee.GameId.ToString() == saveId)
                {
                    var jsonString = gamee.BoardState;
                    game.SetGameStateFromJsonString(jsonString);
                }
            }
            
            
            BattleshipsConsoleUI.DrawBoard(game.GetP1Board(game.bSize), 1); 
            BattleshipsConsoleUI.DrawBoard(game.GetP2Board(game.bSize), 2);
            
            return "";
        }

        static string SaveGameAction(Battleships game)
        {
            var defaultName = "save_" + DateTime.Now.ToString("yyyy-MM-dd") + ".json";
            Console.Write($"File name ({defaultName}): ");
            var fileName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = defaultName;
            }
            else
            {
                fileName = fileName + ".json";
            }

            var serializedGame = game.GetSerializedGameState();
            
            System.IO.File.WriteAllText(fileName, serializedGame);
            return "";
        }
        
        static string SaveGameActionDB(Battleships game)
        {
            var defaultName = "save_" + DateTime.Now.ToString("yyyy-MM-dd");
            Console.Write($"File name ({defaultName}): ");
            var saveName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(saveName))
            {
                saveName = defaultName;
            }
            
            var serializedGame = game.GetSerializedGameState();
            
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(
                @"
                    Server=barrel.itcollege.ee,1533;
                    User Id=student;
                    Password=Student.Bad.password.0;
                    Database=jakaar_battleship_db;
                    MultipleActiveResultSets=true;
                    "
            ).Options;
            
            using var dbCtx = new ApplicationDbContext(dbOptions);
            
            
            var playerA = new Player()
            {
                Name = "A"
            };
            
            var playerB = new Player()
            {
                Name = "B"
            };
            var gameSave = new Game()
            {
                Description = saveName
            };

            gameSave.PlayerA = playerA;
            gameSave.PlayerB = playerB;
            //playerA.Game = game;
            //playerB.Game = game;

            var moveBy = "";
            if (game.NextMoveByP1)
            {
                moveBy = "SamePlayer";
            }
            else
            {
                moveBy = "OtherPlayer";
            }

            if (moveBy.Equals("SamePlayer"))
            {
                var gameOption = new GameOption()
                {
                    Name = "Standard 10x10",
                    BoardWidth = 10,
                    BoardHeight = 10,
                    ENextMoveAfterHit = ENextMoveAfterHit.SamePlayer
                };
                gameSave.GameOption = gameOption;
            }
            else
            {
                var gameOption = new GameOption()
                {
                    Name = "Standard 10x10",
                    BoardWidth = 10,
                    BoardHeight = 10,
                    ENextMoveAfterHit = ENextMoveAfterHit.OtherPlayer
                };
                gameSave.GameOption = gameOption;
            }

            /*var playerABoardState = new PlayerBoardState()
            {
                Player = playerA,
                BoardState = game.GetSerializedGameState()
            };
            
            var playerBBoardState = new PlayerBoardState()
            {
                Player = playerB,
                CreatedAt = DateTime.Now,
                BoardState = game.GetSerializedGameState()
            };*/



            gameSave.BoardState = game.GetSerializedGameState();
            dbCtx.Games.Add(gameSave);
            dbCtx.SaveChanges();

            playerA.GameId = gameSave.GameId;
            playerB.GameId = gameSave.GameId;
            dbCtx.SaveChanges();

            //playerABoardState.PlayerId = gameSave.PlayerAId;
            //playerBBoardState.PlayerId = gameSave.PlayerBId;
            dbCtx.SaveChanges();
            
           
            return "";
        }

        static void GameAction(Battleships game)
        {
            
            Console.WriteLine("Upper left corner is (1,1)!");
            Console.Write($"Give X (1-{game.GetP1Board(game.bSize).GetLength(0)})" +
                          $" and Y (1-{game.GetP1Board(game.bSize).GetLength(1)})" +
                          $" coordinates of the shot like this (X,Y): ");
            var (x, y) = GetShotCoordinates(game);
            
            game.TakeAShot(x, y, game.NextMoveByP1);
            BattleshipsConsoleUI.DrawBoard(game.GetP1Board(game.bSize), 1); 
            BattleshipsConsoleUI.DrawBoard(game.GetP2Board(game.bSize), 2);
            
        }

        static int GetBoardSize()
        {
            Console.WriteLine("The game board is a square, give the length of the board size! (default is 10)");
            var bSize = Console.ReadLine();
            if (bSize.Length == 0)
            {
                return 10;
            }
            return int.Parse(bSize);
        }
        
        static bool GetTouchRule()
        {
            Console.WriteLine("Do you want the ships to be able to touch each other? (y/n)");
            var yesOrNo = Console.ReadLine();
            if (yesOrNo.Length != 1)
            {
                Console.WriteLine("You must answer either 'y' for yes or 'n' for no!");
                GetTouchRule();
            }
            else
            {
                if (yesOrNo.Equals("y"))
                {
                    return true;
                }
                else if (yesOrNo.Equals("n"))
                {
                    return false;
                }else
                {
                    Console.WriteLine("You must answer either 'y' for yes or 'n' for no!");
                    GetTouchRule();
                }
                
            }
            return false;
        }
        
        static int GetCarriers()
        {
            Console.WriteLine("How many carriers (1x5) do you want? (default is 1)");
            var nrOfShips = Console.ReadLine();
            if (nrOfShips.Length == 0)
            {
                return 1;
            }
            return int.Parse(nrOfShips);
        }
        
        static int GetBattleships()
        {
            Console.WriteLine("How many battleships (1x4) do you want? (default is 1)");
            var nrOfShips = Console.ReadLine();
            if (nrOfShips.Length == 0)
            {
                return 1;
            }
            return int.Parse(nrOfShips);
        }
        
        static int GetSubs()
        {
            Console.WriteLine("How many submarines (1x3) do you want? (default is 1)");
            var nrOfShips = Console.ReadLine();
            if (nrOfShips.Length == 0)
            {
                return 1;
            }
            return int.Parse(nrOfShips);
        }
        
        static int GetCruisers()
        {
            Console.WriteLine("How many cruisers (1x2) do you want? (default is 1)");
            var nrOfShips = Console.ReadLine();
            if (nrOfShips.Length == 0)
            {
                return 1;
            }
            return int.Parse(nrOfShips);
        }
        
        static int GetPatrols()
        {
            Console.WriteLine("How many patrols (1x1) do you want? (default is 1)");
            var nrOfShips = Console.ReadLine();
            if (nrOfShips.Length == 0)
            {
                return 1;
            }
            return int.Parse(nrOfShips);
        }
        
        static void PlaceShipAction(Battleships game, int player, int ship, bool canShipsTouch)
        {
            string shipName = "";
            if (ship == 5)
            {
                shipName = "Carrier";
            }else if(ship == 4)
            {
                shipName = "Battleship";
            }else if(ship == 3)
            {
                shipName = "Submarine";
            }else if(ship == 2)
            {
                shipName = "Cruiser";
            }else if(ship == 1)
            {
                shipName = "Patrol";
            }

            if (player == 1)
            {
                BattleshipsConsoleUI.DrawBoard(game.GetP1Board(game.bSize), 1); 
                
                
                Console.WriteLine($"Player 1 write 0 if you want your {shipName} (1x{ship}) to be placed horizontally and 1 if vertically");
                var check = Console.ReadLine();
                
                var intX = 0;
                
                bool xIsNr = int.TryParse(check, out intX);

                if (!xIsNr)
                {
                    Console.WriteLine("Input has to be 0 or 1!");
                    PlaceShipAction(game, player, ship, canShipsTouch);
                }
                else
                {
                    if (int.Parse(check) != 0 && int.Parse(check) != 1)
                    {
                        Console.WriteLine("Input has to be 0 or 1!");
                        PlaceShipAction(game, player, ship, canShipsTouch);
                    }
                    else
                    {
                        var horizontal = int.Parse(check);
                        bool horizontalBool = true;
                        if (horizontal == 0)
                        {
                            horizontalBool = true;
                        }else if (horizontal == 1)
                        {
                            horizontalBool = false;
                        }
                        Console.Write($"Give X (1-{game.GetP1Board(game.bSize).GetLength(0)})" +
                                      $" and Y (1-{game.GetP1Board(game.bSize).GetLength(1)})" +
                                      $" coordinates of the ship's starting point like this (X,Y): ");
                        var (x, y) = GetShotCoordinates(game);
                        if (horizontalBool && x + ship > game.bSize)
                        {
                            Console.WriteLine("Ship is out of bounds! Try again.");
                            PlaceShipAction(game, player, ship, canShipsTouch);
                        }else if (!horizontalBool && y + ship > game.bSize)
                        {
                            Console.WriteLine("Ship is out of bounds! Try again.");
                            PlaceShipAction(game, player, ship, canShipsTouch);
                        }
                        else
                        {
                            bool canPlace = game.PlaceShipP1(horizontalBool, ship, x, y, canShipsTouch);

                            if (!canPlace)
                            {
                                Console.WriteLine("Ships can't overlap!");
                                PlaceShipAction(game, player, ship, canShipsTouch);
                            }
                        }
                    }
                    
                }
            }

            if (player == 2)
            {
                BattleshipsConsoleUI.DrawBoard(game.GetP2Board(game.bSize), 2); 
            
                Console.WriteLine($"Player 2 write 0 if you want your {shipName} (1x{ship}) to be placed horizontally and 1 if vertically");
                var check = Console.ReadLine();
                
                var intX = 0;
                
                bool xIsNr = int.TryParse(check, out intX);

                if (!xIsNr)
                {
                    Console.WriteLine("Input has to be 0 or 1!");
                    PlaceShipAction(game, player,ship, canShipsTouch);
                }
                else
                {
                    if (int.Parse(check) != 0 && int.Parse(check) != 1)
                    {
                        Console.WriteLine("Input has to be 0 or 1!");
                        PlaceShipAction(game, player, ship, canShipsTouch);
                    }
                    else
                    {
                        var horizontal = int.Parse(check);
                        bool horizontalBool = true;
                        if (horizontal == 0)
                        {
                            horizontalBool = true;
                        }else if (horizontal == 1)
                        {
                            horizontalBool = false;
                        }
                        Console.Write($"Give X (1-{game.GetP2Board(game.bSize).GetLength(0)})" +
                                      $" and Y (1-{game.GetP2Board(game.bSize).GetLength(1)})" +
                                      $" coordinates of the ship's starting point like this (X,Y): ");
                        var (x, y) = GetShotCoordinates(game);
                
                        if (horizontalBool && x + ship > game.bSize)
                        {
                            Console.WriteLine("Ship is out of bounds! Try again.");
                            PlaceShipAction(game, player, ship, canShipsTouch);
                        }else if (!horizontalBool && y + ship > game.bSize)
                        {
                            Console.WriteLine("Ship is out of bounds! Try again.");
                            PlaceShipAction(game, player, ship, canShipsTouch);
                        }
                        else
                        {
                            bool canPlace = game.PlaceShipP2(horizontalBool, ship, x, y, canShipsTouch);

                            if (!canPlace)
                            {
                                Console.WriteLine("Ships can't overlap!");
                                PlaceShipAction(game, player, ship, canShipsTouch);
                            }
                            
                        }
                    }
                }
                
            }
            
        }

    }
}
