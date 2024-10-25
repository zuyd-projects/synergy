using ExotischNederland.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class RoleTest : TransactionTest
    {
        [TestMethod]
        public void TestRoleCanBeCreated()
        {
            string name = "Admin";
            string description = "Administrator";

            Role role = Role.Create(name, description);

            Assert.IsInstanceOfType(role, typeof(Role));
            Assert.IsTrue(role.Name == name);

            Role fetchedRole = Role.Find(name);

            Assert.IsTrue(fetchedRole.Name == name);
        }

        [TestMethod]
        public void TestRoleCanBeAddedToUser()
        {
            string name = "Admin";
            string description = "Administrator";

            Role role = Role.Create(name, description);

            string userName = "John Doe";
            string email = "test@test.com";
            string password = "password";

            User user = User.Create(userName, email, password);

            user.AssignRole(role);

            Assert.IsTrue(user.Roles.Any(r => r.Id == role.Id));

            User fetchedUser = User.Find(user.Id);

            Assert.IsTrue(fetchedUser.Roles.Any(r => r.Id == role.Id));
        }

        [TestMethod]
        public void TestRoleCanBeRemovedFromUser()
        {
            string name = "Admin";
            string description = "Administrator";

            Role role = Role.Create(name, description);

            string userName = "John Doe";
            string email = "test@test.com";
            string password = "password";

            User user = User.Create(userName, email, password);

            user.AssignRole(role);
            Assert.IsTrue(user.Roles.Any(r => r.Id == role.Id));

            user.RemoveRole(role);
            Assert.IsFalse(user.Roles.Any(r => r.Id == role.Id));

            User fetchedUser = User.Find(user.Id);

            Assert.IsFalse(fetchedUser.Roles.Any(r => r.Id == role.Id));
        }

        [TestMethod]
        public void TestRolesQuickly()
        {
            string name = "Admin";
            string description = "Administrator";

            Role role = Role.Create(name, description);

            string userName = "John Doe";
            string email = "test@test.com";
            string password = "password";

            User user = User.Create(userName, email, password);

            user.AssignRole(role);
            Assert.IsTrue(user.Roles.Any(r => r.Id == role.Id));

            User fetchedUser = User.Find(user.Id);
            Assert.IsTrue(fetchedUser.Roles.Any(r => r.Id == role.Id));

            user.RemoveRole(role);
            Assert.IsFalse(user.Roles.Any(r => r.Id == role.Id));

            fetchedUser = User.Find(user.Id);
            Assert.IsFalse(fetchedUser.Roles.Any(r => r.Id == role.Id));

            fetchedUser.AssignRole(role);
            Assert.IsTrue(fetchedUser.Roles.Any(r => r.Id == role.Id));

            fetchedUser.RemoveRole(role);
            fetchedUser.AssignRole(role);
            Assert.IsTrue(fetchedUser.Roles.Any(r => r.Id == role.Id));
        }
    }
}
