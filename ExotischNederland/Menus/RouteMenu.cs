using System;
using System.Collections.Generic;
using ExotischNederland.Models;

namespace ExotischNederland.Menus
{
    internal class RouteMenu : IMenu
    {
        private readonly User authenticatedUser;
        private Dictionary<string, string> menuItems = new Dictionary<string, string>();

        public RouteMenu(User _authenticatedUser)
        {
            authenticatedUser = _authenticatedUser;
        }

        public Dictionary<string, string> GetMenuItems()
        {
            var menuItems = new Dictionary<string, string>();
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
            menuItems = GetMenuItems();
            while (true)
            {
                string selected = Helpers.MenuSelect(menuItems, true);

                if (selected == "viewAllRoutes") ViewAllRoutes();
                else if (selected == "createRoute") CreateRoute();
                else if (selected == "editRoute") EditRoute();
                else if (selected == "deleteRoute") DeleteRoute();
                else if (selected == "manageRoutePoints") ManageRoutePoints();
                else if (selected == "back") break;
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
            var fields = new List<FormField>
            {
                new FormField("name", "Enter route name", "string", true),
                new FormField("description", "Enter route description", "string", true)
            };
            var values = new Form(fields).Prompt();
            if (values == null) return;

            Route route = Route.Create((string)values["name"], (string)values["description"], authenticatedUser);
            Console.WriteLine($"Route '{values["name"]}' created successfully with ID: {route.Id}");
            Console.ReadKey();
        }

        private void EditRoute()
        {
            Console.Clear();
            Console.Write("Enter route ID to edit: ");
            if (int.TryParse(Console.ReadLine(), out int routeId))
            {
                Route route = Route.Find(routeId);
                if (route == null || !authenticatedUser.Permission.CanEditRoute(route))
                {
                    Console.WriteLine("Route not found or you do not have permission to edit.");
                    Console.ReadKey();
                    return;
                }

                var fields = new List<FormField>
                {
                    new FormField("name", "New name (leave blank to keep current)", "string", false, route.Name),
                    new FormField("description", "New description (leave blank to keep current)", "string", false, route.Description)
                };
                var values = new Form(fields).Prompt();
                if (values == null) return;

                route.Update((string)values["name"], (string)values["description"]);
                Console.WriteLine("Route updated successfully.");
                Console.ReadKey();
            }
        }

        private void DeleteRoute()
        {
            Console.Clear();
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
                    Console.WriteLine("Route not found or you do not have permission to delete.");
                }
                Console.ReadKey();
            }
        }

        private void ManageRoutePoints()
        {
            Console.Clear();
            Console.Write("Enter route ID to manage points: ");
            if (int.TryParse(Console.ReadLine(), out int routeId))
            {
                Route route = Route.Find(routeId);
                if (route == null || !authenticatedUser.Permission.CanEditRoute(route))
                {
                    Console.WriteLine("Route not found or you do not have permission to manage points.");
                    Console.ReadKey();
                    return;
                }

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"Managing Points for Route '{route.Name}' (ID: {route.Id})");
                    Console.WriteLine("1. Add Route Point");
                    Console.WriteLine("2. Edit Route Point");
                    Console.WriteLine("3. Delete Route Point");
                    Console.WriteLine("4. Back to Route Menu");

                    string choice = Console.ReadLine();
                    if (choice == "1") AddRoutePoint(route);
                    else if (choice == "2") EditRoutePoint(route);
                    else if (choice == "3") DeleteRoutePoint(route);
                    else if (choice == "4") break;
                }
            }
        }

        private void AddRoutePoint(Route route)
        {
            var fields = new List<FormField>
            {
                new FormField("order", "Enter order for this point", "number", true),
                new FormField("poiId", "Enter Point of Interest ID", "number", true)
            };
            var values = new Form(fields).Prompt();
            if (values == null) return;

            int order = (int)values["order"];
            int poiId = (int)values["poiId"];
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
            Console.ReadKey();
        }

        private void EditRoutePoint(Route route)
        {
            Console.Clear();
            Console.Write("Enter Route Point ID to edit: ");
            if (int.TryParse(Console.ReadLine(), out int pointId))
            {
                RoutePoint point = RoutePoint.Find(pointId);
                if (point == null)
                {
                    Console.WriteLine("Route Point not found.");
                    Console.ReadKey();
                    return;
                }

                var fields = new List<FormField>
                {
                    new FormField("order", "Enter new order", "number", true, point.Order.ToString()),
                    new FormField("poiId", "Enter new Point of Interest ID", "number", true, point.PointOfInterest.Id.ToString())
                };
                var values = new Form(fields).Prompt();
                if (values == null) return;

                int newOrder = (int)values["order"];
                int newPoiId = (int)values["poiId"];
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
                Console.ReadKey();
            }
        }

        private void DeleteRoutePoint(Route route)
        {
            Console.Clear();
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
            Console.ReadKey();
        }
    }
}