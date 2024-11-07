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
                menuItems.Add("areas", "Gebieden");

            // Game-related options based on user role
            if (authenticatedUser.Permission.CanManageGames() || authenticatedUser.Permission.CanPlayGames())
                menuItems.Add("games", "Spellen");

            if (authenticatedUser.Permission.CanManageRoutes())
                menuItems.Add("routes", "Routes Beheren");

            // Points of Interest menu
            if (authenticatedUser.Permission.CanViewPointsOfInterest() || authenticatedUser.Permission.CanCreatePointOfInterest())
                menuItems.Add("points_of_interest", "Points of Interests");
            // Users menu
            if (authenticatedUser.Permission.CanViewAllUsers())
                menuItems.Add("users", "Gebruikers Beheren");

            menuItems.Add("logout", "Logout");
            return menuItems;
        }

        public void Show()
        {
            this.menuItems = this.GetMenuItems();
            while (true)
            {
                List<string> text = new List<string> { "Database is online" };
                string selected = Helpers.MenuSelect(this.menuItems, true, text);

                if (selected == "observations")
                {
                    ObservationMenu observationMenu = new ObservationMenu(this.authenticatedUser);
                    observationMenu.Show();
                }
                else if (selected == "areas")
                {
                    AreaMenu areaMenu = new AreaMenu(this.authenticatedUser);
                    areaMenu.Show();
                }
                else if (selected == "routes")
                {
                    RouteMenu routeMenu = new RouteMenu(this.authenticatedUser);
                    routeMenu.Show();
                }
                else if (selected == "points_of_interest")
                {
                    PointOfInterestMenu pointOfInterestMenu = new PointOfInterestMenu(this.authenticatedUser);
                    pointOfInterestMenu.Show();
                }
                else if (selected == "games")
                {
                    GameMenu gameMenu = new GameMenu(this.authenticatedUser);
                    gameMenu.Show(); 
                }
                else if (selected == "users")
                {
                    UserMenu userMenu = new UserMenu(this.authenticatedUser);
                    userMenu.Show();
                }
                else if (selected == "logout")
                {
                    Console.Clear();
                    Console.WriteLine("U bent uitgelogd");
                    Console.WriteLine("Druk op een toets om terug te gaan naar het hoofdmenu");
                    Console.ReadKey();
                    return;
                }
            }
        }
    }
}