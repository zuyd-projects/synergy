using ExotischNederland.DAL;
using System;
using System.Collections.Generic;

namespace ExotischNederland.Models
{
    internal class Area
    {
        readonly string tablename = "Area";
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PolygonPoints { get; set; }
        public List<Route> Routes { get; set; }

        public Area(Dictionary<string, object> _values)
        {
            this.Id = (int)_values["Id"];
            this.Name = (string)_values["Name"];
            this.Description = (string)_values["Description"];
            this.PolygonPoints = (string)_values["PolygonPoints"];
            this.Routes = this.GetRoutes();
        }

        public static Area Create(string name, string description, string polygonPoints)
        {
            SQLDAL sql = new SQLDAL();
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "Name", name },
                { "Description", description },
                { "PolygonPoints", polygonPoints }
            };

            int id = sql.Insert("Area", values);
            return Find(id);
        }

        public static Area Find(int id)
        {
            SQLDAL sql = new SQLDAL();
            return sql.Find<Area>("Id", id.ToString());
        }

        public static void Update(int id, string name, string description, string polygonPoints)
        {
            SQLDAL sql = new SQLDAL();
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "Name", name },
                { "Description", description },
                { "PolygonPoints", polygonPoints }
            };

            sql.Update("Area", id, values);
        }

        public static void Delete(int id)
        {
            SQLDAL sql = new SQLDAL();
            sql.Delete("Area", id);
        }

        public List<Route> GetRoutes()
        {
            SQLDAL sql = new SQLDAL();
            return sql.Select<Route>(qb => qb
                .Where("AreaId", "=", this.Id));
        }

        public List<Observation> GetObservations()
        {
            SQLDAL sql = new SQLDAL();
            return sql.Select<Observation>(qb => qb
                .Join("Route", "Observation.RouteId", "Route.Id")
                .Where("Route.AreaId", "=", this.Id));
        }

        public static List<Area> ListAreas()
        {
            SQLDAL sql = new SQLDAL();
            return sql.Select<Area>();
        }
    }
}