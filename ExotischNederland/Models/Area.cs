using ExotischNederland.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ExotischNederland.Models
{
    internal class Area
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PolygonPoints { get; set; }

        // Property to handle coordinates in the application
        public List<(double lat, double lng)> PolygonCoordinates
        {
            get => ParsePolygonPoints(PolygonPoints);
            set { PolygonPoints = SerializePolygonPoints(value); }
        }

        public List<Route> Routes
        {
            get
            { return this.GetRoutes(); }
        }

        public Area(Dictionary<string, object> _values)
        {
            this.Id = (int)_values["Id"];
            this.Name = (string)_values["Name"];
            this.Description = (string)_values["Description"];
            this.PolygonPoints = (string)_values["PolygonPoints"];
        }

        // Static method for creating an area
        public static Area Create(string _name, string _description, string _polygonCoordinates, User _authenticatedUser)
        {
            if (!_authenticatedUser.Permission.CanCreateArea()) return null;
            SQLDAL sql = SQLDAL.Instance;
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "Name", _name },
                { "Description", _description },
                { "PolygonPoints", _polygonCoordinates }
            };

            int id = sql.Insert("Area", values);
            values["Id"] = id; // Add the generated Id to the values dictionary
            return new Area(values);
        }

        public static Area Find(int _id)
        {
            SQLDAL sql = SQLDAL.Instance;
            return sql.Find<Area>("Id", _id.ToString());
        }

        public static List<Area> GetAll()
        {
            SQLDAL sql = SQLDAL.Instance;
            return sql.Select<Area>();
        }

        public void Update(User _authenticatedUser)
        {
            if(!_authenticatedUser.Permission.CanEditArea()) return;

            SQLDAL sql = SQLDAL.Instance;
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "Name", this.Name },
                { "Description", this.Description },
                { "PolygonPoints", SerializePolygonPoints(this.PolygonCoordinates) }
            };

            sql.Update("Area", this.Id, values);
        }

        public void Delete(User _authenticatedUser)
        {
            if(!_authenticatedUser.Permission.CanDeleteArea()) return;

            SQLDAL sql = SQLDAL.Instance;
            sql.Delete("Area", this.Id);
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
            catch (Exception)
            {
                Console.WriteLine("Error parsing PolygonPoints");
                return new List<(double lat, double lng)>();
            }
        }
        
        //Method for caluclating centoid of polygon
        public (double lat, double lng) CalculateCentroid()
        {
            double latSum = 0, lngSum = 0;
            int numPoints = PolygonCoordinates.Count;

            foreach (var point in PolygonCoordinates)
            {
                latSum += point.lat;
                lngSum += point.lng;
            }

            return (latSum / numPoints, lngSum / numPoints);
        }
        
        public static string SerializePolygonPoints(List<(double lat, double lng)> polygonCoordinates)
        {
            return string.Join(", ", polygonCoordinates.Select(coord => $"POINT({coord.lat} {coord.lng})"));
        }
    }
}