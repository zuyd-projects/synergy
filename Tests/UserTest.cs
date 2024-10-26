using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Transactions;
using ExotischNederland.Models;

namespace Tests
{
    [TestClass]
    public class UserTest : TransactionTest
    {
        [TestMethod]
        public void TestUserCanBeCreated()
        {
            string name = "John Doe";
            string email = "test@test.com";
            string password = "password";

            User user = User.Create(name, email, password);

            Assert.IsInstanceOfType(user, typeof(User));
            Assert.IsTrue(user.Name == name);
        }

        [TestMethod]
        public void TestUserCanBeAuthenticated()
        {
            string name = "John Doe";
            string email = "test@test.com";
            string password = "password";
            User.Create(name, email, password);

            User authenticatedUser = User.Authenticate(email, "wrongPassword");
            Assert.IsNotInstanceOfType(authenticatedUser, typeof(User));

            authenticatedUser = User.Authenticate(email, password);
            Assert.IsInstanceOfType(authenticatedUser, typeof(User));
            Assert.IsTrue(authenticatedUser.Name == name);
        }
    }
}