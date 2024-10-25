using ExotischNederland.DAL;
using ExotischNederland.Models;
using System;
using System.Collections.Generic;
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

        public User(Dictionary<string, object> values)
        {
            this.Id = (int)values["Id"];  // Initialize the User ID
            this.Name = (string)values["Name"];
            this.Email = (string)values["Email"];
            this.PasswordHash = (string)values["PasswordHash"];
            this.Observations = new List<Observation>();
            this.Routes = new List<Route>();
            this.Roles = new List<Role>();
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
            return sql.Select<Observation>("Observation", qb => qb.Where("UserId", "=", this.Id));
        }

        public static User Find(int id)
        {
            SQLDAL sql = new SQLDAL();
            return sql.Find<User>("Id", id.ToString());
        }

        // TODO: Implement the following methods
        // + Update(int id, string name, string email, string passwordHash)
        // + Delete(int id)
        // + AssignRole(int userId, int roleId)
        // + RemoveRole(int userId, int roleId)
    }
}