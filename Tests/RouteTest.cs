using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExotischNederland.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class RouteTest : TransactionTest
    {
        //    [TestMethod]
        //    public void TestRouteCanBeCreated()
        //    {
        //        string name = "Test Route";
        //        string description = "A scenic route for testing.";
        //        int userId = 1; // Assuming a user with ID 1 exists
        //        int areaId = 1; // Assuming an area with ID 1 exists

        //        Route route = Route.Create(name, description, userId, areaId);

        //        Assert.IsInstanceOfType(route, typeof(Route));
        //        Assert.AreEqual(name, route.Name);
        //        Assert.AreEqual(description, route.Description);
        //        Assert.AreEqual(userId, route.UserId);
        //        Assert.AreEqual(areaId, route.AreaId);
        //    }

        //    [TestMethod]
        //    public void TestRouteCanBeUpdated()
        //    {
        //        string name = "Original Route";
        //        string description = "Original description.";
        //        int userId = 1; // Assuming a user with ID 1 exists
        //        int areaId = 1; // Assuming an area with ID 1 exists

        //        // Create the route with original values
        //        Route route = Route.Create(name, description, userId, areaId);

        //        // New values for update
        //        string newName = "Updated Route";
        //        string newDescription = "Updated description.";
        //        int newUserId = 2; // Assuming a user with ID 2 exists
        //        int newAreaId = 2; // Assuming an area with ID 2 exists

        //        // Update the route
        //        Route.Update(route.Id, newName, newDescription, newUserId, newAreaId);
        //        Route updatedRoute = Route.Find(route.Id);

        //        // Assertions to check the update was successful
        //        Assert.AreEqual(newName, updatedRoute.Name);
        //        Assert.AreEqual(newDescription, updatedRoute.Description);
        //        Assert.AreEqual(newUserId, updatedRoute.UserId);
        //        Assert.AreEqual(newAreaId, updatedRoute.AreaId);
        //    }

        //    [TestMethod]
        //    public void TestRouteCanBeDeleted()
        //    {
        //        string name = "Route to Delete";
        //        string description = "This route will be deleted.";
        //        int userId = 1; // Assuming a user with ID 1 exists
        //        int areaId = 1; // Assuming an area with ID 1 exists

        //        // Create the route
        //        Route route = Route.Create(name, description, userId, areaId);
        //        int routeId = route.Id;

        //        // Delete the route and verify deletion
        //        Route.Delete(routeId);
        //        Route deletedRoute = Route.Find(routeId);

        //        Assert.IsNull(deletedRoute);
        //    }

        //    [TestMethod]
        //    public void TestRoutePointsCanBeAddedAndRemoved()
        //    {
        //        string name = "Route with Points";
        //        string description = "Route for testing points.";
        //        int userId = 1; // Assuming a user with ID 1 exists
        //        int areaId = 1; // Assuming an area with ID 1 exists

        //        // Create the route
        //        Route route = Route.Create(name, description, userId, areaId);

        //        // Add route points
        //        var routePoints = new List<RoutePoint>
        //        {
        //            new RoutePoint { Latitude = 1.0, Longitude = 1.0 },
        //            new RoutePoint { Latitude = 2.0, Longitude = 2.0 }
        //        };

        //        foreach (var point in routePoints)
        //        {
        //            route.AddRoutePoint(point);
        //        }

        //        // Verify points were added
        //        List<RoutePoint> points = route.GetRoutePoints();
        //        Assert.AreEqual(2, points.Count);

        //        // Remove a route point
        //        route.RemoveRoutePoint(points[0].Id);

        //        // Verify point was removed
        //        points = route.GetRoutePoints();
        //        Assert.AreEqual(1, points.Count);
        //    }
    }
}
