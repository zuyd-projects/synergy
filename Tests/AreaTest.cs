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
        public void TestAreaCanBeCreated()
        {
            string name = "Test Area";
            string description = "A beautiful natural area for testing.";
            var polygonCoordinates = new List<(double lat, double lng)>
    {
        (1.0, 1.0),
        (2.0, 2.0),
        (3.0, 3.0)
    };

            Area area = Area.Create(name, description, polygonCoordinates);

            Assert.IsInstanceOfType(area, typeof(Area));
            Assert.AreEqual(name, area.Name);
            Assert.AreEqual(description, area.Description);
            CollectionAssert.AreEqual(polygonCoordinates, area.PolygonCoordinates);
        }

        [TestMethod]
        public void TestAreaCanBeUpdated()
        {
            string name = "Original Area";
            string description = "Original description.";
            var originalPolygonCoordinates = new List<(double lat, double lng)>
    {
        (1.0, 1.0),
        (2.0, 2.0)
    };

            // Create the area with original values
            Area area = Area.Create(name, description, originalPolygonCoordinates);

            // New values for update
            string newName = "Updated Area";
            string newDescription = "Updated description.";
            var newPolygonCoordinates = new List<(double lat, double lng)>
    {
        (4.0, 4.0),
        (5.0, 5.0)
    };

            // Update the area
            Area.Update(area.Id, newName, newDescription, newPolygonCoordinates);
            Area updatedArea = Area.Find(area.Id);

            // Assertions to check the update was successful
            Assert.AreEqual(newName, updatedArea.Name);
            Assert.AreEqual(newDescription, updatedArea.Description);
            CollectionAssert.AreEqual(newPolygonCoordinates, updatedArea.PolygonCoordinates);
        }

        [TestMethod]
        public void TestPolygonPointsParsingAndSerialization()
        {
            var polygonCoordinates = new List<(double lat, double lng)>
    {
        (1.0, 1.0),
        (2.0, 2.0),
        (3.0, 3.0)
    };
            string serializedPoints = Area.SerializePolygonPoints(polygonCoordinates);
            var parsedCoordinates = Area.ParsePolygonPoints(serializedPoints);

            CollectionAssert.AreEqual(polygonCoordinates, parsedCoordinates);
        }

        [TestMethod]
        public void TestPolygonPointsParsingInvalidString()
        {
            string invalidPoints = "INVALID_DATA";
            var parsedCoordinates = Area.ParsePolygonPoints(invalidPoints);

            Assert.AreEqual(0, parsedCoordinates.Count, "Parsed coordinates should be empty for invalid input.");
        }

        [TestMethod]
        public void TestAreaCanBeDeleted()
        {
            string name = "Area to Delete";
            string description = "This area will be deleted.";
            var polygonPoints = new List<(double lat, double lng)>
            {
                (1.0, 1.0),
                (2.0, 2.0)
            };

            // Create the area with the list of coordinates
            Area area = Area.Create(name, description, polygonPoints);
            int areaId = area.Id;

            // Delete the area and verify deletion
            Area.Delete(areaId);
            Area deletedArea = Area.Find(areaId);

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