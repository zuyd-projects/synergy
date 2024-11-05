using System;
using System.Collections.Generic;
using ExotischNederland.Models;

namespace ExotischNederland.Menus
{
    internal class MainMenu: IMenu
    {
        private readonly User authenticatedUser;
        private Dictionary<string, string> menuItems = new Dictionary<string, string>();

        public MainMenu(User _authenticatedUser)
        {
            this.authenticatedUser = _authenticatedUser;
        }

        public Dictionary<string, string> GetMenuItems()
        {
            Dictionary<string, string> menuItems = new Dictionary<string, string>();


            // Observation and Area menu options
            if (authenticatedUser.Permission.CanViewAllObservations() || authenticatedUser.Permission.CanCreateObservation())
                menuItems.Add("observations", "Observaties");
            if (authenticatedUser.Permission.CanViewAllAreas())
                menuItems.Add("areas", "Gebieden");

            // Game-related options based on user role
            if (authenticatedUser.Permission.CanManageGames() || authenticatedUser.Permission.CanPlayGames())
                menuItems.Add("games", "Spellen");
            
            // Route menu option
            if (authenticatedUser.Permission.CanManageRoutes())
                menuItems.Add("routes", "Routes");
            if (authenticatedUser.Permission.CanViewRoutes())
                menuItems.Add("view_routes", "Bekijk Routes");

            // User menu option
            if (this.authenticatedUser.Permission.CanViewAllUsers())
                menuItems.Add("users", "Gebruikers");
            
           
            menuItems.Add("logout", "Uitloggen");
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