using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        public bool CanViewAllObservations() => User.Roles.Any(role => role.Name == "Beheerder" || role.Name == "Vrijwilliger");
        public bool CanCreateObservation() => User.Roles.Any(role => role.Name == "Beheerder" || role.Name == "Vrijwilliger" || role.Name == "Wandelaar");
        public bool CanEditObservation(Observation observation) => User.Roles.Any(role => role.Name == "Beheerder" || role.Name == "Vrijwilliger") || observation.User.Id == User.Id;
        public bool CanDeleteObservation(Observation observation) => User.Roles.Any(role => role.Name == "Beheerder") || observation.User.Id == User.Id;
        public bool CanExportObservations() => User.Roles.Any(role => role.Name == "Beheerder");

        public bool CanViewAllAreas() => User.Roles.Any(role => role.Name == "Beheerder" || role.Name == "Vrijwilliger");
        public bool CanCreateArea() => User.Roles.Any(role => role.Name == "Beheerder" || role.Name == "Vrijwilliger" || role.Name == "Wandelaar");
        public bool CanEditArea() => User.Roles.Any(role => role.Name == "Beheerder" || role.Name == "Vrijwilliger");
        public bool CanDeleteArea() => User.Roles.Any(role => role.Name == "Beheerder");

        // Game permissions
        public bool CanManageGames() => User.Roles.Any(role => role.Name == "Beheerder");

        // Allow both "Familie" and "Kinderen" roles to play games
        public bool CanPlayGames() => User.Roles.Any(role => role.Name == "Beheerder" || role.Name == "Familie" || role.Name == "Kinderen");

        // Question and Answer permissions
        public bool CanManageQuestions() => this.CanManageGames() || User.Roles.Any(role => role.Name == "Beheerder");
        public bool CanManageAnswers() => this.CanManageQuestions() || User.Roles.Any(role => role.Name == "Beheerder");

        // Route permissions
        public bool CanManageRoutes() => User.Roles.Any(role => role.Name == "Beheerder");
        public bool CanViewRoutes() => User.Roles.Any(role => role.Name == "Beheerder" || role.Name == "Familie" || role.Name == "Kinderen");
        public bool CanCreateRoute() => User.Roles.Any(role => role.Name == "Beheerder");
        public bool CanEditRoute(Route route) => User.Roles.Any(role => role.Name == "Beheerder") || route.User.Id == User.Id;
        public bool CanDeleteRoute(Route route) => User.Roles.Any(role => role.Name == "Beheerder") || route.User.Id == User.Id;

        // User permissions
        public bool CanViewAllUsers() => User.Roles.Any(role => role.Name == "Beheerder");
        public bool CanEditUser(User user) => User.Roles.Any(role => role.Name == "Beheerder") || user.Id == User.Id;
        public bool CanDeleteUser(User user) => User.Roles.Any(role => role.Name == "Beheerder");
    }
}