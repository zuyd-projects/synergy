using ExotischNederland.DAL;
using System.Collections.Generic;

namespace ExotischNederland.Models
{
    internal class RoutePoint
    {
        public int Id { get; private set; }
        public Route Route { get; private set; }
        public int Order { get; private set; }
        public PointOfInterest PointOfInterest { get; private set; }

        private RoutePoint(Dictionary<string, object> _values)
        {
            this.Id = (int)_values["Id"];
            Route = Route.Find((int)_values["RouteId"]);
            Order = (int)_values["Order"];
            PointOfInterest = PointOfInterest.Find((int)_values["PointOfInterestId"]);
        }

        public static RoutePoint Create(Route route, int order, PointOfInterest poi)
        {
            SQLDAL db = SQLDAL.Instance;
            var values = new Dictionary<string, object>
            {
                { "RouteId", route.Id },
                { "Order", order },
                { "PointOfInterestId", poi.Id }
            };
            int id = db.Insert("RoutePoint", values);
            return Find(id);
        }

        public static RoutePoint Find(int routePointId)
        {
            SQLDAL db = SQLDAL.Instance;
            return db.Find<RoutePoint>("RoutePoint", routePointId.ToString());
        }

        public void Update(int newOrder, PointOfInterest newPoi)
        {
            Order = newOrder;
            PointOfInterest = newPoi;

            SQLDAL db = SQLDAL.Instance;
            var values = new Dictionary<string, object>
            {
                { "Order", newOrder },
                { "PointOfInterestId", newPoi.Id }
            };
            db.Update("RoutePoint", this.Id, values);
        }

        public static void Delete(int routePointId)
        {
            SQLDAL db = SQLDAL.Instance;
            db.Delete("RoutePoint", routePointId);
        }
    }
}