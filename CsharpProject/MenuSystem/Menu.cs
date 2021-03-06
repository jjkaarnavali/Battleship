﻿﻿using System;
using System.Collections.Generic;
using System.Linq;


 namespace MenuSystem
{
    public enum MenuLevel
    {
        Level0,
        Level1,
        Level2Plus,
        Custom
    }

    public class Menu
    {
        private Dictionary<string, MenuItem> MenuItems { get; set; } = new Dictionary<string, MenuItem>();
        
        private readonly MenuLevel _menuLevel;
        
        private readonly string[] reservedActions = new[] {"m", "r"};
        
        public Menu(MenuLevel level)
        {
            _menuLevel = level;
        }

        public void AddMenuItem(MenuItem item)
        {
            if (item.UserChoice == "")
            {
                throw new ArgumentException("Userchoice can't be empty or spaces.");
            }
            
            
            MenuItems.Add(item.UserChoice, item);
        }
        

        public string RunMenu()
        {
            var userChoice = "";
            
            do
            {
                Console.Write("");

                foreach (var menuItem in MenuItems)
                {
                    Console.WriteLine(menuItem.Value);
                }

                switch (_menuLevel)
                {
                    case MenuLevel.Level0: 
                        
                        break;
                    case MenuLevel.Level1: 
                        Console.WriteLine("M) Return to Main");
                        
                        break;
                    case MenuLevel.Level2Plus: 
                        Console.WriteLine("R) Return to previous");
                        Console.WriteLine("M) Return to Main");
                        
                        break;
                    case MenuLevel.Custom: 
                        
                        break;
                    default:
                        throw new Exception("Unknown menu depth!");
                }
                Console.Write(">");
                
                userChoice = Console.ReadLine()?.ToLower().Trim() ?? "";

                
               
                // Is it a reserved keyword
                if (!reservedActions.Contains(userChoice))
                {
                    if (userChoice == "p" || userChoice == "n" || userChoice.Length > 10)
                    {
                        return userChoice;
                    }
                    if (userChoice == "e")
                    {
                        if (_menuLevel == MenuLevel.Custom)
                        {
                            break;
                        }
                        Console.WriteLine("Closing down......");
                        userChoice = "e";
                        break;
                    }
                    
                    
                    // No it wasn't, try to find keyword in MenuItems
                    if (MenuItems.TryGetValue(userChoice, out var userMenuItem))
                    {
                        userChoice = userMenuItem.MethodToExecute();
                    }else
                    {
                        Console.WriteLine("I don't have this option!");
                    }
                    
                }

                if (_menuLevel == MenuLevel.Custom)
                {
                    return userChoice;
                }

               

                if (_menuLevel == MenuLevel.Level0 && userChoice == "r")
                {
                    Console.WriteLine("I don't have this option!");
                    userChoice = "";
                }
                if (_menuLevel == MenuLevel.Level0 && userChoice == "m")
                {
                    Console.WriteLine("I don't have this option!");
                    userChoice = "";
                }

                if (_menuLevel == MenuLevel.Level1 && userChoice == "r")
                {
                    Console.WriteLine("I don't have this option!");
                    userChoice = "";
                }
                
                
                
                if (_menuLevel != MenuLevel.Level0 && userChoice == "m")
                {
                    if (_menuLevel == MenuLevel.Level1)
                    {
                        userChoice = "";
                        break;
                    }

                    if (_menuLevel == MenuLevel.Level2Plus)
                    {
                        userChoice = "m";
                        break;
                    }
                }
                else

                if (_menuLevel == MenuLevel.Level2Plus && userChoice == "r")
                {
                    userChoice = "";
                    break;
                }

            } while (userChoice != "e" && userChoice !="m" && userChoice !="r");

            return userChoice;
        }
    }
}
