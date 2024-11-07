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
    public class RouteTest: TransactionTest
    {
        private User testUser;
        private Area testArea;

        [TestInitialize]
        public void Setup()
        {
            testUser = User.Create("Test User", "test@test.com", "password");
            testUser.AssignRole(Role.Find("Beheerder"));
            testArea = Area.Create("test", "test", "dfdfgdf", testUser);
        }

        [TestMethod]
        public void TestRouteCanBeCreated()
        {
            string name = "Test Route";
            string description = "A scenic route for testing.";
            int areaId = testArea.Id; // Assuming an area with ID 1 exists

            Route route = Route.Create(name, description, testArea, testUser);

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
            Route route = Route.Create(name, description, testArea, testUser);

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
            Route route = Route.Create(name, description, testArea, testUser);
            int routeId = route.Id;

            // Delete the route and verify deletion
            Route.Delete(routeId);
            Route deletedRoute = Route.Find(routeId);

            Assert.IsNull(deletedRoute);
        }
    }
}

