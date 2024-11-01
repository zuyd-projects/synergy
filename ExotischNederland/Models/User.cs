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
        public List<Observation> Observations { get; set; }
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
            this.Observations = new List<Observation>();
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

            SQLDAL sql = new SQLDAL();
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
            SQLDAL sql = new SQLDAL();
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
            SQLDAL sql = new SQLDAL();
            return sql.Select<Observation>(qb => qb.Where("UserId", "=", this.Id));
        }

        public static User Find(int id)
        {
            SQLDAL sql = new SQLDAL();
            return sql.Find<User>("Id", id.ToString());
        }

        public void AssignRole(Role _role)
        {
            // Check if Roles contains a Role with the same ID
            if (this.Roles.Any(r => r.Id == _role.Id)) return;

            SQLDAL sql = new SQLDAL();
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

            SQLDAL sql = new SQLDAL();
            sql.Delete("UserRole", qb => qb
                .Where("UserId", "=", this.Id)
                .Where("RoleId", "=", _role.Id)
            );

            this.Roles = this.Roles.Where(r => r.Id != _role.Id).ToList();
        }

        private List<Role> GetRoles()
        {
            SQLDAL sql = new SQLDAL();
            return sql.Select<Role>(qb => qb
                .Columns("Role.*")
                .Join("UserRole", "Role.Id", "UserRole.RoleId")
                .Where("UserRole.UserId", "=", this.Id)
            );
        }

        // TODO: Implement the following methods
        // + Update(int id, string name, string email, string passwordHash)
        // + Delete(int id)
    }
}