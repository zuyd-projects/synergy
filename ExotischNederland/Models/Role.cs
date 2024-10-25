using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExotischNederland.DAL;

namespace ExotischNederland.Models
{
    internal class Role: Model
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        private List<User> Users { get; set; }

        public Role(Dictionary<string, object> _values)
        {
            this.Id = (int)_values["Id"];
            this.Name = (string)_values["RoleName"];
            this.Description = (string)CastNullable(_values["Description"]);
            this.Users = new List<User>();
        }

        public static Role Create(string _name, string _description)
        {
            SQLDAL sql = new SQLDAL();
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "RoleName", _name },
                { "Description", _description }
            };

            sql.Insert("Role", values);
            return Find(_name);
        }

        public static Role Find(string _name)
        {
            SQLDAL sql = new SQLDAL();
            return sql.Select<Role>(qb => qb.Where("RoleName", "=", _name)).First();
        }
    }
}
