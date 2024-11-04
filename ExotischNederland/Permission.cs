using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ExotischNederland.Models;

namespace ExotischNederland
{
    internal class Permission
    {
        private User User { get; set; }

        public Permission(User _user)
        {
            User = _user;
        }

        // Observation permissions
        public bool CanViewAllObservations()
        {
            if (this.User.Roles.Any(role => role.Name == "Beheerder" || role.Name == "Vrijwilliger"))
                return true;

            return false;
        }

        public bool CanCreateObservation()
        {
            if (this.User.Roles.Any(role =>
                role.Name == "Beheerder" || role.Name == "Vrijwilliger" || role.Name == "Wandelaar"))
                    return true;

            return false;
        }

        public bool CanEditObservation(Observation observation)
        {
            if (this.User.Roles.Any(role => role.Name == "Beheerder" || role.Name == "Vrijwilliger"))
                return true;

            return observation.User.Id == this.User.Id;
        }

        public bool CanDeleteObservation(Observation observation)
        {
            if (this.User.Roles.Any(role => role.Name == "Beheerder"))
                return true;

            return observation.User.Id == this.User.Id;
        }

        public bool CanExportObservations()
        {
            if (this.User.Roles.Any(role => role.Name == "Beheerder"))
                return true;

            return false;
        }


        // Area permissions
        public bool CanViewAllAreas()
        {
            if (this.User.Roles.Any(role => role.Name == "Beheerder" || role.Name == "Vrijwilliger"))
                return true;

            return false;
        }

        public bool CanCreateArea()
        {
            if (this.User.Roles.Any(role =>
                role.Name == "Beheerder" || role.Name == "Vrijwilliger" || role.Name == "Wandelaar"))
                return true;

            return false;
        }

        public bool CanEditArea()
        {
            if (this.User.Roles.Any(role => role.Name == "Beheerder" || role.Name == "Vrijwilliger"))
                return true;

            return false;
        }

        public bool CanDeleteArea()
        {
            if (this.User.Roles.Any(role => role.Name == "Beheerder"))
                return true;

            return false;
        }

        // User permissions
        public bool CanViewAllUsers()
        {
            if (this.User.Roles.Any(role => role.Name == "Beheerder"))
                return true;

            return false;
        }

        public bool CanEditUser(User user)
        {
            if (this.User.Roles.Any(role => role.Name == "Beheerder"))
                return true;

            return user.Id == this.User.Id;
        }

        public bool CanDeleteUser(User user)
        {
            if (this.User.Roles.Any(role => role.Name == "Beheerder"))
                return true;

            return user.Id == this.User.Id;
        }

        public bool CanCreateUser()
        {
            if (this.User.Roles.Any(role => role.Name == "Beheerder"))
                return true;

            return false;
        }
    }
}
