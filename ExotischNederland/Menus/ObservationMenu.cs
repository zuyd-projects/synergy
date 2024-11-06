using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExotischNederland.Models;
using System.IO;

namespace ExotischNederland.Menus
{
    internal class ObservationMenu: IMenu
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
            if (this.authenticatedUser.Permission.CanViewAllObservations()) menuItems.Add("viewAll", "Bekijk alle observaties");
            if (this.authenticatedUser.Observations.Count > 0) menuItems.Add("viewOwn", "Bekijk eigen observaties");
            if (this.authenticatedUser.Permission.CanCreateObservation()) menuItems.Add("create", "Observatie aanmaken");
            if (this.authenticatedUser.Permission.CanExportObservations()) menuItems.Add("export", "Observaties exporteren");
            menuItems.Add("back", "Keer terug naar het hoofdmenu");
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
                if (selectedObservation == "back" || selectedObservation is null)
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
            text.Add($"Soort: {_observation.Specie.Name}");
            text.Add($"Beschrijving: {_observation.Description}");
            text.Add($"Lengtegraad : {_observation.Longitude}");
            text.Add($"Breedtegraad: {_observation.Latitude}");
            text.Add($"Foto-URL: {_observation.PhotoUrl}");
            text.Add($"Gebruiker: {_observation.User.Name}");
            text.Add("Druk op een toets om terug te keren naar de observaties");

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
            fields.Add(new FormField("specieName", "Voer een nieuwe soortnaam in", "string", true, _observation.Specie.Name));
            fields.Add(new FormField("specieCategory", "Voer een nieuwe soortcategorie in", "string", true, _observation.Specie.Category));
            fields.Add(new FormField("longitude", "Voer een nieuwe lengtegraad in", "float", true, _observation.Longitude.ToString()));
            fields.Add(new FormField("latitude", "Voer een nieuwe breedtegraad in", "float", true, _observation.Latitude.ToString()));
            fields.Add(new FormField("description", "Voer een nieuwe beschrijving in", "string", true, _observation.Description));
            fields.Add(new FormField("photoUrl", "Voer een nieuwe foto-URL in", "string", true, _observation.PhotoUrl));

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

            Console.WriteLine("Observatie bijgewerkt!");
            Console.ReadKey();
        }

        private void DeleteObservation(Observation _observation)
        {
            Console.Clear();

            Console.Write($"Weet u zeker dat u observatie {_observation.Id} wilt verwijderen? [J/N]");
            if (Helpers.ConfirmPrompt())
            {
                _observation.Delete(this.authenticatedUser);
                Console.WriteLine("Observatie verwijderd!");
                Console.ReadKey();
                return;
            }
            ViewObservation(_observation);
        }

        private void CreateObservation()
        {
            Console.Clear();
            List<FormField> fields = new List<FormField>();
            fields.Add(new FormField("specieName", "Voer de soortnaam in", "string", true));
            fields.Add(new FormField("specieCategory", "Voer de soortcategorie in", "string", true));
            fields.Add(new FormField("longitude", "Voer de lengtegraad in", "float", true));
            fields.Add(new FormField("latitude", "Voer de breedtegraad in", "float", true));
            fields.Add(new FormField("description", "Voer een beschrijving in", "string", true));
            fields.Add(new FormField("photoUrl", "Voer de foto-URL in", "string", true));

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
                if (!Helpers.ConfirmPrompt()) return;
            }

            Observation.Create(specie, (float)values["longitude"], (float)values["latitude"], (string)values["description"], (string)values["photoUrl"], this.authenticatedUser);
            Console.WriteLine("Observatie aangemaakt!");
            Console.ReadKey();
        }

        private void ExportObservations()
        {
            Console.Clear();
            List<FormField> fields = new List<FormField>
            {
                new FormField("fileName", "Voer de naam in van het exportbestand", "string", true),
                new FormField("from", "Start (dd-mm-yyyy HH:MM)", "datetime", false),
                new FormField("to", "Eind (dd-mm-yyyy HH:MM)", "datetime", false)
            };
            Dictionary<string, object> values = new Form(fields).Prompt();
            if (values == null) return;
            
            string path = Helpers.GetSafeFilePath((string)values["fileName"], "csv", "ObservatieExports");

            DateTime? fromDate = !string.IsNullOrEmpty((string)values["from"]) ? (DateTime?)DateTime.Parse((string)values["from"]) : null;
            DateTime? toDate = !string.IsNullOrEmpty((string)values["to"]) ? (DateTime?)DateTime.Parse((string)values["to"]) : null;

            using (StreamWriter writer = new StreamWriter(path))
            {
                // Write the header
                writer.WriteLine("Id,Specie,Longitude,Latitude,Description,PhotoUrl,UserId,TimeStamp");
                List<Observation> observations = Observation.GetRange(fromDate, toDate);
                // Write each observation
                foreach (var observation in observations)
                {
                    writer.WriteLine($"{observation.Id},{observation.Specie.Name},{observation.Longitude},{observation.Latitude},{observation.Description},{observation.PhotoUrl},{observation.User.Id},{observation.TimeStamp}");
                }
            }

            Console.WriteLine($"Waarnemingen zijn geëxporteerd naar {path}");
           
            
            Console.ReadKey();
        }
    }
}
