using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExotischNederland.Models;

namespace ExotischNederland.Menus
{
    internal class UserMenu: IMenu
    {
        private readonly User authenticatedUser;
        private Dictionary<string, string> menuItems;

        public UserMenu(User authenticatedUser)
        {
            this.authenticatedUser = authenticatedUser;
        }

        public Dictionary<string, string> GetMenuItems()
        {
            Dictionary<string, string> menuItems = new Dictionary<string, string>();
            if (this.authenticatedUser.Permission.CanViewAllUsers()) menuItems.Add("viewAll", "Bekijk alle gebruikers");
            menuItems.Add("create", "Maak nieuwe gebruiker aan");
            menuItems.Add("back", "Terug naar menu");
            return menuItems;
        }

        public void Show()
        {
            while (true)
            {
                menuItems = this.GetMenuItems();
                string selected = Helpers.MenuSelect(this.menuItems, true);
                if (selected == "back" || selected is null) return;

                if (selected == "viewAll") ViewUsers();
                if (selected == "create") CreateUser();
            }
        }

        private void ViewUsers()
        {
            List<User> users = User.GetAll();
            Console.Clear();
            Console.WriteLine("Gebruikers:");

            if (users.Count > 0)
            {
                Dictionary<string, string> options = users.ToDictionary(x => x.Id.ToString(), x => $"{x.Id}. {x.Name} ({x.Email})");
                options.Add("back", "Ga terug");
                string selectedUser = Helpers.MenuSelect(options, false);
                if (selectedUser == "back" || selectedUser is null) return;
                
                User user = users.Find(x => x.Id == int.Parse(selectedUser));
                this.ViewUser(user);
            }
            else
            {
                Console.WriteLine("Geen gebruikers gevonden");
                Console.ReadKey();
            }
            ViewUsers();
        }

        private void ViewUser(User _user)
        {
            Console.Clear();
            string userRoles = string.Join(", ", _user.Roles.Select(x => x.Name));
            List<string> text = new List<string>();
            text.Add("Gebruiker details:");
            text.Add($"Naam: {_user.Name}");
            text.Add($"Email: {_user.Email}");
            text.Add($"Rollen: {userRoles}");
            text.Add($"Observaties: {_user.Observations.Count}");

            Dictionary<string, string> menu = new Dictionary<string, string>();
            if (this.authenticatedUser.Permission.CanEditUser(_user)) menu.Add("edit", "Gebruiker bewerken");
            if (this.authenticatedUser.Permission.CanDeleteUser(_user)) menu.Add("delete", "Gebruiker verwijderen");
            menu.Add("back", "Terug naar menu");
            string selected = Helpers.MenuSelect(menu, true, text);

            if (selected == "edit") EditUser(_user);
            if (selected == "delete") DeleteUser(_user);
        }

        private void EditUser(User _user)
        {
            Console.Clear();
            List<Role> roles = Role.GetAll();
            Dictionary<string, string> roleOptions = roles.ToDictionary(x => x.Id.ToString(), x => x.Name);
            string userRoles = string.Join(",", _user.Roles.Select(x => x.Id));
            List<FormField> fields = new List<FormField>();
            fields.Add(new FormField("name", "Voer een nieuwe naam in", "string", true, _user.Name));
            fields.Add(new FormField("email", "Voer een nieuw e-mailadres in", "string", true, _user.Email));
            fields.Add(new FormField("password", "Voer een nieuw wachtwoord in", "password", true, _user.PasswordHash));
            fields.Add(new FormField("roles", "Gebruikersrollen", "multi_select", false, userRoles, roleOptions));

            Dictionary<string, object> values = new Form(fields).Prompt();
            if (values == null)
            {
                ViewUser(_user);
                return;
            }

            _user.Name = (string)values["name"];
            _user.Email = (string)values["email"];
            _user.PasswordHash = (string)values["password"];

            _user.Update(this.authenticatedUser);
            List<string> selectedRoleIds = values["roles"].ToString().Split(',').ToList();
            List<Role> selectedRoles = roles.Where(x => selectedRoleIds.Contains(x.Id.ToString())).ToList();
            _user.SyncRoles(selectedRoles, this.authenticatedUser);

            ViewUser(_user);
        }

        private void DeleteUser(User _user)
        {
            Console.Clear();

            Console.WriteLine($"Weet u zeker dat u gebruiker {_user.Id} wilt verwijderen? [J/N]");
            if (!Helpers.ConfirmPrompt()) return;
            if (_user.Observations.Count > 0)
            {
                Console.WriteLine("De gebruiker heeft observaties. Als u deze gebruiker verwijderd worden deze observaties aan u gekoppeld.");
                Console.WriteLine("Wilt u doorgaan? [J/N]");
                if (!Helpers.ConfirmPrompt()) return;
            }

            _user.Delete(this.authenticatedUser);
        }

        private void CreateUser()
        {
            Console.Clear();
            List<Role> roles = Role.GetAll();
            Dictionary<string, string> roleOptions = roles.ToDictionary(x => x.Id.ToString(), x => x.Name);
            List<FormField> fields = new List<FormField>();
            fields.Add(new FormField("name", "Naam", "string", true));
            fields.Add(new FormField("email", "Email", "string", true));
            fields.Add(new FormField("password", "Wachtwoord", "password", true));
            fields.Add(new FormField("roles", "Gebruikersrollen", "multi_select", false, null, roleOptions));

            Dictionary<string, object> values = new Form(fields).Prompt();


            User createdUser = User.Create((string)values["name"], (string)values["email"], (string)values["password"]);
            
            if (createdUser == null)
            {
                Console.WriteLine("Gebruiker kon niet worden aangemaakt");
                Console.ReadKey();
                return;
            }
            List<string> selectedRoleIds = values["roles"].ToString().Split(',').ToList();
            List<Role> selectedRoles = roles.Where(x => selectedRoleIds.Contains(x.Id.ToString())).ToList();
            createdUser.SyncRoles(selectedRoles, this.authenticatedUser);

            ViewUser(createdUser);
        }
    }
}
