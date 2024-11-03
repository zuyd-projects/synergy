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

        private RoutePoint(int id, Route route, int order, PointOfInterest poi)
        {
            Id = id;
            Route = route;
            Order = order;
            PointOfInterest = poi;
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
            var values = db.Find<Dictionary<string, object>>("RoutePoint", routePointId.ToString());

            return values != null
                ? new RoutePoint(
                    (int)values["Id"],
                    Route.Find((int)values["RouteId"]),
                    (int)values["Order"],
                    PointOfInterest.Find((int)values["PointOfInterestId"])
                )
                : null;
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