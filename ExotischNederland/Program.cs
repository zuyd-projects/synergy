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
                debugUser = User.Find(1194); // Admin user
                User authenticatedUser = debugUser ?? MainMenu.Show();
                if (authenticatedUser is null) return;
                UserMenu menu = new UserMenu(authenticatedUser);
                menu.Show();
            }

            
            var poi = PointOfInterest.Create("Eiffeltoren", "Cultural", 2.2945f, 48.8584f);
            Console.WriteLine($"Nieuw punt toegevoegd: {poi.Name}, ID: {poi.Id}");

            
            var foundPoi = PointOfInterest.Find(poi.Id);
            if (foundPoi != null)
            {
                Console.WriteLine($"Gevonden punt: {foundPoi.Name}, Type: {foundPoi.Type}");
            }

            
            PointOfInterest.Update(poi.Id, "Eiffeltoren Paris", "Historical", 2.2945f, 48.8584f);
            Console.WriteLine("Locatie bijgewerkt.");

            
            PointOfInterest.Delete(poi.Id);
            Console.WriteLine("Locatie verwijderd.");
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
