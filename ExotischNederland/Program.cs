using System;
using System.Collections.Generic;
using ExotischNederland.Models;
using System.Linq;
using ExotischNederland.DAL;
using System.IO;
using ExotischNederland.Menus;


namespace ExotischNederland
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                User debugUser = null;

                // Uncomment one of the following lines to debug as a specific user type:
        
                // Beheerder (Admin User)
                // |Id: 1194| Name: Admin User | Role: Beheerder | Email: admin@exotischnederland.com | PasswordHash: 08801e4de3979370bd0c8da452cf1e2d03be081e36986b5e9180036f33679266
                // debugUser = User.Find(1194); 
        
                // Vrijwilliger (Volunteer User)
                // |Id: 1195| Name: Volunteer User | Role: Vrijwilliger | Email: volunteer@exotischnederland.com | PasswordHash: 08801e4de3979370bd0c8da452cf1e2d03be081e36986b5e9180036f33679266
                // debugUser = User.Find(1195);

                // Wandelaar (Walker User)
                // |Id: 1196| Name: Walker User | Role: Wandelaar | Email: walker@exotischnederland.com | PasswordHash: 08801e4de3979370bd0c8da452cf1e2d03be081e36986b5e9180036f33679266
                // debugUser = User.Find(1196);

                // Familie (Family User)
                // |Id: 1197| Name: Family User | Role: Familie | Email: family@exotischnederland.com | PasswordHash: 08801e4de3979370bd0c8da452cf1e2d03be081e36986b5e9180036f33679266
                // debugUser = User.Find(1197);

                // Kinderen (Child User)
                // |Id: 1198| Name: Child User | Role: Kinderen | Email: child@exotischnederland.com | PasswordHash: 08801e4de3979370bd0c8da452cf1e2d03be081e36986b5e9180036f33679266
                // debugUser = User.Find(1198);
           
                User authenticatedUser = debugUser ?? MainMenu.Show();
        
                // Exit if no user is authenticated
                if (authenticatedUser is null) return;

                // Display the user menu
                UserMenu menu = new UserMenu(authenticatedUser);
                menu.Show();
            }
        }
    }
}
