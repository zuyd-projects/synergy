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

        // Constructor that takes a dictionary to initialize the Specie fields
        public Specie(Dictionary<string, object> _values)
        {
            this.Id = (int)_values["Id"];
            this.Name = (string)_values["Name"];
            this.Category = (string)_values["Category"];
            this.Observations = new List<Observation>();
        }

        // Static method to find or create a new Specie
        public static Specie FindOrCreate(string _name, string _category)
        {
            SQLDAL sql = new SQLDAL();
            
            // Try to find the specie first
            Specie specie = sql.Find<Specie>("Name", _name) ?? Create(_name, _category);
            return specie;  // Return the found or created specie
        }

        // Static method to create a new Specie
        public static Specie Create(string _name, string _category)
        {
            SQLDAL sql = new SQLDAL();
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "Name", _name },
                { "Category", _category }
            };

            int id = sql.Insert("Specie", values);
            Console.WriteLine($"Specie created with ID: {id}");
            return Find(id);
        }

        // Static method to update a Specie
        public void Update()
        {
            SQLDAL sql = new SQLDAL();
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "Name", this.Name },
                { "Category", this.Category }
            };

            sql.Update("Specie", this.Id, values);
            // TODO: Remove this line from the final code
            Console.WriteLine($"Specie with ID: {this.Id} updated.");
        }

        // Static method to delete a Specie
        public void Delete()
        {
            SQLDAL sql = new SQLDAL();
            sql.Delete("Specie", this.Id);
            // TODO: Remove this line from the final code
            Console.WriteLine($"Specie with ID: {this.Id} deleted.");
        }

        public static Specie Find(int _id)
        {
            SQLDAL sql = new SQLDAL();
            return sql.Find<Specie>("Id", _id.ToString());
        }
    }
}