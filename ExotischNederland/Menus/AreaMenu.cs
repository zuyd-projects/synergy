﻿using ExotischNederland.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.Menus
{
    internal class AreaMenu: IMenu
    {
        private readonly User authenticatedUser;
        private Dictionary<string, string> menuItems = new Dictionary<string, string>();

        public AreaMenu(User _authenticatedUser)
        {
            this.authenticatedUser = _authenticatedUser;
        }

        public Dictionary<string, string> GetMenuItems()
        {
            Dictionary<string, string> menuItems = new Dictionary<string, string>();
            if (this.authenticatedUser.Permission.CanViewAllAreas()) menuItems.Add("viewAll", "Bekijk alle gebieden");
            if (this.authenticatedUser.Permission.CanCreateArea()) menuItems.Add("create", "Nieuw gebied");
            menuItems.Add("back", "Keer terug naar het hoofdmenu");
            return menuItems;
        }

        public void Show()
        {
            this.menuItems = this.GetMenuItems();
            string selected = Helpers.MenuSelect(this.menuItems, true);

            if (selected == "viewAll")
            {
                ViewAreas(Area.GetAll());
                Show();
            }
            else if (selected == "create")
            {
                CreateArea();
                Show();
            }
        }

        private void ViewAreas(List<Area> _areas)
        {
            Console.Clear();

            if (_areas.Count > 0)
            {
                Dictionary<string, string> options = _areas.ToDictionary(x => x.Id.ToString(), x => $"{x.Id}. {x.Name}");
                options.Add("back", "Ga terug");
                string selectedArea = Helpers.MenuSelect(options, false);
                if (selectedArea == "back")
                {
                    return;
                }
                else
                {
                    Area area = _areas.Find(x => x.Id == int.Parse(selectedArea));
                    this.ViewArea(area);
                }
            }
            else
            {
                Console.WriteLine("Geen gebieden gevonden");
                Console.ReadKey();
            }
        }

        private void ViewArea(Area _area)
        {
            Console.Clear();
            List<string> text = new List<string>();
            text.Add("Observatie details:");
            text.Add($"Naam: {_area.Name}");
            text.Add($"Beschrijving: {_area.Description}");

            Dictionary<string, string> menu = new Dictionary<string, string>();
            if (this.authenticatedUser.Permission.CanEditArea()) menu.Add("edit", "Gebied bewerken");
            if (this.authenticatedUser.Permission.CanDeleteArea()) menu.Add("delete", "Gebied verwijderen");
            menu.Add("back", "Ga terug");
            string selected = Helpers.MenuSelect(menu, true, text);

            if (selected == "edit")
            {
                EditArea(_area);
            }
            else if (selected == "delete")
            {
                DeleteArea(_area);
            }
        }

        private void CreateArea()
        {
            Console.Clear();
            List<FormField> fields = new List<FormField>();
            fields.Add(new FormField("name", "Voer de naam van het gebied in", "string", true));
            fields.Add(new FormField("description", "Voer de beschrijving van het gebied in", "string", false));
            fields.Add(new FormField("polygonPoints", "Gebruik [URL_TO_VERCEL_APP] om de polygon te genereren en plak het resultaat hier", "polygonString", true));
            
            Dictionary<string, object> values = new Form(fields).Prompt();
            if (values == null) return;

            Area.Create((string)values["name"], (string)values["description"], (string)values["polygonPoints"], this.authenticatedUser);
            
            Console.WriteLine("Gebied gecreëerd!");
            Console.WriteLine("Druk op een toets om terug te keren naar het menu");
            Console.ReadKey();
        }

        private void EditArea(Area _area)
        {
            Console.Clear();
            List<FormField> fields = new List<FormField>();
            fields.Add(new FormField("name", "Voer de naam van het gebied in", "string", true, _area.Name));
            fields.Add(new FormField("description", "Voer de beschrijving van het gebied in", "string", false, _area.Description));
            fields.Add(new FormField("polygonPoints", "Gebruik [URL_TO_VERCEL_APP] om de polygon te genereren en plak het resultaat hier", "polygonString", true, _area.PolygonPoints));

            Dictionary<string, object> values = new Form(fields).Prompt();
            if (values == null)
            {
                ViewArea(_area);
                return;
            }

            _area.Name = (string)values["name"];
            _area.Description = (string)values["description"];
            _area.PolygonPoints = (string)values["polygonPoints"];
            _area.Update(this.authenticatedUser);
            
            Console.WriteLine("Gebied bijgewerkt!");
            Console.WriteLine("Druk op een toets om terug te keren naar het menu");
            Console.ReadKey();
        }

        private void DeleteArea(Area _area)
        {
            Console.Clear();
            Console.Write($"Weet u zeker dat u observatie {_area.Id} wilt verwijderen? [J/N]");
            if (Helpers.ConfirmPrompt())
            {
                _area.Delete(this.authenticatedUser);
                Console.WriteLine("Gebied verwijderd!");
                Console.WriteLine("Druk op een toets om terug te keren naar het menu");
                Console.ReadKey();
                return;
            }
            ViewArea(_area);
        }
    }
}
