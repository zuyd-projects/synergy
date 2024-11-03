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
                debugUser = User.Find(1198);

                // If no debug user is specified, go to the main menu for authentication
                User authenticatedUser = debugUser ?? MainMenu.Show();
        
                // Exit if no user is authenticated
                if (authenticatedUser is null) return;

                // Display the user menu
                UserMenu menu = new UserMenu(authenticatedUser);
                menu.Show();
            }
        }
        
        //    static void RouteMenu(User user)
        //    {
        //        Dictionary<string, string> routeMenuItems = new Dictionary<string, string>
        //    {
        //        { "1", "Create Route" },
        //        { "2", "View Routes" },
        //        { "3", "Update Route" },
        //        { "4", "Delete Route" },
        //        { "5", "Logout" }
        //    };

        //        while (true)
        //        {
        //            string selected = Helpers.MenuSelect(routeMenuItems, true);
        //            if (selected == "1")
        //            {
        //                CreateRoute();
        //            }
        //            else if (selected == "2")
        //            {
        //                ViewRoutes();
        //            }
        //            else if (selected == "3")
        //            {
        //                UpdateRoute();
        //            }
        //            else if (selected == "4")
        //            {
        //                DeleteRoute();
        //            }
        //            else if (selected == "5")
        //            {
        //                Console.Clear();
        //                break;
        //            }
        //        }
        //    }

        //    static void CreateRoute()
        //    {
        //        Console.Clear();
        //        Console.WriteLine("Creating a new route...");

        //        Console.WriteLine("Enter Route name:");
        //        string name = Console.ReadLine();
        //        Console.WriteLine("Enter Route description:");
        //        string description = Console.ReadLine();
        //        Console.WriteLine("Enter User ID:");
        //        int userId = int.Parse(Console.ReadLine());
        //        Console.WriteLine("Enter Area ID:");
        //        int areaId = int.Parse(Console.ReadLine());

        //        var routePoints = new List<RoutePoint>();
        //        Console.WriteLine("Enter Route Points (type 'done' to finish):");
        //        while (true)
        //        {
        //            Console.Write("Enter latitude: ");
        //            string latInput = Console.ReadLine();
        //            if (latInput.ToLower() == "done") break;
        //            Console.Write("Enter longitude: ");
        //            string lngInput = Console.ReadLine();
        //            //routepoint class moet nog aangemaakt worden
        //            routePoints.Add(new RoutePoint { Latitude = double.Parse(latInput), Longitude = double.Parse(lngInput) });
        //        }

        //        Route route = Route.Create(name, description, userId, areaId);
        //        foreach (var point in routePoints)
        //        {
        //            route.AddRoutePoint(point);
        //        }

        //        Console.WriteLine("Route created with ID: " + route.Id);
        //        Console.ReadKey();
        //    }

        //    static void ViewRoutes()
        //    {
        //        Console.Clear();
        //        Console.WriteLine("List of Routes:");

        //        List<Route> routes = Route.ListRoutes();
        //        if (routes.Count > 0)
        //        {
        //            foreach (var route in routes)
        //            {
        //                Console.WriteLine($"ID: {route.Id}, Name: {route.Name}, Description: {route.Description}");
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("No routes found.");
        //        }

        //        Console.ReadKey();
        //    }

        //    static void UpdateRoute()
        //    {
        //        Console.Clear();
        //        Console.WriteLine("Enter the ID of the route to update:");
        //        int routeId = int.Parse(Console.ReadLine());

        //        Route routeToUpdate = Route.Find(routeId);
        //        if (routeToUpdate != null)
        //        {
        //            Console.WriteLine("Enter new Route name:");
        //            string name = Console.ReadLine();
        //            Console.WriteLine("Enter new Route description:");
        //            string description = Console.ReadLine();
        //            Console.WriteLine("Enter new User ID:");
        //            int userId = int.Parse(Console.ReadLine());
        //            Console.WriteLine("Enter new Area ID:");
        //            int areaId = int.Parse(Console.ReadLine());

        //            //routepoint class moet nog aangemaakt worden
        //            var routePoints = new List<RoutePoint>();
        //            Console.WriteLine("Enter new Route Points (type 'done' to finish):");
        //            while (true)
        //            {
        //                Console.Write("Enter latitude: ");
        //                string latInput = Console.ReadLine();
        //                if (latInput.ToLower() == "done") break;
        //                Console.Write("Enter longitude: ");
        //                string lngInput = Console.ReadLine();
        //                routePoints.Add(new RoutePoint { Latitude = double.Parse(latInput), Longitude = double.Parse(lngInput) });
        //            }

        //            Route.Update(routeId, name, description, userId, areaId);
        //            foreach (var point in routePoints)
        //            {
        //                routeToUpdate.AddRoutePoint(point);
        //            }

        //            Console.WriteLine("Route updated successfully!");
        //        }
        //        else
        //        {
        //            Console.WriteLine("Route not found.");
        //        }

        //        Console.ReadKey();
        //    }

        //    static void DeleteRoute()
        //    {
        //        Console.Clear();
        //        Console.WriteLine("Enter the ID of the route to delete:");
        //        int routeId = int.Parse(Console.ReadLine());

        //        Route routeToDelete = Route.Find(routeId);
        //        if (routeToDelete != null)
        //        {
        //            Route.Delete(routeId);
        //            Console.WriteLine("Route deleted successfully!");
        //        }
        //        else
        //        {
        //            Console.WriteLine("Route not found.");
        //        }
        //    }
    }
}
