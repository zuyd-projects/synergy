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
        private readonly User authenticatedUser;
        private Dictionary<string, string> menuItems = new Dictionary<string, string>();

        public ObservationMenu(User _authenticatedUser)
        {
            this.authenticatedUser = _authenticatedUser;
        }

        public Dictionary<string, string> GetMenuItems()
        {
            Dictionary<string, string> menuItems = new Dictionary<string, string>();
            if (this.authenticatedUser.Permission.CanViewAllObservations()) menuItems.Add("viewAll", "View all observations");
            if (this.authenticatedUser.Observations.Count > 0) menuItems.Add("viewOwn", "View own observations");
            if (this.authenticatedUser.Permission.CanCreateObservation()) menuItems.Add("create", "Create observation");
            if (this.authenticatedUser.Permission.CanExportObservations()) menuItems.Add("export", "Observaties exporteren");
            menuItems.Add("back", "Return to main menu");
            return menuItems;
        }

        public void Show()
        {
            menuItems = this.GetMenuItems();
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
            List<FormField> fields = new List<FormField>();
            fields.Add(new FormField("specieName", "Enter new specie name", "string", true, _observation.Specie.Name));
            fields.Add(new FormField("specieCategory", "Enter new specie category", "string", true, _observation.Specie.Category));
            fields.Add(new FormField("longitude", "Enter new longitude", "float", true, _observation.Longitude.ToString()));
            fields.Add(new FormField("latitude", "Enter new latitude", "float", true, _observation.Latitude.ToString()));
            fields.Add(new FormField("description", "Enter new description", "string", true, _observation.Description));
            fields.Add(new FormField("photoUrl", "Enter new photo URL", "string", true, _observation.PhotoUrl));

            Dictionary<string, object> values = new Form(fields).Prompt();
            if (values == null)
            {
                ViewObservation(_observation);
                return;
            }

            _observation.Specie = Specie.FindOrCreate((string)values["specieName"], (string)values["specieCategory"]);
            _observation.Longitude = (float)values["longitude"];
            _observation.Latitude = (float)values["latitude"];
            _observation.Description = (string)values["description"];
            _observation.PhotoUrl = (string)values["photoUrl"];

            _observation.Update(this.authenticatedUser);

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
            List<FormField> fields = new List<FormField>();
            fields.Add(new FormField("specieName", "Enter specie name", "string", true));
            fields.Add(new FormField("specieCategory", "Enter specie category", "string", true));
            fields.Add(new FormField("longitude", "Enter longitude", "float", true));
            fields.Add(new FormField("latitude", "Enter latitude", "float", true));
            fields.Add(new FormField("description", "Enter description", "string", true));
            fields.Add(new FormField("photoUrl", "Enter photo URL", "string", true));

            Dictionary<string, object> values = new Form(fields).Prompt();
            Specie specie = Specie.FindOrCreate((string)values["specieName"], (string)values["specieCategory"]);

            List<Observation> similarObservations = Observation.GetAll().Where(x => x.Specie.Name == specie.Name && x.Latitude == (float)values["latitude"] && x.Longitude == (float)values["longitude"]).ToList();
            int count = similarObservations.Count;
            if (count > 0)
            {
                Console.Clear();
                Console.WriteLine($"{count} vergelijkbare {(count > 1 ? "observaties" : "observatie")} van deze soort op deze locatie gevonden");
                foreach(var observation in similarObservations)
                {
                    Console.WriteLine($"Observatie ID: {observation.Id}, Specie: {observation.Specie.Name}, Beschrijving: {observation.Description}");
                }
                Console.WriteLine("Weet u zeker dat u nog een observatie wilt vastleggen? [J/N]");
                ConsoleKey key = ConsoleKey.NoName;
                while (key != ConsoleKey.N && key != ConsoleKey.J)
                {
                    key = Console.ReadKey().Key;
                    // If user presses N, return to menu
                    if (key == ConsoleKey.N) return;
                    // If user presses J, continue to create observation
                    if (key == ConsoleKey.J) break;
                }
            }

            Observation.Create(specie, (float)values["longitude"], (float)values["latitude"], (string)values["description"], (string)values["photoUrl"], this.authenticatedUser);
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
