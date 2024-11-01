using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ExotischNederland.Models;

namespace ExotischNederland.Menus
{
    internal class UserMenu
    {
        private User authenticatedUser;
        Dictionary<string, string> menuItems = new Dictionary<string, string>();

        public UserMenu(User _authenticatedUser) 
        {
            this.authenticatedUser = _authenticatedUser;
            // Logic to add menu items
            if (_authenticatedUser.Permission.CanViewAllObservations() || _authenticatedUser.Permission.CanCreateObservation()) menuItems.Add("observations", "Observaties");
            if (_authenticatedUser.Permission.CanViewAllAreas()) menuItems.Add("areas", "Gebieden");
            menuItems.Add("logout", "Uitloggen");
        }

        public void Show()
        {
            while (true)
            {
                string selected = Helpers.MenuSelect(this.menuItems, true);

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
