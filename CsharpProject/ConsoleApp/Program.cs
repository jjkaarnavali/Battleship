using System;
using System.Linq;
using GameBrain;
using GameConsoleUI;
using MenuSystem;


namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===========> BATTLESHIPS <===========");
            var menuD = new Menu(MenuLevel.Level2Plus);
            menuD.AddMenuItem(new MenuItem("Sub 5", "1", DefaulMenuAction));
            
            var menuC = new Menu(MenuLevel.Level2Plus);
            menuC.AddMenuItem(new MenuItem("Sub 4", "1", menuD.RunMenu));
            
            var menuB = new Menu(MenuLevel.Level2Plus);
            menuB.AddMenuItem(new MenuItem("Sub 3", "1", menuC.RunMenu));
           
            
            var menuA = new Menu(MenuLevel.Level1);
            menuA.AddMenuItem(new MenuItem("Sub 2", "1", menuB.RunMenu));
            menuA.AddMenuItem(new MenuItem("testing", "2", DefaulMenuAction));
            
            var menu = new Menu(MenuLevel.Level0);
            menu.AddMenuItem(new MenuItem("Go to submenu 1", "s", menuA.RunMenu));
            menu.AddMenuItem(new MenuItem("New game human vs human. Pointless.", "1", Battleships));
            menu.AddMenuItem(new MenuItem("New game puny human vs mighty AI", "2", DefaulMenuAction));
            menu.AddMenuItem(new MenuItem("New game mighty AI vs superior AI", "3", DefaulMenuAction));
            
            menu.RunMenu();
        }

        static string DefaulMenuAction()
        {
            Console.WriteLine("Not implemented yet!");
            return "";
        }

        static string Battleships()
        {
            var game = new Battleships();
            var over = false;
            
            do
            {

                BattleshipsConsoleUI.DrawBoard(game.GetP1Board(), 1);
                BattleshipsConsoleUI.DrawBoard(game.GetP2Board(), 2);

                var menu = new Menu(MenuLevel.Level0);
                menu.AddMenuItem(new MenuItem("Save game",
                    userChoice: "s", () => { return SaveGameAction(game); })
                );
                menu.AddMenuItem(new MenuItem("Load game",
                    userChoice: "l", () => { return LoadGameAction(game); })
                );

                menu.AddMenuItem(new MenuItem("Exit game",
                    userChoice: "e",
                    DefaulMenuAction));
                
                menu.AddMenuItem(new MenuItem($"Player {(game.NextMoveByP1 ? "1" : "2")} make a move",
                    userChoice: "p",
                    DefaulMenuAction));

                
               

                var userChoice = menu.RunMenu();

                if (userChoice == "p")
                {
                    GameAction(game);
                    Console.Clear();
                    BattleshipsConsoleUI.SwitchPlayer(game);
                    Console.ReadKey();
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
            Console.WriteLine("Upper left corner is (1,1)!");
            Console.Write($"Give X (1-{game.GetP1Board().GetLength(0)})" +
                          $" and Y (1-{game.GetP1Board().GetLength(1)})" +
                          $" coordinates of the shot like this (X,Y): ");
            var coords = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(coords))
            {
                coords = "";
                Console.WriteLine("You have to enter coordinates for your shot! Try again: ");
                coords = Console.ReadLine();

            }
            while (!coords.Contains(","))
            {
                coords = "";
                Console.WriteLine("Coordinates have to be separated by a coma! Try again: ");
                coords = Console.ReadLine();

            }

            var userValue = coords!.Split(",");
            while (int.Parse(userValue[0].Trim()) > game.GetP1Board().GetLength(0) ||
                   int.Parse(userValue[1].Trim()) > game.GetP1Board().GetLength(1))
            {
                coords = "";
                Console.WriteLine($"Given coordinates are out of range! Max coords are" +
                                  $" {game.GetP1Board().GetLength(0)},{game.GetP1Board().GetLength(0)}" +
                                  $" Try again: ");
                coords = Console.ReadLine();
            }
            

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
            
            BattleshipsConsoleUI.DrawBoard(game.GetP1Board(), 1); 
            BattleshipsConsoleUI.DrawBoard(game.GetP2Board(), 2);
            
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

        static void GameAction(Battleships game)
        {
            var (x, y) = GetShotCoordinates(game);
            game.TakeAShot(x, y, game.NextMoveByP1);
            BattleshipsConsoleUI.DrawBoard(game.GetP1Board(), 1); 
            BattleshipsConsoleUI.DrawBoard(game.GetP2Board(), 2);
            
        }

    }
}
