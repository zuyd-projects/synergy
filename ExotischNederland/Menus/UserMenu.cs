using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ExotischNederland.Models;

namespace ExotischNederland.Menus
{
    internal class UserMenu: IMenu
    {
        private readonly User authenticatedUser;
        private Dictionary<string, string> menuItems = new Dictionary<string, string>();

        public UserMenu(User _authenticatedUser) 
        {
            this.authenticatedUser = _authenticatedUser;
        }

        public Dictionary<string, string> GetMenuItems()
        {
            Dictionary<string, string> menuItems = new Dictionary<string, string>();
            // Logic to add menu items
            if (this.authenticatedUser.Permission.CanViewAllObservations() || this.authenticatedUser.Permission.CanCreateObservation()) menuItems.Add("observations", "Observaties");
            if (this.authenticatedUser.Permission.CanViewAllAreas()) menuItems.Add("areas", "Gebieden");
            menuItems.Add("logout", "Uitloggen");
            return menuItems;
        }

        public void Show()
        {
            this.menuItems = this.GetMenuItems();
            while (true)
            {
                List<string> text = new List<string>
                {
                    "Database is online"
                };
                string selected = Helpers.MenuSelect(this.menuItems, true, text);

                if (selected == "observations")
                {
                    ObservationMenu observarionMenu = new ObservationMenu(this.authenticatedUser);
                    observarionMenu.Show();
                }

                if (selected == "areas")
                {
                    AreaMenu areaMenu = new AreaMenu(this.authenticatedUser);
                    areaMenu.Show();
                }

                if (selected == "logout")
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
