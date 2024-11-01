using ExotischNederland;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class DefaultTest
    {
        [TestMethod]
        public void ExampleTest()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void TestDBSettings()
        {
            Dictionary<string, string> dbSettings = Helpers.LoadSettings();

            Assert.AreEqual(5, dbSettings.Count);
        }
    }
}
