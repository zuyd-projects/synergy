using ExotischNederland.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.Menus
{
    internal class MainMenu
    {
        

        public static User Show()
        {
            Dictionary<string, string> items = new Dictionary<string, string>
            {
                { "login", "Login" },
                { "register", "Register" },
                { "exit", "Exit" }
            };
            string selected = Helpers.MenuSelect(items, true);

            if (selected == "login")
            {
                Console.Clear();
                Console.WriteLine("Enter your email:");
                string email = Console.ReadLine();
                Console.WriteLine("Enter your password:");
                string password = Helpers.ReadPassword();

                User authenticatedUser = User.Authenticate(email, password);

                if (authenticatedUser != null)
                {
                    Console.Clear();
                    Console.WriteLine($"Welkom {authenticatedUser.Name}");
                    return authenticatedUser;
                }
                else
                {
                    Console.WriteLine("Login failed!");
                }

                Console.WriteLine("Press a key to return to the menu");
                Console.ReadKey();
                Show();
            }
            else if (selected == "register")
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
                Show();
            }
            return null;
        }
    }
}
