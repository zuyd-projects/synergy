using System;
using System.Collections.Generic;
using ExotischNederland.Models;

namespace ExotischNederland.Menus
{
    internal class MainMenu : IMenu
    {
        private readonly User authenticatedUser;
        private Dictionary<string, string> menuItems;

        public MainMenu(User _authenticatedUser)
        {
            this.authenticatedUser = _authenticatedUser;
            this.menuItems = new Dictionary<string, string>();
        }

        public Dictionary<string, string> GetMenuItems()
        {
            var menuItems = new Dictionary<string, string>();
            
            // Observations menu
            if (authenticatedUser.Permission.CanViewAllObservations() || authenticatedUser.Permission.CanCreateObservation())
                menuItems.Add("observations", "Observaties Beheren");
            // Areas menu
            if (authenticatedUser.Permission.CanViewAllAreas())
                menuItems.Add("areas", "Natuurgebieden Beheren");
            // Games menu
            if (authenticatedUser.Permission.CanManageGames())
                menuItems.Add("manage_games", "Spellen Beheren");
            // Play game menu
            if (authenticatedUser.Permission.CanPlayGames())
                menuItems.Add("play_game", "Spel Spelen");
            // Routes menu
            if (authenticatedUser.Permission.CanManageRoutes())
                menuItems.Add("routes", "Routes Beheren");
            // View routes menu
            if (authenticatedUser.Permission.CanViewRoutes())
                menuItems.Add("view_routes", "Routes Bekijken");
            // Points of Interest menu
            if (authenticatedUser.Permission.CanViewPointsOfInterest() || authenticatedUser.Permission.CanCreatePointOfInterest())
                menuItems.Add("points_of_interest", "Points of Interest Beheren");
            // Users menu
            if (authenticatedUser.Permission.CanViewAllUsers())
                menuItems.Add("users", "Gebruikers Beheren");

            menuItems.Add("logout", "Logout");
            return menuItems;
        }

        public void Show()
        {
            menuItems = GetMenuItems();
            while (true)
            {
                string selected = Helpers.MenuSelect(menuItems, true, new List<string> { "Database is online" });

                // Invoke respective menu based on selection
                switch (selected)
                {
                    case "observations":
                        ShowMenu(new ObservationMenu(authenticatedUser));
                        break;
                    case "areas":
                        ShowMenu(new AreaMenu(authenticatedUser));
                        break;
                    case "manage_games":
                        ShowMenu(new GameMenu(authenticatedUser));
                        break;
                    case "play_game":
                        ShowMenu(new GameMenu(authenticatedUser));
                        break;
                    case "routes":
                        ShowMenu(new RouteMenu(authenticatedUser));
                        break;
                    case "view_routes":
                        ShowMenu(new RouteMenu(authenticatedUser));
                        break;
                    case "points_of_interest":
                        ShowMenu(new PointOfInterestMenu(authenticatedUser));
                        break;
                    case "users":
                        ShowMenu(new UserMenu(authenticatedUser));
                        break;
                    case "logout":
                        Logout();
                        return;
                    default:
                        InvalidSelection();
                        break;
                }
            }
        }

        private void ShowMenu(IMenu menu)
        {
            menu.Show();
        }

        private void Logout()
        {
            Console.Clear();
            Console.WriteLine("You have logged out.");
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
        }

        private void InvalidSelection()
        {
            Console.WriteLine("Invalid selection.");
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
        }
    }
}