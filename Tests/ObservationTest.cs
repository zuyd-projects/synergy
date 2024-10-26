using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ExotischNederland.Models;

namespace Tests
{
    [TestClass]
    public class ObservationTest : TransactionTest
    {
        [TestMethod]
        public void TestObservationCanBeCreated()
        {
            // Arrange
            User user = User.Create("Test User", "test@test.com", "password");
            Specie specie = Specie.Create("Test Specie", "Test Description");
            double longitude = 1.0;
            double latitude = 1.0;
            string description = "Test Description";
            string photoUrl = "http://test.com/photo.jpg";

            // Act
            Observation observation = Observation.Create(specie, longitude, latitude, description, photoUrl, user);

            // Assert
            Assert.IsNotNull(observation);
            Assert.AreEqual(specie.Id, observation.Specie.Id);
            Assert.AreEqual(longitude, observation.Longitude);
            Assert.AreEqual(latitude, observation.Latitude);
            Assert.AreEqual(description, observation.Description);
            Assert.AreEqual(photoUrl, observation.PhotoUrl);
            Assert.AreEqual(user.Id, observation.User.Id);
        }

        [TestMethod]
        public void TestObservationCanBeUpdated()
        {
            // Arrange
            User user = User.Create("Test User", "test@test.com", "password");
            Specie specie = Specie.Create("Test Specie", "Test Description");
            double longitude = 1.0;
            double latitude = 1.0;
            string description = "Test Description";
            string photoUrl = "http://test.com/photo.jpg";
            Observation observation = Observation.Create(specie, longitude, latitude, description, photoUrl, user);
            Assert.IsNotNull(observation);

            // Act
            string newDescription = "New Description";
            string newPhotoUrl = "http://test.com/newphoto.jpg";
            observation.Description = newDescription;
            observation.PhotoUrl = newPhotoUrl;
            observation.Update();

            // Assert
            Observation updatedObservation = Observation.Find(observation.Id);
            Assert.IsNotNull(updatedObservation);
            Assert.AreEqual(newDescription, updatedObservation.Description);
            Assert.AreEqual(newPhotoUrl, updatedObservation.PhotoUrl);
        }

        [TestMethod]
        public void TestObservationCanBeDeleted()
        {
            // Arrange
            User user = User.Create("Test User", "test@test.com", "password");
            Specie specie = Specie.Create("Test Specie", "Test Description");
            double longitude = 1.0;
            double latitude = 1.0;
            string description = "Test Description";
            string photoUrl = "http://test.com/photo.jpg";
            Observation observation = Observation.Create(specie, longitude, latitude, description, photoUrl, user);
            Assert.IsNotNull(observation);

            // Act
            observation.Delete();

            // Assert
            Observation deletedObservation = Observation.Find(observation.Id);
            Assert.IsNull(deletedObservation);
        }
    }
}