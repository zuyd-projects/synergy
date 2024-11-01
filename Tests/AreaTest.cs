using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;
using ExotischNederland.Models;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class AreaTest : TransactionTest
    {
        [TestMethod]
        public void TestAreaCanBeCreatedByBeheerder()
        {
            User user = User.Create("TestUser", "test@test.com", "password");
            user.AssignRole(Role.Create("Beheerder"));
            string name = "Test Area";
            string description = "A beautiful natural area for testing.";
            var polygonPoints = "POINT(50.93197114526339 5.968108426221363), POINT(50.936514832961535 5.990767727979716), POINT(50.92169211871979 6.00278402436561), POINT(50.90967901024405 5.974459897169027), POINT(50.91833741853662 5.931544552930347), POINT(50.93197114526339 5.968108426221363)";

            Area area = Area.Create(name, description, polygonPoints, user);

            Assert.IsInstanceOfType(area, typeof(Area));
            Assert.AreEqual(name, area.Name);
            Assert.AreEqual(description, area.Description);
            Assert.AreEqual(polygonPoints, area.PolygonPoints);
            Assert.IsNotNull(area.PolygonCoordinates);
        }

        [TestMethod]
        public void TestAreaCannotBeCreatedByNonBeheerder()
        {
            User user = User.Create("TestUser", "test@test.com", "password");
            string name = "Test Area";
            string description = "A beautiful natural area for testing.";
            var polygonPoints = "POINT(50.93197114526339 5.968108426221363), POINT(50.936514832961535 5.990767727979716), POINT(50.92169211871979 6.00278402436561), POINT(50.90967901024405 5.974459897169027), POINT(50.91833741853662 5.931544552930347), POINT(50.93197114526339 5.968108426221363)";

            Area area = Area.Create(name, description, polygonPoints, user);

            Assert.IsNull(area);
        }

        [TestMethod]
        public void TestAreaCanBeUpdated()
        {
            User user = User.Create("TestUser", "test@test.com", "password");
            user.AssignRole(Role.Create("Beheerder"));
            string name = "Original Area";
            string description = "Original description.";
            var originalPolygonCoordinates = "POINT(50.93197114526339 5.968108426221363), POINT(50.936514832961535 5.990767727979716), POINT(50.92169211871979 6.00278402436561), POINT(50.90967901024405 5.974459897169027), POINT(50.91833741853662 5.931544552930347), POINT(50.93197114526339 5.968108426221363)";

            // Create the area with original values
            Area area = Area.Create(name, description, originalPolygonCoordinates, user);

            // New values for update
            string newName = "Updated Area";
            string newDescription = "Updated description.";
            var newPolygonCoordinates = new List<(double lat, double lng)>
            {
                (4.0, 4.0),
                (5.0, 5.0)
            };
            area.Name = newName;
            area.Description = newDescription;
            area.PolygonCoordinates = newPolygonCoordinates;

            // Update the area
            area.Update(user);
            Area updatedArea = Area.Find(area.Id);

            // Assertions to check the update was successful
            Assert.AreEqual(newName, updatedArea.Name);
            Assert.AreEqual(newDescription, updatedArea.Description);
            CollectionAssert.AreEqual(newPolygonCoordinates, updatedArea.PolygonCoordinates);
        }

        [TestMethod]
        public void TestAreaCanBeDeletedByBeheerder()
        {
            User user = User.Create("TestUser", "test@test.com", "password");
            user.AssignRole(Role.Create("Beheerder"));
            string name = "Area to Delete";
            string description = "This area will be deleted.";
            var polygonPoints = "POINT(50.93197114526339 5.968108426221363), POINT(50.936514832961535 5.990767727979716), POINT(50.92169211871979 6.00278402436561), POINT(50.90967901024405 5.974459897169027), POINT(50.91833741853662 5.931544552930347), POINT(50.93197114526339 5.968108426221363)";

            // Create the area with the list of coordinates
            Area area = Area.Create(name, description, polygonPoints, user);

            // Delete the area and verify deletion
            area.Delete(user);
            Area deletedArea = Area.Find(area.Id);

            Assert.IsNull(deletedArea);
        }

        // The tests below are examples for how area's can be tested when connected to routes,
        // so the routes class should first be made before we can add those

        // [TestMethod]
        // public void TestGetRoutesForArea()
        // {
        //     string name = "Route Test Area";
        //     string description = "Area with routes for testing.";
        //     string polygonPoints = "POINT(1 1), POINT(2 2)";
        //     
        //     Area area = Area.Create(name, description, polygonPoints);
        //
        //     // Assuming Route.Create sets AreaId when creating routes for an area
        //     Route route1 = Route.Create("Route 1", "First route", area.Id);
        //     Route route2 = Route.Create("Route 2", "Second route", area.Id);
        //
        //     List<Route> routes = area.GetRoutes();
        //
        //     Assert.AreEqual(2, routes.Count);
        //     Assert.IsTrue(routes.Exists(r => r.Name == "Route 1"));
        //     Assert.IsTrue(routes.Exists(r => r.Name == "Route 2"));
        // }

        // [TestMethod]
        // public void TestGetObservationsForArea()
        // {
        //     string name = "Observation Test Area";
        //     string description = "Area with observations for testing.";
        //     string polygonPoints = "POINT(1 1), POINT(2 2)";
        //     
        //     Area area = Area.Create(name, description, polygonPoints);
        //     Route route = Route.Create("Route with Observations", "Route for observations", area.Id);
        //
        //     Observation observation1 = Observation.Create(1, 1.0f, 2.0f, "Observation 1", "photoUrl1", route.Id);
        //     Observation observation2 = Observation.Create(2, 3.0f, 4.0f, "Observation 2", "photoUrl2", route.Id);
        //
        //     List<Observation> observations = area.GetObservations();
        //
        //     Assert.AreEqual(2, observations.Count);
        //     Assert.IsTrue(observations.Exists(o => o.Description == "Observation 1"));
        //     Assert.IsTrue(observations.Exists(o => o.Description == "Observation 2"));
        // }
    }
}