using System;
using System.Collections.Generic;
using ExotischNederland.Models;

namespace ExotischNederland.Menus
{
    internal class PointOfInterestMenu : IMenu
    {
        private readonly User authenticatedUser;
        private Dictionary<string, string> menuItems = new Dictionary<string, string>();

        public PointOfInterestMenu(User _authenticatedUser)
        {
            authenticatedUser = _authenticatedUser;
        }

        public Dictionary<string, string> GetMenuItems()
        {
            var menuItems = new Dictionary<string, string>();
            var permission = authenticatedUser.Permission;

            if (permission.CanViewPointsOfInterest())
                menuItems.Add("viewAllPOIs", "View All Points of Interest");
            if (permission.CanCreatePointOfInterest())
                menuItems.Add("createPOI", "Create Point of Interest");

            menuItems.Add("back", "Return to main menu");
            return menuItems;
        }

        public void Show()
        {
            menuItems = GetMenuItems();
            while (true)
            {
                string selected = Helpers.MenuSelect(menuItems, true);

                if (selected == "viewAllPOIs") ViewAllPOIs();
                else if (selected == "createPOI") CreatePOI();
                else if (selected == "back") break;
            }
        }

        private void ViewAllPOIs()
        {
            Console.Clear();
            List<PointOfInterest> pointsOfInterest = PointOfInterest.GetAll();

            Console.WriteLine("All Points of Interest:");
            if (pointsOfInterest.Count > 0)
            {
                var poiOptions = new Dictionary<string, string>();
                foreach (var poi in pointsOfInterest)
                {
                    poiOptions[poi.Id.ToString()] = $"{poi.Name} - {poi.Type}";
                }
                poiOptions.Add("back", "Return to menu");

                string selectedPoiId = Helpers.MenuSelect(poiOptions, false);
                if (selectedPoiId != "back")
                {
                    int poiId = int.Parse(selectedPoiId);
                    PointOfInterest poi = pointsOfInterest.Find(p => p.Id == poiId);
                    ViewPOIDetails(poi);
                }
            }
            else
            {
                Console.WriteLine("No points of interest available.");
                Console.ReadKey();
            }
        }

        private void ViewPOIDetails(PointOfInterest poi)
        {
            Console.Clear();
            Console.WriteLine("Point of Interest Details:");
            Console.WriteLine($"Name: {poi.Name}");
            Console.WriteLine($"Description: {poi.Description}");
            Console.WriteLine($"Type: {poi.Type}");
            Console.WriteLine($"Location: Latitude {poi.Latitude}, Longitude {poi.Longitude}");

            var options = new Dictionary<string, string> { { "back", "Return to POI list" } };
            if (authenticatedUser.Permission.CanEditPointOfInterest(poi))
            {
                options.Add("edit", "Edit this point of interest");
            }
            if (authenticatedUser.Permission.CanDeletePointOfInterest(poi))
            {
                options.Add("delete", "Delete this point of interest");
            }

            string selectedOption = Helpers.MenuSelect(options, true);

            if (selectedOption == "edit") EditPOI(poi);
            else if (selectedOption == "delete") DeletePOI(poi);
        }

        private void CreatePOI()
        {
            var fields = new List<FormField>
            {
                new FormField("name", "Enter POI name", "string", true),
                new FormField("description", "Enter POI description", "string", false),
                new FormField("type", "Enter POI type", "string", true),
                new FormField("longitude", "Enter POI longitude", "float", true),
                new FormField("latitude", "Enter POI latitude", "float", true)
            };
            var values = new Form(fields).Prompt();
            if (values == null) return;

            string name = (string)values["name"];
            string description = (string)values["description"];
            string type = (string)values["type"];
            float longitude = (float)values["longitude"];
            float latitude = (float)values["latitude"];

            PointOfInterest poi = PointOfInterest.Create(name, description, type, longitude, latitude);
            Console.WriteLine($"Point of Interest '{name}' created successfully with ID: {poi.Id}");
            Console.ReadKey();
        }

        private void EditPOI(PointOfInterest poi)
        {
            var fields = new List<FormField>
            {
                new FormField("name", "New name (leave blank to keep current)", "string", false, poi.Name),
                new FormField("description", "New description (leave blank to keep current)", "string", false, poi.Description),
                new FormField("type", "New type (leave blank to keep current)", "string", false, poi.Type),
                new FormField("longitude", "New longitude (leave blank to keep current)", "float", false, poi.Longitude.ToString()),
                new FormField("latitude", "New latitude (leave blank to keep current)", "float", false, poi.Latitude.ToString())
            };
            var values = new Form(fields).Prompt();
            if (values == null) return;

            string newName = (string)values["name"] ?? poi.Name;
            string newDescription = (string)values["description"] ?? poi.Description;
            string newType = (string)values["type"] ?? poi.Type;
            float newLongitude = values.ContainsKey("longitude") ? (float)values["longitude"] : poi.Longitude;
            float newLatitude = values.ContainsKey("latitude") ? (float)values["latitude"] : poi.Latitude;

            poi.Update(newName, newDescription, newType, newLongitude, newLatitude);
            Console.WriteLine("Point of Interest updated successfully.");
            Console.ReadKey();
        }

        private void DeletePOI(PointOfInterest poi)
        {
            Console.Clear();
            Console.Write($"Are you sure you want to delete the point of interest '{poi.Name}'? [Y/N] ");
            ConsoleKey confirmation = Console.ReadKey().Key;

            if (confirmation == ConsoleKey.Y)
            {
                PointOfInterest.Delete(poi.Id);
                Console.WriteLine("\nPoint of Interest deleted successfully.");
            }
            else
            {
                ViewPOIDetails(poi);
            }
            Console.ReadKey();
        }
    }
}