using ExotischNederland.DAL;
using ExotischNederland.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExotischNederland.Models
{
    internal class User
    {
        readonly string tablename = "User";
        public int Id { get; private set; }  // Add the User ID property with a private setter
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<Observation> Observations { get
            {
                return this.GetObservations();
            }
            set
            {
                this.Observations = value;
            }
        }
        public List<Route> Routes { get; set; }
        public List<Role> Roles { get; set; }
        public Permission Permission { get; private set; }
        public float CurrentLatitude { get; set; } = 0.0f;
        public float CurrentLongitude { get; set; } = 0.0f;

        // Constructor om User objecten te initialiseren vanuit een Dictionary<string, object>
        public User(Dictionary<string, object> _values)
        {
            this.Id = (int)_values["Id"];
            this.Name = (string)_values["Name"];
            this.Email = (string)_values["Email"];
            this.PasswordHash = (string)_values["PasswordHash"];
            this.Routes = new List<Route>();
            this.Roles = this.GetRoles();
            this.Permission = new Permission(this);

            // Hardcode eventueel de coördinaten als tijdelijke oplossing, totdat GPS is geïmplementeerd
            this.CurrentLatitude = 52.0f;
            this.CurrentLongitude = 4.0f;
        }

        public static User Authenticate(string _email, string _password)
        {
            if (_email == null || _password == null) return null;

            SQLDAL sql = SQLDAL.Instance;
            User user = sql.Find<User>("email", _email);

            if (user == null) return null;
            if (user.PasswordHash == Helpers.HashPassword(_password))
            {
                return user;  // Return the authenticated User object
            }

            return null;  // Return null if authentication fails
        }

        public static User Create(string _name, string _email, string _password)
        {
            SQLDAL sql = SQLDAL.Instance;
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "Name", _name },
                { "Email", _email },
                { "PasswordHash", Helpers.HashPassword(_password) }
            };

            int id = sql.Insert("User", values);
            return Find(id);
        }

        public List<Observation> GetObservations()
        {
            SQLDAL sql = SQLDAL.Instance;
            return sql.Select<Observation>(qb => qb.Where("UserId", "=", this.Id));
        }

        public static User Find(int id)
        {
            SQLDAL sql = SQLDAL.Instance;
            return sql.Find<User>("Id", id.ToString());
        }

        public void AssignRole(Role _role)
        {
            // Check if Roles contains a Role with the same ID
            if (this.Roles.Any(r => r.Id == _role.Id)) return;

            SQLDAL sql = SQLDAL.Instance;
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "UserId", this.Id },
                { "RoleId", _role.Id }
            };

            sql.Insert("UserRole", values);
            this.Roles.Add(_role);
        }

        public void RemoveRole(Role _role)
        {
            // Check if Roles contains a Role with the same ID
            if (!this.Roles.Any(r => r.Id == _role.Id)) return;

            SQLDAL sql = SQLDAL.Instance;
            sql.Delete("UserRole", qb => qb
                .Where("UserId", "=", this.Id)
                .Where("RoleId", "=", _role.Id)
            );

            this.Roles = this.Roles.Where(r => r.Id != _role.Id).ToList();
        }

        public void SyncRoles(List<Role> _roles, User _authenticatedUser)
        {
            if (!_authenticatedUser.Permission.CanEditUser(this)) return;

            foreach (Role _role in this.Roles)
            {
                if (!_roles.Any(r => r.Id == _role.Id))
                {
                    this.RemoveRole(_role);
                }
            }
            foreach (Role _role in _roles)
            {
                if (!this.Roles.Any(r => r.Id == _role.Id))
                {
                    this.AssignRole(_role);
                }
            }
        }

        private List<Role> GetRoles()
        {
            SQLDAL sql = SQLDAL.Instance;
            return sql.Select<Role>(qb => qb
                .Columns("Role.*")
                .Join("UserRole", "Role.Id", "UserRole.RoleId")
                .Where("UserRole.UserId", "=", this.Id)
            );
        }

        public static List<User> GetAll()
        {
            SQLDAL sql = SQLDAL.Instance;
            return sql.Select<User>();
        }

        public void Update(User _authenticatedUser)
        {
            if (!_authenticatedUser.Permission.CanEditUser(this)) return;

            SQLDAL sql = SQLDAL.Instance;
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "Name", this.Name },
                { "Email", this.Email },
                { "PasswordHash", this.PasswordHash }
            };

            sql.Update("User", this.Id, values);
        }

        public void Delete(User _authenticatedUser)
        {
            if (!_authenticatedUser.Permission.CanDeleteUser(this)) return;

            SQLDAL sql = SQLDAL.Instance;
            sql.Delete("User", this.Id);
        }
    }
}