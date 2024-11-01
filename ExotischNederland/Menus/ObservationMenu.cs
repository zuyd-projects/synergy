using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExotischNederland.Models;
using System.IO;

namespace ExotischNederland.Menus
{
    internal class ObservationMenu
    {
        private User authenticatedUser;
        Dictionary<string, string> menuItems = new Dictionary<string, string>();

        public ObservationMenu(User _authenticatedUser)
        {
            this.authenticatedUser = _authenticatedUser;
        }

        private void GetMenuItems()
        {
            menuItems.Clear();
            if (this.authenticatedUser.Permission.CanViewAllObservations()) menuItems.Add("viewAll", "View all observations");
            if (this.authenticatedUser.Observations.Count > 0) menuItems.Add("viewOwn", "View own observations");
            if (this.authenticatedUser.Permission.CanCreateObservation()) menuItems.Add("create", "Create observation");
            if (this.authenticatedUser.Permission.CanExportObservations()) menuItems.Add("export", "Observaties exporteren");
            menuItems.Add("back", "Return to main menu");
        }

        public void Show()
        {
            this.GetMenuItems();
            string selected = Helpers.MenuSelect(this.menuItems, true);

            if (selected == "viewAll")
            {
                ViewObservations(Observation.GetAll());
                Show();
            }
            else if (selected == "create")
            {
                CreateObservation();
                Show();
            }
            else if (selected == "viewOwn")
            {
                ViewObservations(this.authenticatedUser.Observations);
                Show();
            }
            else if (selected == "export")
            {
                ExportObservations();
                Show();
            }
        }

        private void ViewObservations(List<Observation> _observations)
        {
            Console.Clear();
            Console.WriteLine("Observaties:");

            if (_observations.Count > 0)
            {
                Dictionary<string, string> options = _observations.ToDictionary(x => x.Id.ToString(), x => $"{x.Id}. Specie: {x.Specie.Name}, Description: {x.Description}");
                options.Add("back", "Ga terug");
                string selectedObservation = Helpers.MenuSelect(options, false);
                if (selectedObservation == "back")
                {
                    return;
                }
                else
                {
                    Observation observation = _observations.Find(x => x.Id == int.Parse(selectedObservation));
                    this.ViewObservation(observation);
                }
            }
            else
            {
                Console.WriteLine("Geen observaties gevonden");
                Console.ReadKey();
            }
        }

        private void ViewObservation(Observation _observation)
        {
            Console.Clear();
            List<string> text = new List<string>();
            text.Add("Observatie details:");
            text.Add($"Specie: {_observation.Specie.Name}");
            text.Add($"Description: {_observation.Description}");
            text.Add($"Longitude: {_observation.Longitude}");
            text.Add($"Latitude: {_observation.Latitude}");
            text.Add($"Photo URL: {_observation.PhotoUrl}");
            text.Add($"User: {_observation.User.Name}");
            text.Add("Press a key to return to the observations");

            Dictionary<string, string> menu = new Dictionary<string, string>();
            if (this.authenticatedUser.Permission.CanEditObservation(_observation)) menu.Add("edit", "Observatie bewerken");
            if (this.authenticatedUser.Permission.CanDeleteObservation(_observation)) menu.Add("delete", "Observatie verwijderen");
            menu.Add("back", "Terug naar menu");
            string selected = Helpers.MenuSelect(menu, true, text);

            if (selected == "edit")
            {
                EditObservation(_observation);
            }
            else if (selected == "delete")
            {
                DeleteObservation(_observation);
            }
        }

        private void EditObservation(Observation _observation)
        {
            Console.Clear();
            
            Console.WriteLine($"Enter new Specie name: [{_observation.Specie.Name}]");
            string specieName = Console.ReadLine();
            if (string.IsNullOrEmpty(specieName)) specieName = _observation.Specie.Name;
            Console.WriteLine($"Enter new Specie category: [{_observation.Specie.Category}]");
            string specieCategory = Console.ReadLine();
            if (string.IsNullOrEmpty(specieCategory)) specieCategory = _observation.Specie.Category;
            Specie specie = Specie.FindOrCreate(specieName, specieCategory);
            Console.WriteLine($"Enter new Longitude: [{_observation.Longitude}]");
            string inputLongitude = Console.ReadLine();
            if (string.IsNullOrEmpty(inputLongitude)) inputLongitude = _observation.Latitude.ToString();
            float longitude = float.Parse(inputLongitude);
            Console.WriteLine($"Enter new Latitude: [{_observation.Latitude}]");
            string inputLatitude = Console.ReadLine();
            if (string.IsNullOrEmpty(inputLatitude)) inputLatitude = _observation.Latitude.ToString();
            float latitude = float.Parse(inputLatitude);
            Console.WriteLine($"Enter new Description: [{_observation.Description}]");
            string description = Console.ReadLine();
            if (string.IsNullOrEmpty(description)) description = _observation.Description;
            Console.WriteLine($"Enter new Photo URL: [{_observation.PhotoUrl}]");
            string photoUrl = Console.ReadLine();
            if (string.IsNullOrEmpty(photoUrl)) photoUrl = _observation.PhotoUrl;

            _observation.Specie = specie;
            _observation.Longitude = longitude;
            _observation.Latitude = latitude;
            _observation.Description = description;
            _observation.PhotoUrl = photoUrl;
            _observation.Update(this.authenticatedUser);

            Console.WriteLine("Observation updated!");
            Console.ReadKey();
        }

        private void DeleteObservation(Observation _observation)
        {
            Console.Clear();

            Console.Write($"Weet u zeker dat u observatie {_observation.Id} wilt verwijderen? ");
            Console.WriteLine("J/N");
            ConsoleKey key = Console.ReadKey().Key;
            if (key == ConsoleKey.J)
            {
                _observation.Delete(this.authenticatedUser);
                Console.WriteLine("Observation deleted!");
                Console.ReadKey();
                return;
            }
            ViewObservation(_observation);
        }

        private void CreateObservation()
        {
            Console.Clear();
            Console.WriteLine("Creating a new observation...");

            Console.WriteLine("Enter Specie name:");
            string specieName = Console.ReadLine();
            Console.WriteLine("Enter Specie category:");
            string specieCategory = Console.ReadLine();

            Specie specie = Specie.FindOrCreate(specieName, specieCategory);

            Console.WriteLine("Enter Longitude:");
            float longitude = float.Parse(Console.ReadLine());
            Console.WriteLine("Enter Latitude:");
            float latitude = float.Parse(Console.ReadLine());
            Console.WriteLine("Enter Description:");
            string description = Console.ReadLine();
            Console.WriteLine("Enter Photo URL:");
            string photoUrl = Console.ReadLine();

            Observation.Create(specie, longitude, latitude, description, photoUrl, this.authenticatedUser);
            Console.WriteLine("Observation created!");
            Console.ReadKey();
        }

        private void ExportObservations()
        {
            Console.Clear();
            List<Observation> observations = Observation.GetAll();
            Console.WriteLine("Voer het pad in naar het exportbestand:");
            string filePath = Console.ReadLine();

            using (StreamWriter writer = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "../../../ExotischNederland/", filePath)))
            {
                // Schrijf de header
                writer.WriteLine("Id,Specie,Longitude,Latitude,Description,PhotoUrl,UserId");

                // Schrijf elke observatie
                foreach (var observation in observations)
                {
                    writer.WriteLine($"{observation.Id},{observation.Specie.Name},{observation.Longitude},{observation.Latitude},{observation.Description},{observation.PhotoUrl},{observation.User.Id}");
                }
            }

            Console.WriteLine($"Observations have been exported to {filePath}");
            Console.ReadKey();
        }
    }
}
