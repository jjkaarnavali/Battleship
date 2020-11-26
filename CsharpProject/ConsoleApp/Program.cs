using System;
using System.Linq;
using System.Xml.Schema;
using GameBrain;
using GameConsoleUI;
using MenuSystem;
using DAL;
using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;



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

            
            
            var over = false;
            var bSize = GameSettings();
            if (bSize == null)
            {
                bSize = 10;
            }
            var battleshipGame = new Battleships();
            GameBrain.Battleships.s = bSize;
            
            do
            {
                
                BattleshipsConsoleUI.DrawBoard(battleshipGame.GetP1Board(bSize), 1);
                BattleshipsConsoleUI.DrawBoard(battleshipGame.GetP2Board(bSize), 2);

                var menu = new Menu(MenuLevel.Custom);
                menu.AddMenuItem(new MenuItem("Save game",
                    userChoice: "s", () => { return SaveGameAction(battleshipGame); })
                );
                menu.AddMenuItem(new MenuItem("Save game to database",
                    userChoice: "d", () => { return SaveGameActionDB(battleshipGame); })
                );
                menu.AddMenuItem(new MenuItem("Load game",
                    userChoice: "l", () => { return LoadGameAction(battleshipGame); })
                );
                menu.AddMenuItem(new MenuItem("Load game from DB",
                    userChoice: "q", () => { return LoadGameActionDB(battleshipGame); })
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
                    BattleshipsConsoleUI.SwitchPlayer(battleshipGame);
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
        
        static string LoadGameAction(Battleships game)
        {
            var files = System.IO.Directory.EnumerateFiles(".", "*.json").ToList();
            for (int i = 0; i < files.Count; i++)
            {
                Console.WriteLine($"{i} - {files[i]}");
            }

            var fileNo = Console.ReadLine();
            var fileName = files[int.Parse(fileNo!.Trim())];
            var jsonString = System.IO.File.ReadAllText(fileName);
            game.SetGameStateFromJsonString(jsonString);
            
            BattleshipsConsoleUI.DrawBoard(game.GetP1Board(game.bSize), 1); 
            BattleshipsConsoleUI.DrawBoard(game.GetP2Board(game.bSize), 2);
            
            return "";
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

        static int GameSettings()
        {
            Console.WriteLine("The game board is a square, give the length of the board size!");
            var bSize = Console.ReadLine();
            if (bSize.Length == 0)
            {
                return 10;
            }
            return int.Parse(bSize);
        }

    }
}
