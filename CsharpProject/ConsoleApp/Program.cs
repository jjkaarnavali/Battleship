using System;
using System.ComponentModel.Design;
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
            BattleshipsConsoleUI.DrawBoard(game.GetP1Board(), 1); 
            BattleshipsConsoleUI.DrawBoard(game.GetP2Board(), 2);
            
            var menu = new Menu(MenuLevel.Level0);
            menu.AddMenuItem(new MenuItem($"Player {(game.NextMoveByP1? "1" : "2")} make a move",
                userChoice: "p",
                () =>
                {
                    var (x, y) = GetShotCoordinates(game);
                    game.TakeAShot(x, y, game.NextMoveByP1);
                    BattleshipsConsoleUI.DrawBoard(game.GetP1Board(), 1); 
                    BattleshipsConsoleUI.DrawBoard(game.GetP2Board(), 2);
                    return "";
                })
            );
            /*var (x, y) = GetShotCoordinates(game);
                    game.TakeAShot(x, y, game.NextMoveByP1);
                    BattleshipsConsoleUI.DrawBoard(game.GetP1Board(), 1); 
                    BattleshipsConsoleUI.DrawBoard(game.GetP2Board(), 2);
                    return "";
                    */

            menu.AddMenuItem(new MenuItem("Save game",
                userChoice: "s", () => { return SaveGameAction(game); })
            );
            menu.AddMenuItem(new MenuItem("Load game",
                userChoice: "l", () => { return LoadGameAction(game); })
            );
            
            menu.AddMenuItem(new MenuItem("Exit game",
                userChoice: "e",
                DefaulMenuAction));

            var userChoice = menu.RunMenu();
            
            
            return userChoice;
        }
        static (int x, int y) GetShotCoordinates(Battleships game)
        {
            Console.WriteLine("Upper left corner is (1,1)!");
            Console.Write("Give X (1-10) and Y (1-10) coordinates of the shot like this (X,Y): ");
            var userValue = Console.ReadLine().Split(",");
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

            var serializedGame = game.GetSerializedGameState();
            //Console.WriteLine(serializedGame);
            System.IO.File.WriteAllText(fileName, serializedGame);
            return "";
        }

    }
}
