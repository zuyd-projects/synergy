using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ExotischNederland.Models;

namespace Tests
{
    [TestClass]
    public class SpecieTest: TransactionTest
    {
        [TestMethod]
        public void TestSpecieCanBeCreated()
        {
            // Arrange
            string name = "Test Specie";
            string category = "Test Category";

            // Act
            Specie specie = Specie.Create(name, category);

            // Assert
            Assert.AreEqual(name, specie.Name);
            Assert.AreEqual(category, specie.Category);
        }

        [TestMethod]
        public void TestSpecieCanBeUpdated() {
            // Arrange
            string name = "Test Specie";
            string category = "Test Category";
            Specie specie = Specie.Create(name, category);

            // Act
            string updatedName = "Updated Specie";
            string updatedCategory = "Updated Category";
            specie.Name = updatedName;
            specie.Category = updatedCategory;
            specie.Update();

            // Assert
            Assert.AreEqual(updatedName, specie.Name);
            Assert.AreEqual(updatedCategory, specie.Category);

            Specie fetchedSpecie = Specie.Find(specie.Id);
            Assert.AreEqual(updatedName, fetchedSpecie.Name);
            Assert.AreEqual(updatedCategory, fetchedSpecie.Category);
        }

        [TestMethod]
        public void TestSpecieCanBeDeleted() {
            // Arrange
            string name = "Test Specie";
            string category = "Test Category";
            Specie specie = Specie.Create(name, category);

            // Act
            specie.Delete();

            // Assert
            Specie fetchedSpecie = Specie.Find(specie.Id);
            Assert.IsNull(fetchedSpecie);
        }
    }
}
