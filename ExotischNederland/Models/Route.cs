using ExotischNederland.DAL;
using System.Collections.Generic;

namespace ExotischNederland.Models
{
    internal class Route
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public User User { get; private set; }
        public List<RoutePoint> Points { get; private set; }

        private Route(int id, string name, string description, User user)
        {
            Id = id;
            Name = name;
            Description = description;
            User = user;
            Points = LoadRoutePoints(); // Load RoutePoints from the database
        }

        public static Route Create(string name, string description, int areaId, User user)
        {
            SQLDAL db = SQLDAL.Instance;
            var values = new Dictionary<string, object>
            {
                { "Name", name },
                { "Description", description },
                { "AreaId", areaId },  
                { "UserId", user.Id }
            };
            int id = db.Insert("Route", values);
            return Find(id);
        }

        public static Route Find(int routeId)
        {
            SQLDAL db = SQLDAL.Instance;
            var values = db.Find<Dictionary<string, object>>("Route", routeId.ToString());

            return values != null
                ? new Route(
                    (int)values["Id"],
                    (string)values["Name"],
                    (string)values["Description"],
                    User.Find((int)values["UserId"])
                )
                : null;
        }

        public static List<Route> GetAllRoutes()
        {
            SQLDAL sql = SQLDAL.Instance;
            List<Route> routes = sql.Select<Route>();

            return routes;
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