using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExotischNederland.Models;

namespace ExotischNederland
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> menuItems = new Dictionary<string, string>
            {
                { "1", "Login" },
                { "2", "Register" },
                { "3", "Exit" }
            };

            while (true)
            {
                string selected = Helpers.MenuSelect(menuItems, true);
                if (selected == "1")
                {
                    Console.Clear();
                    Console.WriteLine("Enter your email:");
                    string email = Console.ReadLine();
                    Console.WriteLine("Enter your password:");
                    string password = Console.ReadLine();
                    if (User.Authenticate(email, password))
                    {
                        Console.WriteLine("Login successful!");
                    }
                    else
                    {
                        Console.WriteLine("Login failed!");
                    }
                    Console.WriteLine("Press a key to return to the menu");
                    Console.ReadKey();
                }
                else if (selected == "2")
                {
                    Console.Clear();
                    Console.WriteLine("Enter your name:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Enter your email:");
                    string email = Console.ReadLine();
                    Console.WriteLine("Enter your password:");
                    string password = Console.ReadLine();
                    User.Create(name, email, password);
                    Console.WriteLine("User created!");
                    Console.WriteLine("Press a key to return to the menu");
                    Console.ReadKey();
                }
                else if (selected == "3")
                {
                    break;
                }
            }
        }
    }
}
