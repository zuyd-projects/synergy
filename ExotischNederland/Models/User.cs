using ExotischNederland.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.Models
{
    internal class User
    {
        private string tablename = "User";
        private string name;
        private string email;
        private string passwordHash;
        private List<Observation> observations;
        private List<Route> routes;
        private List<Role> roles;

        public User(Dictionary<string, object> values)
        {
            this.name = (string)values["Name"];
            this.email = (string)values["Email"];
            this.passwordHash = (string)values["PasswordHash"];
            observations = new List<Observation>();
            routes = new List<Route>();
            roles = new List<Role>();
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public List<Observation> GetObservations()
        {
            return observations;
        }

        public List<Route> GetRoutes()
        {
            return routes;
        }

        public List<Role> GetRoles()
        {
            return roles;
        }

        public static bool Authenticate(string _email, string _password)
        {
            // Hash the password and compare it to the stored hash
            if (_email == null || _password == null) return false;

            SQLDAL sql = new SQLDAL();
            User user = sql.Find<User>("email", _email);

            if (user == null) return false;
            Console.WriteLine(user.passwordHash + " " + Helpers.HashPassword(_password));
            return user.passwordHash == Helpers.HashPassword(_password);
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

            // TODO: Implement the following methods
            // + Create(string name, string email, string passwordHash)
            // + Update(int id, string name, string email, string
            // passwordHash)
            // + Delete(int id)
            // + AssignRole(int userId, int roleId)
            // + RemoveRole(int userId, int roleId)

    }
}
