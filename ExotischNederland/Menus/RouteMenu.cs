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
                menuItems.Add("viewAllRoutes", "Bekijk alle routes");
            if (permission.CanCreateRoute())
                menuItems.Add("createRoute", "Route aanmaken");

            menuItems.Add("back", "Keer terug naar het hoofdmenu");
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
            List<Route> routes = Route.GetAllRoutes();

            Console.WriteLine("All Routes:");
            if (routes.Count > 0)
            {
                var routeOptions = new Dictionary<string, string>();
                foreach (var route in routes)
                {
                    routeOptions[route.Id.ToString()] = $"{route.Name} - {route.Description}";
                }
                routeOptions.Add("back", "Terug naar menu");

                string selectedRouteId = Helpers.MenuSelect(routeOptions, false);
                if (selectedRouteId != "back")
                {
                    int routeId = int.Parse(selectedRouteId);
                    Route route = routes.Find(r => r.Id == routeId);
                    ViewRouteDetails(route);
                }
            }
            else
            {
                Console.WriteLine("Geen routes beschikbaar.");
                Console.ReadKey();
            }
        }

        private void ViewRouteDetails(Route route)
        {
            Console.Clear();
            Console.WriteLine("Routedetails:");
            Console.WriteLine($"Naam: {route.Name}");
            Console.WriteLine($"Beschrijving: {route.Description}");

            var options = new Dictionary<string, string> { { "back", "Terug naar routelijst" } };
            if (authenticatedUser.Permission.CanEditRoute(route))
            {
                options.Add("edit", "Bewerk deze route");
                options.Add("managePoints", "Routepunten beheren");
            }
            if (authenticatedUser.Permission.CanDeleteRoute(route))
            {
                options.Add("delete", "Verwijder deze route");
            }

            string selectedOption = Helpers.MenuSelect(options, true);

            if (selectedOption == "edit") EditRoute(route);
            else if (selectedOption == "managePoints") ManageRoutePoints(route);
            else if (selectedOption == "delete") DeleteRoute(route);
        }

        private void CreateRoute()
        {
            var fields = new List<FormField>
            {
                new FormField("name", "Voer de routenaam in", "string", true),
                new FormField("description", "Routebeschrijving invoeren", "string", true),
                new FormField("areaId", "Voer gebieds-ID voor de route in", "number", true)  // Added area ID field
            };
            var values = new Form(fields).Prompt();
            if (values == null) return;

            int areaId = (int)values["areaId"];  // Ensure areaId is provided and not null
            Route route = Route.Create((string)values["name"], (string)values["description"], areaId, authenticatedUser);
            Console.WriteLine($"Route '{values["name"]}' succesvol aangemaakt met ID: {route.Id}");
            Console.ReadKey();
        }

        private void EditRoute(Route route)
        {
            var fields = new List<FormField>
            {
                new FormField("name", "Nieuwe naam (leeg laten om te behouden)", "string", false, route.Name),
                new FormField("description", "Nieuwe beschrijving (leeg laten om te behouden", "string", false, route.Description)
            };
            var values = new Form(fields).Prompt();
            if (values == null) return;

            route.Update((string)values["name"], (string)values["description"]);
            Console.WriteLine("Route succesvol bijgewerkt.");
            Console.ReadKey();
        }

        private void DeleteRoute(Route route)
        {
            Console.Clear();
            Console.Write($"Weet u zeker dat u de route wilt verwijderen? '{route.Name}'? [Y/N] ");
            ConsoleKey confirmation = Console.ReadKey().Key;

            if (confirmation == ConsoleKey.Y)
            {
                Route.Delete(route.Id);
                Console.WriteLine("Route succesvol verwijderd.");
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
                Console.WriteLine($"Punten voor route beheren '{route.Name}' (ID: {route.Id})");
                Console.WriteLine("1. Routepunt toevoegen");
                Console.WriteLine("2. Routepunt toevoegen");
                Console.WriteLine("3. Routepunt verwijderen");
                Console.WriteLine("4. Terug naar Routedetails");

                string choice = Console.ReadLine();
                if (choice == "1") AddRoutePoint(route);
                else if (choice == "2") EditRoutePoint(route);
                else if (choice == "3") DeleteRoutePoint(route);
                else if (choice == "4") break;
            }
        }

        private void AddRoutePoint(Route route)
        {
            var fields = new List<FormField>
            {
                new FormField("order", "Voer de volgorde voor dit punt in", "number", true),
                new FormField("poiId", "Voer InteressePunt-ID in", "number", true)
            };
            var values = new Form(fields).Prompt();
            if (values == null) return;

            int order = (int)values["order"];
            int poiId = (int)values["poiId"];
            PointOfInterest poi = PointOfInterest.Find(poiId);

            if (poi != null)
            {
                RoutePoint point = RoutePoint.Create(route, order, poi);
                Console.WriteLine($"Routepunt toegevoegd met ID: {point.Id}");
            }
            else
            {
                Console.WriteLine("Interesse Punt niet gevonden.");
            }
            Console.ReadKey();
        }

        private void EditRoutePoint(Route route)
        {
            Console.Write("Voer Routepunt-ID in om te bewerken: ");
            if (int.TryParse(Console.ReadLine(), out int pointId))
            {
                RoutePoint point = RoutePoint.Find(pointId);
                if (point == null)
                {
                    Console.WriteLine("Routepunt niet gevonden.");
                    Console.ReadKey();
                    return;
                }

                var fields = new List<FormField>
                {
                    new FormField("order", "Voer een nieuwe bestelling in", "number", true, point.Order.ToString()),
                    new FormField("poiId", "Voer een nieuwe InteressePunt-ID in", "number", true, point.PointOfInterest.Id.ToString())
                };
                var values = new Form(fields).Prompt();
                if (values == null) return;

                int newOrder = (int)values["order"];
                int newPoiId = (int)values["poiId"];
                PointOfInterest newPoi = PointOfInterest.Find(newPoiId);

                if (newPoi != null)
                {
                    point.Update(newOrder, newPoi);
                    Console.WriteLine("Routepunt is succesvol bijgewerkt.");
                }
                else
                {
                    Console.WriteLine("Nieuwe nuttige plaats niet gevonden.");
                }
                Console.ReadKey();
            }
        }

        private void DeleteRoutePoint(Route route)
        {
            Console.Write("Voer Routepunt-ID in om te verwijderen: ");
            if (int.TryParse(Console.ReadLine(), out int pointId))
            {
                RoutePoint point = RoutePoint.Find(pointId);
                if (point != null)
                {
                    RoutePoint.Delete(pointId);
                    Console.WriteLine("Routepunt is succesvol verwijderd.");
                }
                else
                {
                    Console.WriteLine("Routepunt niet gevonden.");
                }
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Ongeldige invoer voor routepunt-ID.");
            }
        }
    }
}