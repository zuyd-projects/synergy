using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExotischNederland.DAL;
using ExotischNederland.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class RouteTest
    {
        private SQLDAL db;
        private User testUser;

        [TestInitialize]
        public void Setup()
        {
            db = SQLDAL.Instance;
                        
            testUser = User.Create("Test User", "test@test.com", "password");
        }

        [TestMethod]
        public void TestRouteCanBeCreated()
        {
            string name = "Test Route";
            string description = "A scenic route for testing.";
            int areaId = 1; // Assuming an area with ID 1 exists

            Route route = Route.Create(name, description, areaId, testUser);

            Assert.IsInstanceOfType(route, typeof(Route));
            Assert.AreEqual(name, route.Name);
            Assert.AreEqual(description, route.Description);
            Assert.AreEqual(testUser.Id, route.User.Id);
        }

        [TestMethod]
        public void TestRouteCanBeUpdated()
        {
            string name = "Original Route";
            string description = "Original description.";
            int areaId = 1; // Assuming an area with ID 1 exists

            // Create the route with original values
            Route route = Route.Create(name, description, areaId, testUser);

            // New values for update
            string newName = "Updated Route";
            string newDescription = "Updated description.";

            // Update the route
            route.Update(newName, newDescription);
            Route updatedRoute = Route.Find(route.Id);

            // Assertions to check the update was successful
            Assert.AreEqual(newName, updatedRoute.Name);
            Assert.AreEqual(newDescription, updatedRoute.Description);
        }

        [TestMethod]
        public void TestRouteCanBeDeleted()
        {
            string name = "Route to Delete";
            string description = "This route will be deleted.";
            int areaId = 1; // Assuming an area with ID 1 exists

            // Create the route
            Route route = Route.Create(name, description, areaId, testUser);
            int routeId = route.Id;

            // Delete the route and verify deletion
            Route.Delete(routeId);
            Route deletedRoute = Route.Find(routeId);

            Assert.IsNull(deletedRoute);
        }

        [TestMethod]
        public void TestRoutePointsCanBeAddedAndRemoved()
        {
            string name = "Route with Points";
            string description = "Route for testing points.";
            int areaId = 1; // Assuming an area with ID 1 exists

            // Create the route
            Route route = Route.Create(name, description, areaId, testUser);

            // Create a PointOfInterest for the RoutePoints
            PointOfInterest poi = PointOfInterest.Create("Test POI", "Description", 1.0, 1.0);

            // Add route points
            var routePoints = new List<RoutePoint>
            {
                RoutePoint.Create(route, 1, poi),
                RoutePoint.Create(route, 2, poi)
            };

            foreach (var point in routePoints)
            {
                route.AddRoutePoint(point);
            }

            // Verify points were added
            List<RoutePoint> points = route.Points;
            Assert.AreEqual(2, points.Count);

            // Remove a route point
            route.RemoveRoutePoint(points[0].Id);

            // Verify point was removed
            points = route.Points;
            Assert.AreEqual(1, points.Count);
        }
    }

    
}

