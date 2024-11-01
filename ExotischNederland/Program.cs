using System;
using System.Collections.Generic;
using ExotischNederland.Models;
using System.Linq;
using ExotischNederland.DAL;

namespace ExotischNederland
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> menuItems = new Dictionary<string, string>
            {
                { "1", "Login" },
                { "2", "Register" },
                { "3", "Exit" }
            };

            while (true)
            {
                string selected = Helpers.MenuSelect(menuItems, true);
                if (selected == "1")
                {
                    Console.Clear();
                    Console.WriteLine("Enter your email:");
                    string email = Console.ReadLine();
                    Console.WriteLine("Enter your password:");
                    string password = Console.ReadLine();

                    User authenticatedUser = User.Authenticate(email, password);

                    if (authenticatedUser != null)
                    {
                        Console.WriteLine("Login successful!");
                        Console.WriteLine("Choose an option:");
                        Console.WriteLine("1. Manage Observations");
                        Console.WriteLine("2. Manage Areas");

                        string choice = Console.ReadLine();
                        if (choice == "1")
                        {
                            ObservationMenu(authenticatedUser);  // Enter observation menu after login
                        }
                        else if (choice == "2")
                        {
                            AreaMenu(authenticatedUser);  // Enter area management menu after login
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice. Returning to main menu.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Login failed!");
                    }

                    Console.WriteLine("Press a key to return to the menu");
                    Console.ReadKey();
                }
                else if (selected == "2")
                {
                    Console.Clear();
                    Console.WriteLine("Enter your name:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Enter your email:");
                    string email = Console.ReadLine();
                    Console.WriteLine("Enter your password:");
                    string password = Console.ReadLine();
                    User.Create(name, email, password);
                    Console.WriteLine("User created!");
                    Console.WriteLine("Press a key to return to the menu");
                    Console.ReadKey();
                }
                else if (selected == "3")
                {
                    break;
                }
            }
        }

        static void ObservationMenu(User user)
        {
            Dictionary<string, string> observationMenuItems = new Dictionary<string, string>
            {
                { "1", "Create Observation" },
                { "2", "View Observations" },
                { "3", "Update Observation" },
                { "4", "Delete Observation" },
                { "5", "Logout" }
            };

            while (true)
            {
                string selected = Helpers.MenuSelect(observationMenuItems, true);
                if (selected == "1")
                {
                    CreateObservation(user);
                }
                else if (selected == "2")
                {
                    ViewObservations(user);
                }
                else if (selected == "3")
                {
                    UpdateObservation(user);
                }
                else if (selected == "4")
                {
                    DeleteObservation(user);
                }
                else if (selected == "5")
                {
                    Console.Clear();
                    break;
                }
            }
        }

        static void CreateObservation(User user)
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

            Observation.Create(specie, longitude, latitude, description, photoUrl, user);
            Console.WriteLine("Observation created!");
            Console.ReadKey();
        }

        static void ViewObservations(User user)
        {
            Console.Clear();
            Console.WriteLine("Your Observations:");

            List<Observation> observations = user.GetObservations();
            if (observations.Count > 0)
            {
                foreach (var obs in observations)
                {
                    Console.WriteLine($"ID: {obs.Id}, Specie: {obs.Specie.Name}, Description: {obs.Description}");
                }
            }
            else
            {
                Console.WriteLine("No observations found.");
            }

            Console.ReadKey();
        }

        static void UpdateObservation(User user)
        {
            Console.Clear();
            Console.WriteLine("Enter the ID of the observation to update:");
            int observationId = int.Parse(Console.ReadLine());

            List<Observation> observations = user.GetObservations();
            Observation observationToUpdate = observations.FirstOrDefault(o => o.Id == observationId);

            if (observationToUpdate != null)
            {
                Console.WriteLine("Enter new Specie name:");
                string specieName = Console.ReadLine();
                Console.WriteLine("Enter new Specie category:");
                string specieCategory = Console.ReadLine();
                Specie specie = Specie.FindOrCreate(specieName, specieCategory);

                Console.WriteLine("Enter new Longitude:");
                float longitude = float.Parse(Console.ReadLine());
                Console.WriteLine("Enter new Latitude:");
                float latitude = float.Parse(Console.ReadLine());
                Console.WriteLine("Enter new Description:");
                string description = Console.ReadLine();
                Console.WriteLine("Enter new Photo URL:");
                string photoUrl = Console.ReadLine();

                observationToUpdate.Specie = specie;
                observationToUpdate.Longitude = longitude;
                observationToUpdate.Latitude = latitude;
                observationToUpdate.Description = description;
                observationToUpdate.PhotoUrl = photoUrl;

                observationToUpdate.Update(user);
                Console.WriteLine("Observation updated!");
            }
            else
            {
                Console.WriteLine("Observation not found.");
            }

            Console.ReadKey();
        }

        static void DeleteObservation(User user)
        {
            Console.Clear();
            Console.WriteLine("Enter the ID of the observation to delete:");
            int observationId = int.Parse(Console.ReadLine());

            List<Observation> observations = user.GetObservations();
            Observation observationToDelete = observations.FirstOrDefault(o => o.Id == observationId);

            if (observationToDelete != null)
            {
                observationToDelete.Delete(user);
                Console.WriteLine("Observation deleted!");
            }
            else
            {
                Console.WriteLine("Observation not found.");
            }

            Console.ReadKey();
        }

        static void AreaMenu(User user)
        {
            Dictionary<string, string> areaMenuItems = new Dictionary<string, string>
            {
                { "1", "Create Area" },
                { "2", "View Areas" },
                { "3", "Update Area" },
                { "4", "Delete Area" },
                { "5", "Logout" }
            };

            while (true)
            {
                string selected = Helpers.MenuSelect(areaMenuItems, true);
                if (selected == "1")
                {
                    CreateArea();
                }
                else if (selected == "2")
                {
                    ViewAreas();
                }
                else if (selected == "3")
                {
                    UpdateArea();
                }
                else if (selected == "4")
                {
                    DeleteArea();
                }
                else if (selected == "5")
                {
                    Console.Clear();
                    break;
                }
            }
        }
        static void CreateArea()
        {
            Console.Clear();
            Console.WriteLine("Creating a new area...");

            Console.WriteLine("Enter Area name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter Area description:");
            string description = Console.ReadLine();

            var polygonCoordinates = new List<(double lat, double lng)>();
            Console.WriteLine("Enter Polygon Points (type 'done' to finish):");
            while (true)
            {
                Console.Write("Enter latitude: ");
                string latInput = Console.ReadLine();
                if (latInput.ToLower() == "done") break;
                Console.Write("Enter longitude: ");
                string lngInput = Console.ReadLine();
                polygonCoordinates.Add((double.Parse(latInput), double.Parse(lngInput)));
            }

            Area area = Area.Create(name, description, polygonCoordinates);
            Console.WriteLine("Area created with ID: " + area.Id);
            Console.ReadKey();
        }

        static void ViewAreas()
        {
            Console.Clear();
            Console.WriteLine("List of Areas:");

            List<Area> areas = Area.ListAreas();
            if (areas.Count > 0)
            {
                foreach (var area in areas)
                {
                    Console.WriteLine($"ID: {area.Id}, Name: {area.Name}, Description: {area.Description}");
                }
            }
            else
            {
                Console.WriteLine("No areas found.");
            }

            Console.ReadKey();
        }

        static void UpdateArea()
        {
            Console.Clear();
            Console.WriteLine("Enter the ID of the area to update:");
            int areaId = int.Parse(Console.ReadLine());

            Area areaToUpdate = Area.Find(areaId);
            if (areaToUpdate != null)
            {
                Console.WriteLine("Enter new Area name:");
                string name = Console.ReadLine();
                Console.WriteLine("Enter new Area description:");
                string description = Console.ReadLine();

                var polygonCoordinates = new List<(double lat, double lng)>();
                Console.WriteLine("Enter new Polygon Points (type 'done' to finish):");
                while (true)
                {
                    Console.Write("Enter latitude: ");
                    string latInput = Console.ReadLine();
                    if (latInput.ToLower() == "done") break;
                    Console.Write("Enter longitude: ");
                    string lngInput = Console.ReadLine();
                    polygonCoordinates.Add((double.Parse(latInput), double.Parse(lngInput)));
                }

                Area.Update(areaId, name, description, polygonCoordinates);
                Console.WriteLine("Area updated successfully!");
            }
            else
            {
                Console.WriteLine("Area not found.");
            }

            Console.ReadKey();
        }

        static void DeleteArea()
        {
            Console.Clear();
            Console.WriteLine("Enter the ID of the area to delete:");
            int areaId = int.Parse(Console.ReadLine());

            Area areaToDelete = Area.Find(areaId);
            if (areaToDelete != null)
            {
                Area.Delete(areaId);
                Console.WriteLine("Area deleted successfully!");
            }
            else
            {
                Console.WriteLine("Area not found.");
            }

            Console.ReadKey();
        }
    }
}