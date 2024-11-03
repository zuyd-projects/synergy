using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExotischNederland.DAL;

namespace ExotischNederland.Models
{
    public class Route
    {
        public int Id { get; internal set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public List<RoutePoint> Points { get; set; }

        public Route() { }

        public Route(Dictionary<string, object> _values)
        {
            Id = (int)_values["Id"];
            Name = (string)_values["Name"];
            Description = (string)_values["Description"];
        }

        public static Route Create(string _name, string _description)
        {
            SQLDAL sql = new SQLDAL();
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "Name", _name },
                { "Description", _description }
            };

            int id = sql.Insert("Route", values);

            return Find(id);
        }

        public static Route Find(int _routeId)
        {
            SQLDAL db = new SQLDAL();
            return db.Find<Route>("Id", _routeId.ToString());
        }
    }

}
