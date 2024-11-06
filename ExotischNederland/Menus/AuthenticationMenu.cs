using ExotischNederland.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.Menus
{
    internal class AuthenticationMenu
    {
        

        public static User Show()
        {
            Dictionary<string, string> items = new Dictionary<string, string>
            {
                { "login", "Login" },
                { "register", "Registreer" },
                { "exit", "Verlaat" }
            };
            string selected = Helpers.MenuSelect(items, true);

            if (selected == "login")
            {
                List<FormField> fields = new List<FormField>
                {
                    new FormField("email", "E-mail", "string", true),
                    new FormField("password", "Wachtwoord", "password", true)
                };
                Dictionary<string, object> values = new Form(fields).Prompt();

                User authenticatedUser = User.Authenticate((string)values["email"], (string)values["password"]);

                if (authenticatedUser != null)
                {
                    Console.Clear();
                    Console.WriteLine($"Welkom {authenticatedUser.Name}");
                    return authenticatedUser;
                }
                else
                {
                    Console.WriteLine("Login mislukt!");
                }

                Console.WriteLine("Druk op een toets om terug te keren naar het menu");
                Console.ReadKey();
                Show();
            }
            else if (selected == "registreer")
            {
                Console.Clear();
                Console.WriteLine("Voer uw naam in:");
                string name = Console.ReadLine();
                Console.WriteLine("Voer uw e-mailadres in:");
                string email = Console.ReadLine();
                Console.WriteLine("Voer uw wachtwoord in:");
                string password = Console.ReadLine();
                User.Create(name, email, password);
                Console.WriteLine("Gebruiker gemaakt!");
                Console.WriteLine("Druk op een toets om terug te keren naar het menu");
                Console.ReadKey();
                Show();
            }
            return null;
        }
    }
}
