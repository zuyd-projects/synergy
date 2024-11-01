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
                debugUser = User.Find(1); // Uncomment this line to debug as a specific user
                User authenticatedUser = debugUser ?? MainMenu.Show();
                if (authenticatedUser is null) return;
                UserMenu menu = new UserMenu(authenticatedUser);
                menu.Show();
            }
        }

        //    static void AreaMenu(User user)
        //    {
        //        Dictionary<string, string> areaMenuItems = new Dictionary<string, string>
        //        {
        //            { "1", "Create Area" },
        //            { "2", "View Areas" },
        //            { "3", "Update Area" },
        //            { "4", "Delete Area" },
        //            { "5", "Logout" }
        //        };

        //        while (true)
        //        {
        //            string selected = Helpers.MenuSelect(areaMenuItems, true);
        //            if (selected == "1")
        //            {
        //                CreateArea();
        //            }
        //            else if (selected == "2")
        //            {
        //                ViewAreas();
        //            }
        //            else if (selected == "3")
        //            {
        //                UpdateArea();
        //            }
        //            else if (selected == "4")
        //            {
        //                DeleteArea();
        //            }
        //            else if (selected == "5")
        //            {
        //                Console.Clear();
        //                break;
        //            }
        //        }
        //    }
        //    static void CreateArea()
        //    {
        //        Console.Clear();
        //        Console.WriteLine("Creating a new area...");

        //        Console.WriteLine("Enter Area name:");
        //        string name = Console.ReadLine();
        //        Console.WriteLine("Enter Area description:");
        //        string description = Console.ReadLine();

        //        var polygonCoordinates = new List<(double lat, double lng)>();
        //        Console.WriteLine("Enter Polygon Points (type 'done' to finish):");
        //        while (true)
        //        {
        //            Console.Write("Enter latitude: ");
        //            string latInput = Console.ReadLine();
        //            if (latInput.ToLower() == "done") break;
        //            Console.Write("Enter longitude: ");
        //            string lngInput = Console.ReadLine();
        //            polygonCoordinates.Add((double.Parse(latInput), double.Parse(lngInput)));
        //        }

        //        Area area = Area.Create(name, description, polygonCoordinates);
        //        Console.WriteLine("Area created with ID: " + area.Id);
        //        Console.ReadKey();
        //    }

        //    static void ViewAreas()
        //    {
        //        Console.Clear();
        //        Console.WriteLine("List of Areas:");

        //        List<Area> areas = Area.ListAreas();
        //        if (areas.Count > 0)
        //        {
        //            foreach (var area in areas)
        //            {
        //                Console.WriteLine($"ID: {area.Id}, Name: {area.Name}, Description: {area.Description}");
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("No areas found.");
        //        }

        //        Console.ReadKey();
        //    }

        //    static void UpdateArea()
        //    {
        //        Console.Clear();
        //        Console.WriteLine("Enter the ID of the area to update:");
        //        int areaId = int.Parse(Console.ReadLine());

        //        Area areaToUpdate = Area.Find(areaId);
        //        if (areaToUpdate != null)
        //        {
        //            Console.WriteLine("Enter new Area name:");
        //            string name = Console.ReadLine();
        //            Console.WriteLine("Enter new Area description:");
        //            string description = Console.ReadLine();

        //            var polygonCoordinates = new List<(double lat, double lng)>();
        //            Console.WriteLine("Enter new Polygon Points (type 'done' to finish):");
        //            while (true)
        //            {
        //                Console.Write("Enter latitude: ");
        //                string latInput = Console.ReadLine();
        //                if (latInput.ToLower() == "done") break;
        //                Console.Write("Enter longitude: ");
        //                string lngInput = Console.ReadLine();
        //                polygonCoordinates.Add((double.Parse(latInput), double.Parse(lngInput)));
        //            }

        //            Area.Update(areaId, name, description, polygonCoordinates);
        //            Console.WriteLine("Area updated successfully!");
        //        }
        //        else
        //        {
        //            Console.WriteLine("Area not found.");
        //        }

        //        Console.ReadKey();
        //    }

        //    static void DeleteArea()
        //    {
        //        Console.Clear();
        //        Console.WriteLine("Enter the ID of the area to delete:");
        //        int areaId = int.Parse(Console.ReadLine());

        //        Area areaToDelete = Area.Find(areaId);
        //        if (areaToDelete != null)
        //        {
        //            Area.Delete(areaId);
        //            Console.WriteLine("Area deleted successfully!");
        //        }
        //        else
        //        {
        //            Console.WriteLine("Area not found.");
        //        }

        //        Console.ReadKey();
        //    }
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
