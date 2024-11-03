using System;
using System.Collections.Generic;
using ExotischNederland.Models;

namespace ExotischNederland.Menus
{
    internal class RouteMenu
    {
        private readonly User authenticatedUser;
        private Dictionary<string, string> menuItems = new Dictionary<string, string>();

        public RouteMenu(User _authenticatedUser)
        {
            this.authenticatedUser = _authenticatedUser;
        }

        public Dictionary<string, string> GetMenuItems()
        {
            Dictionary<string, string> menuItems = new Dictionary<string, string>();
            
            var permission = authenticatedUser.Permission;

            if (permission.CanViewRoutes())
                menuItems.Add("viewAllRoutes", "View All Routes");
            if (permission.CanCreateRoute())
                menuItems.Add("createRoute", "Create Route");
            if (permission.CanEditRoute(null))
                menuItems.Add("editRoute", "Edit Route");
            if (permission.CanDeleteRoute(null))
                menuItems.Add("deleteRoute", "Delete Route");
            if (permission.CanEditRoute(null))
                menuItems.Add("manageRoutePoints", "Manage Route Points");

            menuItems.Add("back", "Return to main menu");
            return menuItems;
        }

        public void Show()
        {
            menuItems = this.GetMenuItems();
            while (true)
            {
                string selected = Helpers.MenuSelect(this.menuItems, true);

                if (selected == "viewAllRoutes")
                {
                    ViewAllRoutes();
                }
                else if (selected == "createRoute" && authenticatedUser.Permission.CanCreateRoute())
                {
                    CreateRoute();
                }
                else if (selected == "editRoute" && authenticatedUser.Permission.CanEditRoute(null))
                {
                    EditRoute();
                }
                else if (selected == "deleteRoute" && authenticatedUser.Permission.CanDeleteRoute(null))
                {
                    DeleteRoute();
                }
                else if (selected == "manageRoutePoints" && authenticatedUser.Permission.CanEditRoute(null))
                {
                    ManageRoutePoints();
                }
                else if (selected == "back")
                {
                    break;
                }
            }
        }

        private void ViewAllRoutes()
        {
            Console.Clear();
            List<Route> routes = Route.GetAllRoutes();

            Console.WriteLine("All Routes:");
            foreach (var route in routes)
            {
                Console.WriteLine($"ID: {route.Id}, Name: {route.Name}, Description: {route.Description}");
            }

            Console.WriteLine("Press any key to return to the menu.");
            Console.ReadKey();
        }

        private void CreateRoute()
        {
            if (!authenticatedUser.Permission.CanCreateRoute())
            {
                Console.WriteLine("You do not have permission to create routes.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("Create New Route");

            Console.Write("Enter route name: ");
            string name = Console.ReadLine();

            Console.Write("Enter route description: ");
            string description = Console.ReadLine();

            Route route = Route.Create(name, description, authenticatedUser);
            Console.WriteLine($"Route '{name}' created successfully with ID: {route.Id}");

            Console.WriteLine("Press any key to return to the menu.");
            Console.ReadKey();
        }

        private void EditRoute()
        {
            Console.Clear();
            Console.WriteLine("Edit Route");

            Console.Write("Enter route ID to edit: ");
            if (int.TryParse(Console.ReadLine(), out int routeId))
            {
                Route route = Route.Find(routeId);
                if (route != null && authenticatedUser.Permission.CanEditRoute(route))
                {
                    Console.Write("Enter new name (leave blank to keep current): ");
                    string newName = Console.ReadLine();
                    Console.Write("Enter new description (leave blank to keep current): ");
                    string newDescription = Console.ReadLine();

                    route.Update(newName == "" ? route.Name : newName, newDescription == "" ? route.Description : newDescription);
                    Console.WriteLine("Route updated successfully.");
                }
                else
                {
                    Console.WriteLine("Route not found or you do not have permission to edit this route.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input for route ID.");
            }

            Console.WriteLine("Press any key to return to the menu.");
            Console.ReadKey();
        }

        private void DeleteRoute()
        {
            Console.Clear();
            Console.WriteLine("Delete Route");

            Console.Write("Enter route ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int routeId))
            {
                Route route = Route.Find(routeId);
                if (route != null && authenticatedUser.Permission.CanDeleteRoute(route))
                {
                    Route.Delete(routeId);
                    Console.WriteLine("Route deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Route not found or you do not have permission to delete this route.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input for route ID.");
            }

            Console.WriteLine("Press any key to return to the menu.");
            Console.ReadKey();
        }

        private void ManageRoutePoints()
        {
            Console.Clear();
            Console.WriteLine("Manage Route Points");

            Console.Write("Enter route ID to manage points: ");
            if (int.TryParse(Console.ReadLine(), out int routeId))
            {
                Route route = Route.Find(routeId);
                if (route != null && authenticatedUser.Permission.CanEditRoute(route))
                {
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine($"Managing Points for Route '{route.Name}' (ID: {route.Id})");
                        Console.WriteLine("1. Add Route Point");
                        Console.WriteLine("2. Edit Route Point");
                        Console.WriteLine("3. Delete Route Point");
                        Console.WriteLine("4. Back to Route Menu");

                        string choice = Console.ReadLine();
                        if (choice == "1")
                        {
                            AddRoutePoint(route);
                        }
                        else if (choice == "2")
                        {
                            EditRoutePoint(route);
                        }
                        else if (choice == "3")
                        {
                            DeleteRoutePoint(route);
                        }
                        else if (choice == "4")
                        {
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Route not found or you do not have permission to manage points for this route.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input for route ID.");
            }
        }

        private void AddRoutePoint(Route route)
        {
            Console.Clear();
            Console.WriteLine("Add Route Point");

            Console.Write("Enter order for this point: ");
            int order = int.Parse(Console.ReadLine());

            Console.Write("Enter Point of Interest ID: ");
            int poiId = int.Parse(Console.ReadLine());
            PointOfInterest poi = PointOfInterest.Find(poiId);

            if (poi != null)
            {
                RoutePoint point = RoutePoint.Create(route, order, poi);
                Console.WriteLine($"Route Point added with ID: {point.Id}");
            }
            else
            {
                Console.WriteLine("Point of Interest not found.");
            }

            Console.WriteLine("Press any key to return to the menu.");
            Console.ReadKey();
        }

        private void EditRoutePoint(Route route)
        {
            Console.Clear();
            Console.WriteLine("Edit Route Point");

            Console.Write("Enter Route Point ID to edit: ");
            if (int.TryParse(Console.ReadLine(), out int pointId))
            {
                RoutePoint point = RoutePoint.Find(pointId);
                if (point != null)
                {
                    Console.Write("Enter new order: ");
                    int newOrder = int.Parse(Console.ReadLine());

                    Console.Write("Enter new Point of Interest ID: ");
                    int newPoiId = int.Parse(Console.ReadLine());
                    PointOfInterest newPoi = PointOfInterest.Find(newPoiId);

                    if (newPoi != null)
                    {
                        point.Update(newOrder, newPoi);
                        Console.WriteLine("Route Point updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("New Point of Interest not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Route Point not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input for Route Point ID.");
            }

            Console.WriteLine("Press any key to return to the menu.");
            Console.ReadKey();
        }

        private void DeleteRoutePoint(Route route)
        {
            Console.Clear();
            Console.WriteLine("Delete Route Point");

            Console.Write("Enter Route Point ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int pointId))
            {
                RoutePoint point = RoutePoint.Find(pointId);
                if (point != null)
                {
                    RoutePoint.Delete(pointId);
                    Console.WriteLine("Route Point deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Route Point not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input for Route Point ID.");
            }

            Console.WriteLine("Press any key to return to the menu.");
            Console.ReadKey();
        }
    }
}