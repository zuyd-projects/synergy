using System;
using System.Collections.Generic;
using System.Linq;
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
                else if (selected == "back") break;
            }
        }

        private void ViewAllRoutes()
        {
            Console.Clear();
            List<Route> routes = Route.GetAll();

            Console.WriteLine("All Routes:");
            if (routes.Count == 0)
            {
                Console.WriteLine("Geen routes beschikbaar");
                Console.ReadKey();
                return;
            }

            var routeOptions = routes.ToDictionary(x => x.Id.ToString(), x => $"{x.Name} - {x.Description}");
            routeOptions.Add("back", "Ga terug");

            string selectedRouteId = Helpers.MenuSelect(routeOptions, false);
            if (selectedRouteId == "back" || selectedRouteId is null) return;

            int routeId = int.Parse(selectedRouteId);
            Route route = routes.Find(r => r.Id == routeId);
            ViewRouteDetails(route);
            
        }

        private void ViewRouteDetails(Route route)
        {
            Console.Clear();
            Console.WriteLine("Route Details:");
            Console.WriteLine($"Name: {route.Name}");
            Console.WriteLine($"Description: {route.Description}");

            var options = new Dictionary<string, string> { { "back", "Ga terug" } };
            if (authenticatedUser.Permission.CanEditRoute(route))
            {
                options.Add("edit", "Edit this route");
                options.Add("managePoints", "Manage Route Points");
            }
            if (authenticatedUser.Permission.CanDeleteRoute(route))
            {
                options.Add("delete", "Delete this route");
            }

            string selectedOption = Helpers.MenuSelect(options, true);

            if (selectedOption == "edit") EditRoute(route);
            else if (selectedOption == "managePoints") ManageRoutePoints(route);
            else if (selectedOption == "delete") DeleteRoute(route);
        }

        private void CreateRoute()
        {
            Dictionary<string, string> areaOptions = Area.GetAll().ToDictionary(x => x.Id.ToString(), x => x.Name);
            var fields = new List<FormField>
            {
                new FormField("name", "Enter route name", "string", true),
                new FormField("description", "Enter route description", "string", true),
                new FormField("area", "Kies een gebied", "single_select", true, null, areaOptions)
            };
            var values = new Form(fields).Prompt();
            if (values == null) return;

            int areaId = int.Parse((string)values["area"]);
            Area area = Area.Find(areaId);  // Ensure areaId is provided and not null
            Route route = Route.Create((string)values["name"], (string)values["description"], area, authenticatedUser);
            Console.WriteLine($"Route '{values["name"]}' created successfully with ID: {route.Id}");
            Console.ReadKey();
        }

        private void EditRoute(Route route)
        {
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

        private void DeleteRoute(Route route)
        {
            Console.Clear();
            Console.Write($"Are you sure you want to delete the route '{route.Name}'? [Y/N] ");
            ConsoleKey confirmation = Console.ReadKey().Key;

            if (confirmation == ConsoleKey.Y)
            {
                Route.Delete(route.Id);
                Console.WriteLine("\nRoute deleted successfully.");
            }
            else
            {
                ViewRouteDetails(route);
            }
            Console.ReadKey();
        }

        private void ManageRoutePoints(Route route)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Managing Points for Route '{route.Name}' (ID: {route.Id})");
                Console.WriteLine("1. Add Route Point");
                Console.WriteLine("2. Edit Route Point");
                Console.WriteLine("3. Delete Route Point");
                Console.WriteLine("4. Back to Route Details");

                string choice = Console.ReadLine();
                if (choice == "1") AddRoutePoint(route);
                else if (choice == "2") EditRoutePoint(route);
                else if (choice == "3") DeleteRoutePoint(route);
                else if (choice == "4") break;
            }
        }

        private void AddRoutePoint(Route route)
        {
            Dictionary<string, string> poiOptions = PointOfInterest.GetAll().ToDictionary(x => x.Id.ToString(), x => x.Name);
            var fields = new List<FormField>
            {
                new FormField("order", "Enter order for this point", "number", true),
                new FormField("poiId", "Choose a Point of Interest", "single_select", true, null, poiOptions)
            };
            var values = new Form(fields).Prompt();
            if (values == null) return;

            int order = (int)values["order"];
            int poiId = int.Parse((string)values["poiId"]);
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
            //TODO: @Rick, add select here please
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
                    //TODO: @Rick, add select here please
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
            //TODO: @Rick, add select here please
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
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Invalid input for Route Point ID.");
            }
        }
    }
}