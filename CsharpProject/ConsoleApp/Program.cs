using System;
using System.ComponentModel.Design;
using MenuSystem;


namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===========> TIC-TAC-TOE <===========");
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
            
            menu.AddMenuItem(new MenuItem("New game human vs human. Pointless.", "1", DefaulMenuAction));
            
            menu.AddMenuItem(new MenuItem("New game puny human vs mighty AI", "2", DefaulMenuAction));
            menu.AddMenuItem(new MenuItem("New game mighty AI vs superior AI", "3", DefaulMenuAction));
            
            menu.RunMenu();
        }

        static string DefaulMenuAction()
        {
            Console.WriteLine("Not implemented yet!");
            return "";
        }
    }
}