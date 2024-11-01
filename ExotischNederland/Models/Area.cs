using ExotischNederland.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExotischNederland.Models
{
    internal class Area
    {
        readonly string tablename = "Area";
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }

        private string PolygonPoints { get; set; }  // Database field for the polygon string

        // Property to handle coordinates in the application
        public List<(double lat, double lng)> PolygonCoordinates
        {
            get => ParsePolygonPoints(PolygonPoints);
            set => PolygonPoints = SerializePolygonPoints(value);
        }

        public List<Route> Routes { get; set; }

        public Area(Dictionary<string, object> _values)
        {
            this.Id = (int)_values["Id"];
            this.Name = (string)_values["Name"];
            this.Description = (string)_values["Description"];
            this.PolygonPoints = (string)_values["PolygonPoints"];
            this.Routes = this.GetRoutes();
        }

        // Static method for creating an area
        public static Area Create(string name, string description, List<(double lat, double lng)> polygonCoordinates)
        {
            SQLDAL sql = SQLDAL.Instance;
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "Name", name },
                { "Description", description },
                { "PolygonPoints", SerializePolygonPoints(polygonCoordinates) }
            };

            int id = sql.Insert("Area", values);
            return Find(id);
        }

        public static Area Find(int id)
        {
            SQLDAL sql = SQLDAL.Instance;
            return sql.Find<Area>("Id", id.ToString());
        }

        public static void Update(int id, string name, string description, List<(double lat, double lng)> polygonCoordinates)
        {
            SQLDAL sql = SQLDAL.Instance;
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "Name", name },
                { "Description", description },
                { "PolygonPoints", SerializePolygonPoints(polygonCoordinates) }
            };

            sql.Update("Area", id, values);
        }

        public static void Delete(int id)
        {
            SQLDAL sql = SQLDAL.Instance;
            sql.Delete("Area", id);
        }

        public List<Route> GetRoutes()
        {
            SQLDAL sql = SQLDAL.Instance;
            return sql.Select<Route>(qb => qb
                .Where("AreaId", "=", this.Id));
        }

        public List<Observation> GetObservations()
        {
            SQLDAL sql = SQLDAL.Instance;
            return sql.Select<Observation>(qb => qb
                .Join("Route", "Observation.RouteId", "Route.Id")
                .Where("Route.AreaId", "=", this.Id));
        }

        public static List<Area> ListAreas()
        {
            SQLDAL sql = SQLDAL.Instance;
            return sql.Select<Area>();
        }

        // Helper methods for parsing and serializing polygon points
        public static List<(double lat, double lng)> ParsePolygonPoints(string polygonPoints)
        {
            try
            {
                return polygonPoints
                    .Split(',')
                    .Select(point => point.Trim().Replace("POINT(", "").Replace(")", "").Split(' '))
                    .Select(coords => (lat: double.Parse(coords[0]), lng: double.Parse(coords[1])))
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing PolygonPoints: {ex.Message}");
                return new List<(double lat, double lng)>();
            }
        }
        
        public static string SerializePolygonPoints(List<(double lat, double lng)> polygonCoordinates)
        {
            return string.Join(", ", polygonCoordinates.Select(coord => $"POINT({coord.lat} {coord.lng})"));
        }
    }
}