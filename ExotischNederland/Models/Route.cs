using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExotischNederland.DAL;

namespace ExotischNederland.Models
{
    internal class Route
    {
        readonly string tablename = "Route";
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int AreaId { get; set; }

        public List<RoutePoint> Points { get; set; }

        public Route(Dictionary<string, object> _values)
        {
            this.Id = (int)_values["Id"];
            this.Name = (string)_values["Name"];
            this.Description = (string)_values["Description"];
            this.UserId = (int)_values["UserId"];
            this.AreaId = (int)_values["AreaId"];
            this.Points = this.GetRoutePoints();
        }
        // Static method for creating a route
        public static Route Create(string name, string description, int userId, int areaId)
        {
            SQLDAL sql = new SQLDAL();
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "Name", name },
                { "Description", description },
                { "UserId", userId },
                { "AreaId", areaId }
            };

            int id = sql.Insert("Route", values);
            return Find(id);
        }
        public static Route Find(int id)
        {
            SQLDAL sql = new SQLDAL();
            return sql.Find<Route>("Id", id.ToString());
        }

        public static void Update(int id, string name, string description, int userId, int areaId)
        {
            SQLDAL sql = new SQLDAL();
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "Name", name },
                { "Description", description },
                { "UserId", userId },
                { "AreaId", areaId }
            };

            sql.Update("Route", id, values);
        }
        public static void Delete(int id)
        {
            SQLDAL sql = new SQLDAL();
            sql.Delete("Route", id);
        }

        public List<RoutePoint> GetRoutePoints()
        {
            SQLDAL sql = new SQLDAL();
            return sql.Select<RoutePoint>(qb => qb
                .Where("RouteId", "=", this.Id));
        }
        //method to add route points
        public void AddRoutePoint(RoutePoint point)
        {
            point.RouteId = this.Id;
            point.Save();
            this.Points.Add(point);
        }
        //method to remove route points
        public void RemoveRoutePoint(int pointId)
        {
            RoutePoint point = this.Points.FirstOrDefault(p => p.Id == pointId);
            if (point != null)
            {
                point.Delete();
                this.Points.Remove(point);
            }
        }

        public static List<Route> ListRoutes()
        {
            SQLDAL sql = new SQLDAL();
            return sql.Select<Route>();
        }

        //method to generate shortest route
    }
}
