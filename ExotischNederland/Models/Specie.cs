using System;
using System.Collections.Generic;
using ExotischNederland.DAL;

namespace ExotischNederland.Models
{
    internal class Specie
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public List<Observation> Observations { get; set; }

        // Constructor to initialize Specie with id, name, and category
        public Specie(int id, string name, string category)
        {
            this.Id = id;
            this.Name = name;
            this.Category = category;
            this.Observations = new List<Observation>();
        }

        // Constructor that takes a dictionary to initialize the Specie fields
        public Specie(Dictionary<string, object> values)
        {
            this.Id = (int)values["Id"];
            this.Name = (string)values["Name"];
            this.Category = (string)values["Category"];
            this.Observations = new List<Observation>();
        }

        // Static method to find or create a new Specie
        public static Specie FindOrCreate(string name, string category)
        {
            SQLDAL sql = new SQLDAL();
            
            // Try to find the specie first
            Specie specie = sql.Find<Specie>("Name", name);
            
            if (specie == null)
            {
                // If the specie doesn't exist, create it
                Create(name, category);
                specie = sql.Find<Specie>("Name", name);  // Fetch the newly created specie
            }

            return specie;  // Return the found or created specie
        }

        // Static method to create a new Specie
        public static void Create(string name, string category)
        {
            SQLDAL sql = new SQLDAL();
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "Name", name },
                { "Category", category }
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
                { "Name", name },
                { "Category", category }
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

        public static Specie Find(int id)
        {
            SQLDAL sql = new SQLDAL();
            return sql.Find<Specie>("Id", id.ToString());
        }
    }
}