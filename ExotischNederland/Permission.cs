using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool CanCreateObservation()
        {
            if (this.User.Roles.Any(role =>
                role.Name == "Beheerder" || role.Name == "Vrijwilliger" || role.Name == "Wandelaar"))
            {
                return true;
            }
            return false;
        }

        public bool CanEditObservation(Observation observation)
        {
            if (this.User.Roles.Any(role => 
                role.Name == "Beheerder" || role.Name == "Vrijwilliger"))
            {
                return true;
            }
            return observation.User.Id == this.User.Id;
        }

        public bool CanDeleteObservation(Observation observation)
        {
            if (this.User.Roles.Any(role => role.Name == "Beheerder"))
            {
                return true;
            }
            return observation.User.Id == this.User.Id;
        }
    }
}
