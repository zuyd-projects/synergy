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
        private int id;
        private User user;  // Foreign key to the User table
        private Specie specie; // Foreign key to the Specie table
        private float longitude;
        private float latitude;
        private string description;
        private string photoUrl;

        public Observation(Dictionary<string, object> values)
        {
            this.id = Convert.ToInt32(values["Id"]);

            // Handle User as an ID or full object
            if (values.ContainsKey("User") && values["User"] is Dictionary<string, object> userDict)
            {
                this.user = new User(userDict);
            }
            else if (values.ContainsKey("UserId"))
            {
                // Load User object by UserId
                this.user = new User(new Dictionary<string, object> { { "Id", Convert.ToInt32(values["UserId"]) } });
            }
            else
            {
                this.user = null;
            }

            // Handle Specie as an ID or full object
            if (values.ContainsKey("Specie") && values["Specie"] is Dictionary<string, object> specieDict)
            {
                this.specie = new Specie(specieDict);
            }
            else if (values.ContainsKey("SpecieId"))
            {
                // Load Specie object by SpecieId
                this.specie = new Specie(new Dictionary<string, object> { { "Id", Convert.ToInt32(values["SpecieId"]) } });
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

        // Properties to access the fields
        public int Id => id;
        public User User => user;  
        public Specie Specie => specie;  
        public float Longitude => longitude;
        public float Latitude => latitude;
        public string Description => description;
        public string PhotoUrl => photoUrl;

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

        //FIXME: Method to update an observation
        public static void Update(int id, Specie specie, float longitude, float latitude, string description, string photoUrl, User user)
        {
            SQLDAL sql = new SQLDAL();
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "SpecieId", specie.Id },  // Store the Specie's ID
                { "Longitude", longitude },
                { "Latitude", latitude },
                { "Description", description },
                { "PhotoUrl", photoUrl },
                { "UserId", user.Id }  // Store the User's ID
            };

            sql.Update("Observation", id, values);
            Console.WriteLine($"Observation with ID: {id} updated.");
        }

        //FIXME: Method to delete an observation *SHOULD ONLY BE FOR BEHEERDERS
        public static void Delete(int id)
        {
            SQLDAL sql = new SQLDAL();
            sql.Delete("Observation", id);
            Console.WriteLine($"Observation with ID: {id} deleted.");
        }

        //FIXME Retrieve all observations for a particular user or specie
        public List<Observation> GetObservations(User user)
        {
            SQLDAL sql = new SQLDAL();
            var observations = sql.Select<Observation>("Observation")
                .Where(o => o.User != null && o.User.Id == user.Id)  // Filter by the user's ID
                .ToList();
            return observations;
        }
    }
}