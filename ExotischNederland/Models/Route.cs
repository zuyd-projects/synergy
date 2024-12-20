﻿using ExotischNederland.DAL;
using System.Collections.Generic;

namespace ExotischNederland.Models
{
    internal class Route
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public User User { get; private set; }
        public Area Area { get; private set; }
        public List<RoutePoint> Points { 
            get
            {
                return this.LoadRoutePoints();
            }
            private set
            {
                Points = value;
            }
        }

        public Route(Dictionary<string, object> _values)
        {
            this.Id = (int)_values["Id"];
            this.Name = (string)_values["Name"];
            this.Description = (string)_values["Description"];
            this.User = User.Find((int)_values["UserId"]);
            this.Area = Area.Find((int)_values["AreaId"]);
        }

        public static Route Create(string name, string description, Area _area, User user)
        {
            SQLDAL db = SQLDAL.Instance;
            var values = new Dictionary<string, object>
            {
                { "Name", name },
                { "Description", description },
                { "AreaId", _area.Id },  
                { "UserId", user.Id }
            };
            int id = db.Insert("Route", values);
            values["Id"] = id; // Add the generated Id to the values dictionary
            return new Route(values);
        }

        public static Route Find(int routeId)
        {
            SQLDAL db = SQLDAL.Instance;
            return db.Find<Route>("Id", routeId.ToString());
        }

        public static List<Route> GetAll()
        {
            SQLDAL sql = SQLDAL.Instance;
            return sql.Select<Route>();
        }

        public void Update(string newName, string newDescription)
        {
            Name = newName;
            Description = newDescription;

            SQLDAL db = SQLDAL.Instance;
            var values = new Dictionary<string, object>
            {
                { "Name", newName },
                { "Description", newDescription }
            };
            db.Update("Route", this.Id, values);
        }

        public void AddRoutePoint(RoutePoint point)
        {
            if (!Points.Exists(p => p.Id == point.Id))
            {
                Points.Add(point);
            }
        }

        public void RemoveRoutePoint(int pointId)
        {
            Points.RemoveAll(p => p.Id == pointId);
            SQLDAL db = SQLDAL.Instance;
            db.Delete("RoutePoint", qb => qb
                .Where("RouteId", "=", this.Id)
                .Where("Id", "=", pointId));
        }

        private List<RoutePoint> LoadRoutePoints()
        {
            SQLDAL db = SQLDAL.Instance;
            return db.Select<RoutePoint>(qb => qb.Where("RouteId", "=", this.Id));
        }

        public static void Delete(int routeId)
        {
            SQLDAL db = SQLDAL.Instance;
            db.Delete("Route", routeId);
        }
    }
}