using System;
using System.Collections.Generic;
using ExotischNederland.Models;
using System.Linq;
using ExotischNederland.DAL;
using System.IO;

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
                        ObservationMenu(authenticatedUser);  // Enter observation menu after login
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

        // Observation Menu for CRUD actions (Create, Read, Update, Delete) 
        // TODO: @RICK limit the actions based on the user's role when roles are implemented
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
                    // Create a new observation
                    CreateObservation(user);
                }
                else if (selected == "2")
                {
                    // View all observations for the user
                    ViewObservations(user);
                }
                else if (selected == "3")
                {
                    // Update an observation
                    UpdateObservation(user);
                }
                else if (selected == "4")
                {
                    // Delete an observation
                    DeleteObservation(user);
                }
                else if (selected == "5")
                {
                    // Log out
                    Console.Clear();
                    break;
                }
            }
        }

        static void CreateObservation(User user)
        {
            Console.Clear();
            Console.WriteLine("Creating a new observation...");

            // Ask for specie details
            Console.WriteLine("Enter Specie name:");
            string specieName = Console.ReadLine();
            Console.WriteLine("Enter Specie category:");
            string specieCategory = Console.ReadLine();

            // Call the Specie class to find or create the specie
            Specie specie = Specie.FindOrCreate(specieName, specieCategory);

            // Ask for observation details
            Console.WriteLine("Enter Longitude:");
            float longitude = float.Parse(Console.ReadLine());
            Console.WriteLine("Enter Latitude:");
            float latitude = float.Parse(Console.ReadLine());
            Console.WriteLine("Enter Description:");
            string description = Console.ReadLine();
            Console.WriteLine("Enter Photo URL:");
            string photoUrl = Console.ReadLine();

            // Create the observation using the found or created specie
            Observation.Create(specie, longitude, latitude, description, photoUrl, user);
            Console.WriteLine("Observation created!");
            Console.ReadKey();
        }

        // Method to view observations for the authenticated user
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

        // Method to update an observation
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

                // Update the properties of the observation
                observationToUpdate.Specie = specie;
                observationToUpdate.Longitude = longitude;
                observationToUpdate.Latitude = latitude;
                observationToUpdate.Description = description;
                observationToUpdate.PhotoUrl = photoUrl;

                // Call the Update method on the instance
                observationToUpdate.Update();
                Console.WriteLine("Observation updated!");
            }
            else
            {
                Console.WriteLine("Observation not found.");
            }

            Console.ReadKey();
        }

        // Method to delete an observation
        static void DeleteObservation(User user)
        {
            Console.Clear();
            Console.WriteLine("Enter the ID of the observation to delete:");
            int observationId = int.Parse(Console.ReadLine());

            List<Observation> observations = user.GetObservations();
            Observation observationToDelete = observations.FirstOrDefault(o => o.Id == observationId);

            if (observationToDelete != null)
            {
                // Call the Delete method on the instance
                observationToDelete.Delete();
                Console.WriteLine("Observation deleted!");
            }
            else
            {
                Console.WriteLine("Observation not found.");
            }

            Console.ReadKey();
        }
        //export observations to csv
        

        public static void ExportObservationsToCsv(string filePath)
        {
            List<Observation> observations = GetAllObservations();

            using (StreamWriter writer = new StreamWriter(filePath))
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
        }

        public static List<Observation> GetAllObservations()
        {
            SQLDAL sql = new SQLDAL();
            return sql.Select<Observation>();
        }
    }
}
    
