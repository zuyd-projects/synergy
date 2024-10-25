using ExotischNederland.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.Models
{
    internal class Observation
    {
        private readonly int id;
        private User user;  // Foreign key to the User table
        private Specie specie; // Foreign key to the Specie table
        private float longitude;
        private float latitude;
        private string description;
        private string photoUrl;

        public Observation(Dictionary<string, object> values)
        {
            this.id = Convert.ToInt32(values["Id"]);
            
            // Handle User as an ID
            if (values.ContainsKey("UserId"))
            {
                // Load User object by UserId
                this.user = User.Find(Convert.ToInt32(values["UserId"]));
            }
            else
            {
                this.user = null;
            }

            // Handle Specie as an ID
            if (values.ContainsKey("SpecieId"))
            {
                // Load Specie object by SpecieId
                this.specie = Specie.Find(Convert.ToInt32(values["SpecieId"]));
            }
            else
            {
                this.specie = null;
            }

            this.longitude = Convert.ToSingle(values["Longitude"]);
            this.latitude = Convert.ToSingle(values["Latitude"]);
            this.description = values["Description"]?.ToString();
            this.photoUrl = values["PhotoUrl"]?.ToString();
        }

        // Properties to access the fields or set them
        public int Id => id;
        public User User
        {
            get => user;
            set => user = value;
        }
        public Specie Specie
        {
            get => specie;
            set => specie = value;
        }
        public float Longitude
        {
            get => longitude;
            set => longitude = value;
        }
        public float Latitude
        {
            get => latitude;
            set => latitude = value;
        }
        public string Description
        {
            get => description;
            set => description = value;
        }
        public string PhotoUrl
        {
            get => photoUrl;
            set => photoUrl = value;
        }

        // Method to create a new observation
        public static void Create(Specie specie, float longitude, float latitude, string description, string photoUrl, User user)
        {
            SQLDAL sql = new SQLDAL();
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "SpecieId", specie.Id },  // Store the Specie's ID in the database
                { "Longitude", longitude },
                { "Latitude", latitude },
                { "Description", description },
                { "PhotoUrl", photoUrl },
                { "UserId", user.Id }  // Store the User's ID in the database
            };

            int id = sql.Insert("Observation", values);
            Console.WriteLine($"Observation created with ID: {id}");
        }

        // Method to update an observation
        public void Update()
        {
            SQLDAL sql = new SQLDAL();
            Dictionary<string, object> values = new Dictionary<string, object>
            {
            { "SpecieId", this.specie.Id },  // Store the Specie's ID
            { "Longitude", this.longitude },
            { "Latitude", this.latitude },
            { "Description", this.description },
            { "PhotoUrl", this.photoUrl },
            { "UserId", this.user.Id }  // Store the User's ID
            };

            sql.Update("Observation", this.id, values);
            Console.WriteLine($"Observation with ID: {this.id} updated.");
        }

        // Method to delete an observation
        public void Delete()
        {
            SQLDAL sql = new SQLDAL();
            sql.Delete("Observation", this.id);
            Console.WriteLine($"Observation with ID: {this.id} deleted.");
        }
    }
}