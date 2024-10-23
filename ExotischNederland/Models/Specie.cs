using System;
using System.Collections.Generic;
using ExotischNederland.DAL;

namespace ExotischNederland.Models
{
    internal class Specie
    {
        private int id;
        private string name;
        private string category;
        private List<Observation> observations;

        private string tablename = "Specie";

        // Constructor to initialize Specie with id, name, and category
        public Specie(int id, string name, string category)
        {
            this.id = id;
            this.name = name;
            this.category = category;
            this.observations = new List<Observation>();  // Initialize the observations list
        }

        // Constructor that takes a dictionary to initialize the Specie fields
        public Specie(Dictionary<string, object> values)
        {
            this.id = (int)values["Id"];
            this.name = (string)values["Name"];
            this.category = (string)values["Category"];
            this.observations = new List<Observation>();  // Initialize the observations list
        }

        // Properties to access the fields
        public int Id => id;
        public string Name => name;
        public string Category => category;
        public List<Observation> Observations => observations;

        // Static method to find or create a new Specie
        public static Specie FindOrCreate(string name, string category)
        {
            SQLDAL sql = new SQLDAL();
            
            // Try to find the specie first
            Specie specie = sql.Find<Specie>("name", name);
            
            if (specie == null)
            {
                // If the specie doesn't exist, create it
                Create(name, category);
                specie = sql.Find<Specie>("name", name);  // Fetch the newly created specie
            }

            return specie;  // Return the found or created specie
        }

        // Static method to create a new Specie
        public static void Create(string name, string category)
        {
            SQLDAL sql = new SQLDAL();
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "name", name },
                { "category", category }
            };

            int id = sql.Insert("Specie", values);
            Console.WriteLine($"Specie created with ID: {id}");
        }

        // Static method to update a Specie
        public static void Update(int id, string name, string category)
        {
            SQLDAL sql = new SQLDAL();
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "name", name },
                { "category", category }
            };

            sql.Update("Specie", id, values);
            Console.WriteLine($"Specie with ID: {id} updated.");
        }

        // Static method to delete a Specie
        public static void Delete(int id)
        {
            SQLDAL sql = new SQLDAL();
            sql.Delete("Specie", id);
            Console.WriteLine($"Specie with ID: {id} deleted.");
        }
    }
}