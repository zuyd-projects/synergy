using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExotischNederland;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Tests
{
    [TestClass]
    public class ExportTest
    {
        [TestMethod]
        public void TestExportObservationsToCsv()
        {
            string filePath = "test_observations.csv";
            Program.ExportObservationsToCsv(filePath);

            Assert.IsTrue(File.Exists(filePath), "CSV file should be created.");

            string[] lines = File.ReadAllLines(filePath);
            Assert.IsTrue(lines.Length > 1, "CSV file should contain header and at least one observation.");

            // Clean up
            File.Delete(filePath);
        }
    }
}
