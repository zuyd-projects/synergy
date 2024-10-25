using ExotischNederland.DAL;
using ExotischNederland.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.Models
{
    internal class User
    {
        private int id;  // Add the User ID property
        private string tablename = "User";
        private string name;
        private string email;
        private string passwordHash;
        private List<Observation> observations;
        private List<Route> routes;
        private List<Role> roles;

        public User(Dictionary<string, object> values)
        {
            this.id = (int)values["Id"];  // Initialize the User ID
            this.name = (string)values["Name"];
            this.email = (string)values["Email"];
            this.passwordHash = (string)values["PasswordHash"];
            observations = new List<Observation>();
            routes = new List<Route>();
            roles = new List<Role>();
        }

        public int Id => id;  // Add the User ID property
        public string Name => name;
        public string Email => email;
        public string PasswordHash => passwordHash;
        public List<Observation> Observations => observations;
        public List<Route> Routes => routes;
        public List<Role> Roles => roles;

        public static User Authenticate(string _email, string _password)
        {
            if (_email == null || _password == null) return null;

            SQLDAL sql = new SQLDAL();
            User user = sql.Find<User>("email", _email);

            if (user == null) return null;
            if (user.passwordHash == Helpers.HashPassword(_password))
            {
                return user;  // Return the authenticated User object
            }

            return null;  // Return null if authentication fails
        }

        public static void Create(string _name, string _email, string _password)
        {
            SQLDAL sql = new SQLDAL();
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "name", _name },
                { "email", _email },
                { "passwordHash", Helpers.HashPassword(_password) }
            };

            int id = sql.Insert("User", values);
        }

        public List<Observation> GetObservations()
        {
            SQLDAL sql = new SQLDAL();
            return sql.Select<Observation>("Observation").Where(o => o.User.id == this.Id).ToList();

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