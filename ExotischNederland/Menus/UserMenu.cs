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
            menuItems = this.GetMenuItems();
            string selected = Helpers.MenuSelect(this.menuItems, true);

            if (selected == "viewAll")
            {
                ViewUsers(User.GetAll());
                Show();
            }
            else if (selected == "create")
            {
                CreateUser();
                Show();
            }
        }

        private void ViewUsers(List<User> _users)
        {
            Console.Clear();
            Console.WriteLine("Gebruikers:");

            if (_users.Count > 0)
            {
                Dictionary<string, string> options = _users.ToDictionary(x => x.Id.ToString(), x => $"{x.Id}. {x.Name} ({x.Email})");
                options.Add("back", "Ga terug");
                string selectedUser = Helpers.MenuSelect(options, false);
                if (selectedUser == "back")
                {
                    return;
                }
                else
                {
                    User user = _users.Find(x => x.Id == int.Parse(selectedUser));
                    this.ViewUser(user);
                }
            }
            else
            {
                Console.WriteLine("Geen gebruikers gevonden");
                Console.ReadKey();
            }
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

            Dictionary<string, string> menu = new Dictionary<string, string>();
            if (this.authenticatedUser.Permission.CanEditUser(_user)) menu.Add("edit", "Gebruiker bewerken");
            if (this.authenticatedUser.Permission.CanEditUser(_user)) menu.Add("editRoles", "Rollen bewerken");
            if (this.authenticatedUser.Permission.CanDeleteUser(_user)) menu.Add("delete", "Gebruiker verwijderen");
            menu.Add("back", "Terug naar menu");
            string selected = Helpers.MenuSelect(menu, true, text);

            if (selected == "edit")
            {
                EditUser(_user);
            }
            else if (selected == "editRoles")
            {
                EditRoles(_user);
            }
            else if (selected == "delete")
            {
                DeleteUser(_user);
            }
        }

        private void EditUser(User _user)
        {
            Console.Clear();
            List<FormField> fields = new List<FormField>();
            fields.Add(new FormField("name", "Enter new name", "string", true, _user.Name));
            fields.Add(new FormField("email", "Enter new email", "string", true, _user.Email));
            fields.Add(new FormField("password", "Enter new password", "password", true, _user.PasswordHash));

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

            Console.WriteLine("Gebruiker bijgewerkt!");
            Console.ReadKey();
        }

        private void EditRoles(User _user)
        {
            Console.Clear();
            List<Role> roles = Role.GetAll();
            Dictionary<string, string> options = roles.ToDictionary(x => x.Id.ToString(), x => x.Name);
            
            List<string> userRoles = _user.Roles.Select(x => x.Id.ToString()).ToList();

            List<string> selectedRoleIds = Helpers.MultiSelect(options, false, userRoles, new List<string> { "Selecteer gebruikersrollen"});

            List<Role> selectedRoles = roles.Where(x => selectedRoleIds.Contains(x.Id.ToString())).ToList();

            _user.SyncRoles(selectedRoles, this.authenticatedUser);
            ViewUser(_user);
        }

        private void DeleteUser(User _user)
        {
            Console.Clear();

            Console.Write($"Weet u zeker dat u gebruiker {_user.Id} wilt verwijderen? ");
            Console.WriteLine("J/N");
            ConsoleKey key = Console.ReadKey().Key;
            if (key == ConsoleKey.J)
            {
                _user.Delete(this.authenticatedUser);
                Console.WriteLine("Gebruiker verwijderd!");
                Console.ReadKey();
                return;
            }
            ViewUser(_user);
        }

        private void CreateUser()
        {
            Console.Clear();
            List<FormField> fields = new List<FormField>();
            fields.Add(new FormField("name", "Naam", "string", true));
            fields.Add(new FormField("email", "Email", "string", true));
            fields.Add(new FormField("password", "Wachtwoord", "password", true));

            Dictionary<string, object> values = new Form(fields).Prompt();


            User createdUser = User.Create((string)values["name"], (string)values["email"], (string)values["password"]);
            
            if (createdUser == null)
            {
                Console.WriteLine("Gebruiker kon niet worden aangemaakt");
                Console.ReadKey();
                return;
            }
            ViewUser(createdUser);
        }
    }
}
