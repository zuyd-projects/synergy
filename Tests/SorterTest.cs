using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ExotischNederland.Tools;
using ExotischNederland.Models;

namespace Tests
{
    [TestClass]
    public class SorterTest: TransactionTest
    {
        [TestMethod]
        public void TestSorterCanSortObservationsByDescription()
        {
            // Arrange
            User user = User.Create("Test User", "test@test.com", "password");
            user.AssignRole(Role.Create("Beheerder"));
            Specie specie = Specie.Create("Test Specie", "Test Description");
            double longitude = 1.0;
            double latitude = 1.0;
            string photoUrl = "http://test.com/photo.jpg";

            List<Observation> observations = new List<Observation>
            {
                Observation.Create(specie, longitude, latitude, "A", photoUrl, user),
                Observation.Create(specie, longitude, latitude, "C", photoUrl, user),
                Observation.Create(specie, longitude, latitude, "B", photoUrl, user),
            };

            // Act
            List<Observation> sortedObservations = new Sorter<Observation>(observations)
                .Sort("Description")
                .ToList();

            // Assert
            Assert.AreEqual("A", sortedObservations[0].Description);
            Assert.AreEqual("B", sortedObservations[1].Description);
            Assert.AreEqual("C", sortedObservations[2].Description);
        }

        [TestMethod]
        public void TestSorterCanSortObservationsByUserIdAndDescription()
        {
            // Arrange
            User user = User.Create("Test User", "test@test.com", "password");
            user.AssignRole(Role.Create("Beheerder"));
            User user2 = User.Create("Test User 2", "test2@test.com", "password");
            user2.AssignRole(Role.Create("Beheerder"));
            Specie specie = Specie.Create("Test Specie", "Test Description");
            double longitude = 1.0;
            double latitude = 1.0;
            string photoUrl = "http://test.com/photo.jpg";

            List<Observation> observations = new List<Observation>
            {
                Observation.Create(specie, longitude, latitude, "A", photoUrl, user),
                Observation.Create(specie, longitude, latitude, "B", photoUrl, user2),
                Observation.Create(specie, longitude, latitude, "A", photoUrl, user2),
                Observation.Create(specie, longitude, latitude, "B", photoUrl, user)
            };

            // Act
            List<Observation> sortedObservations = new Sorter<Observation>(observations)
                .Sort("User.Id")
                .Sort("Description")
                .ToList();

            // Assert
            Assert.AreEqual("A", sortedObservations[0].Description);
            Assert.AreEqual(user.Id, sortedObservations[0].User.Id);
            Assert.AreEqual("B", sortedObservations[1].Description);
            Assert.AreEqual(user.Id, sortedObservations[1].User.Id);
            Assert.AreEqual("A", sortedObservations[2].Description);
            Assert.AreEqual(user2.Id, sortedObservations[2].User.Id);
            Assert.AreEqual("B", sortedObservations[3].Description);
            Assert.AreEqual(user2.Id, sortedObservations[3].User.Id);
        }
    }
}
