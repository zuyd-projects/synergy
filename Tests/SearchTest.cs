using ExotischNederland.Models;
using ExotischNederland.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class SearchTest : TransactionTest
    {

        [TestMethod]
        public void TestSearchCanQueryObservationsByDescription()
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
                    Observation.Create(specie, longitude, latitude, "Here's something containing A", photoUrl, user),
                    Observation.Create(specie, longitude, latitude, "Here's the thing containing B", photoUrl, user),
                    Observation.Create(specie, longitude, latitude, "And here's containing A again", photoUrl, user),
                };

            // Act
            List<Observation> result = new Search<Observation>(observations)
                .Query("Description", "containing A");

            // Assert
            Assert.AreEqual(2, result.Count);
        }
    }
}