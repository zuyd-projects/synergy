using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ExotischNederland.Models;

namespace Tests
{
    [TestClass]
    public class ObservationTest : TransactionTest
    {
        [TestMethod]
        public void TestObservationCannotBeCreatedByUnathorizedUser()
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
            Assert.IsNull(observation);
        }

        [TestMethod]
        public void TestObservationCanBeCreated()
        {
            // Arrange
            User user = User.Create("Test User", "test@test.com", "password");
            user.AssignRole(Role.Create("Wandelaar"));
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
        public void TestObservationCanBeUpdatedBySameUser()
        {
            // Arrange
            User user = User.Create("Test User", "test@test.com", "password");
            user.AssignRole(Role.Create("Wandelaar"));
            Specie specie = Specie.Create("Test Specie", "Test Description");
            double longitude = 1.0;
            double latitude = 1.0;
            string description = "Test Description";
            string photoUrl = "http://test.com/photo.jpg";
            Observation observation = Observation.Create(specie, longitude, latitude, description, photoUrl, user);

            // Act
            string newDescription = "New Description";
            string newPhotoUrl = "http://test.com/newphoto.jpg";
            observation.Description = newDescription;
            observation.PhotoUrl = newPhotoUrl;
            observation.Update(user);

            // Assert
            Observation updatedObservation = Observation.Find(observation.Id);
            Assert.IsNotNull(updatedObservation);
            Assert.AreEqual(newDescription, updatedObservation.Description);
            Assert.AreEqual(newPhotoUrl, updatedObservation.PhotoUrl);
        }

        [TestMethod]
        public void TestObservationCanBeUpdatedByBeheerder()
        {
            // Arrange
            User user = User.Create("Test User", "test@test.com", "password");
            user.AssignRole(Role.Create("Wandelaar"));
            User beheerder = User.Create("Beheerder", "beheerder@test.com", "password");
            beheerder.AssignRole(Role.Create("Beheerder"));
            Specie specie = Specie.Create("Test Specie", "Test Description");
            double longitude = 1.0;
            double latitude = 1.0;
            string description = "Test Description";
            string photoUrl = "http://test.com/photo.jpg";
            Observation observation = Observation.Create(specie, longitude, latitude, description, photoUrl, user);

            // Act
            string newDescription = "New Description";
            string newPhotoUrl = "http://test.com/newphoto.jpg";
            observation.Description = newDescription;
            observation.PhotoUrl = newPhotoUrl;
            observation.Update(beheerder);

            // Assert
            Observation updatedObservation = Observation.Find(observation.Id);
            Assert.IsNotNull(updatedObservation);
            Assert.AreEqual(newDescription, updatedObservation.Description);
            Assert.AreEqual(newPhotoUrl, updatedObservation.PhotoUrl);
        }

        [TestMethod]
        public void TestObservationCannotBeUpdatedByOtherUser()
        {
            // Arrange
            User user = User.Create("Test User", "test@test.com", "password");
            user.AssignRole(Role.Create("Wandelaar"));
            User user2 = User.Create("Test User2", "test2@test.com", "password");
            user2.AssignRole(Role.Create("Wandelaar"));
            Specie specie = Specie.Create("Test Specie", "Test Description");
            double longitude = 1.0;
            double latitude = 1.0;
            string description = "Test Description";
            string photoUrl = "http://test.com/photo.jpg";
            Observation observation = Observation.Create(specie, longitude, latitude, description, photoUrl, user);

            // Act
            string newDescription = "New Description";
            string newPhotoUrl = "http://test.com/newphoto.jpg";
            observation.Description = newDescription;
            observation.PhotoUrl = newPhotoUrl;
            observation.Update(user2);

            // Assert
            Observation updatedObservation = Observation.Find(observation.Id);
            Assert.IsNotNull(updatedObservation);
            Assert.AreNotEqual(newDescription, updatedObservation.Description);
            Assert.AreNotEqual(newPhotoUrl, updatedObservation.PhotoUrl);
        }

        [TestMethod]
        public void TestObservationCannotBeDeletedByOtherUser()
        {
            // Arrange
            User user = User.Create("Test User", "test@test.com", "password");
            user.AssignRole(Role.Create("Wandelaar"));
            User user2 = User.Create("Test User2", "test2@test.com", "password");
            user2.AssignRole(Role.Create("Wandelaar"));
            Specie specie = Specie.Create("Test Specie", "Test Description");
            double longitude = 1.0;
            double latitude = 1.0;
            string description = "Test Description";
            string photoUrl = "http://test.com/photo.jpg";
            Observation observation = Observation.Create(specie, longitude, latitude, description, photoUrl, user);
            Assert.IsNotNull(observation);

            // Act
            observation.Delete(user2);

            // Assert
            Observation deletedObservation = Observation.Find(observation.Id);
            Assert.IsNotNull(deletedObservation);
        }

        [TestMethod]
        public void TestObservationCanBeDeletedByOwnUser()
        {
            // Arrange
            User user = User.Create("Test User", "test@test.com", "password");
            user.AssignRole(Role.Create("Wandelaar"));
            Specie specie = Specie.Create("Test Specie", "Test Description");
            double longitude = 1.0;
            double latitude = 1.0;
            string description = "Test Description";
            string photoUrl = "http://test.com/photo.jpg";
            Observation observation = Observation.Create(specie, longitude, latitude, description, photoUrl, user);
            Assert.IsNotNull(observation);

            // Act
            observation.Delete(user);

            // Assert
            Observation deletedObservation = Observation.Find(observation.Id);
            Assert.IsNull(deletedObservation);
        }

        [TestMethod]
        public void TestObservationCanBeDeletedByBeheerder()
        {
            // Arrange
            User user = User.Create("Test User", "test@test.com", "password");
            user.AssignRole(Role.Create("Wandelaar"));
            User beheerder = User.Create("Test Beheerder", "beheerder@test.com", "password");
            beheerder.AssignRole(Role.Create("Beheerder"));
            Specie specie = Specie.Create("Test Specie", "Test Description");
            double longitude = 1.0;
            double latitude = 1.0;
            string description = "Test Description";
            string photoUrl = "http://test.com/photo.jpg";
            Observation observation = Observation.Create(specie, longitude, latitude, description, photoUrl, user);
            Assert.IsNotNull(observation);

            // Act
            observation.Delete(beheerder);

            // Assert
            Observation deletedObservation = Observation.Find(observation.Id);
            Assert.IsNull(deletedObservation);
        }
    }
}