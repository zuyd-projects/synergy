using ExotischNederland.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class PointOfInterestTest : TransactionTest
    {
        [TestMethod]
        public void TestPointOfInterestCanBeCreated()
        {
            // Arrange
            string name = "Test Point";
            string description = "A test point of interest";
            string type = "Historical";
            float longitude = 4.895168f;
            float latitude = 52.370216f;

            // Act
            PointOfInterest poi = PointOfInterest.Create(name, description, type, longitude, latitude);

            // Assert
            Assert.AreEqual(name, poi.Name);
            Assert.AreEqual(description, poi.Description);
            Assert.AreEqual(type, poi.Type);
            Assert.AreEqual(longitude, poi.Longitude);
            Assert.AreEqual(latitude, poi.Latitude);
        }

        [TestMethod]
        public void TestPointOfInterestCanBeUpdated()
        {
            // Arrange
            string name = "Test Point";
            string description = "A test point of interest";
            string type = "Historical";
            float longitude = 4.895168f;
            float latitude = 52.370216f;
            PointOfInterest poi = PointOfInterest.Create(name, description, type, longitude, latitude);

            // Act
            string updatedName = "Updated Test Point";
            string updatedDescription = "Updated description";
            string updatedType = "Cultural";
            float updatedLongitude = 5.895168f;
            float updatedLatitude = 53.370216f;
            poi.Update(updatedName, updatedDescription, updatedType, updatedLongitude, updatedLatitude);

            // Assert
            Assert.AreEqual(updatedName, poi.Name);
            Assert.AreEqual(updatedDescription, poi.Description);
            Assert.AreEqual(updatedType, poi.Type);
            Assert.AreEqual(updatedLongitude, poi.Longitude);
            Assert.AreEqual(updatedLatitude, poi.Latitude);

            PointOfInterest fetchedPoi = PointOfInterest.Find(poi.Id);
            Assert.AreEqual(updatedName, fetchedPoi.Name);
            Assert.AreEqual(updatedDescription, fetchedPoi.Description);
            Assert.AreEqual(updatedType, fetchedPoi.Type);
            Assert.AreEqual(updatedLongitude, fetchedPoi.Longitude);
            Assert.AreEqual(updatedLatitude, fetchedPoi.Latitude);
        }

        [TestMethod]
        public void TestPointOfInterestCanBeDeleted()
        {
            // Arrange
            string name = "Test Point";
            string description = "A test point of interest";
            string type = "Historical";
            float longitude = 4.895168f;
            float latitude = 52.370216f;
            PointOfInterest poi = PointOfInterest.Create(name, description, type, longitude, latitude);

            // Act
            PointOfInterest.Delete(poi.Id);

            // Assert
            PointOfInterest fetchedPoi = PointOfInterest.Find(poi.Id);
            Assert.IsNull(fetchedPoi);
        }
    }
}
