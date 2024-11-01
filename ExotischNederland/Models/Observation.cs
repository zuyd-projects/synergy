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
        public int Id { get; private set; }
        public User User { get; set; }
        public Specie Specie { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }

        public Observation(Dictionary<string, object> _values)
        {
            this.Id = Convert.ToInt32(_values["Id"]);
            
            // Handle User as an ID
            if (_values.ContainsKey("UserId"))
            {
                // Load User object by UserId
                this.User = User.Find(Convert.ToInt32(_values["UserId"]));
            }
            else
            {
                this.User = null;
            }

            // Handle Specie as an ID
            if (_values.ContainsKey("SpecieId"))
            {
                // Load Specie object by SpecieId
                this.Specie = Specie.Find(Convert.ToInt32(_values["SpecieId"]));
            }
            else
            {
                this.Specie = null;
            }

            this.Longitude = Convert.ToSingle(_values["Longitude"]);
            this.Latitude = Convert.ToSingle(_values["Latitude"]);
            this.Description = _values["Description"]?.ToString();
            this.PhotoUrl = _values["PhotoUrl"]?.ToString();
        }

        // Method to create a new observation
        public static Observation Create(Specie _specie, double _longitude, double _latitude, string _description, string _photoUrl, User _user)
        {
            if (!_user.Permission.CanCreateObservation()) return null;

            SQLDAL sql = SQLDAL.Instance;
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "SpecieId", _specie.Id },  // Store the Specie's ID in the database
                { "Longitude", _longitude },
                { "Latitude", _latitude },
                { "Description", _description },
                { "PhotoUrl", _photoUrl },
                { "UserId", _user.Id }  // Store the User's ID in the database
            };

            int id = sql.Insert("Observation", values);
            Console.WriteLine($"Observation created with ID: {id}");
            return Find(id);
        }

        // Method to update an observation
        public void Update(User _authenticatedUser)
        {
            if (!_authenticatedUser.Permission.CanEditObservation(this)) return;

            SQLDAL sql = SQLDAL.Instance;
            Dictionary<string, object> values = new Dictionary<string, object>
            {
            { "SpecieId", this.Specie.Id },  // Store the Specie's ID
            { "Longitude", this.Longitude },
            { "Latitude", this.Latitude },
            { "Description", this.Description },
            { "PhotoUrl", this.PhotoUrl },
            { "UserId", this.User.Id }  // Store the User's ID
            };

            sql.Update("Observation", this.Id, values);
            Console.WriteLine($"Observation with ID: {this.Id} updated.");
        }

        // Method to delete an observation
        public void Delete(User _authenticatedUser)
        {
            if (!_authenticatedUser.Permission.CanDeleteObservation(this)) return;

            SQLDAL sql = SQLDAL.Instance;
            sql.Delete("Observation", this.Id);
        }
        public static Observation Find(int _id)
        {
            SQLDAL sql = SQLDAL.Instance;
            return sql.Find<Observation>("Id", _id.ToString());
        }
        // Method to get all observations
        public static List<Observation> GetAll()
        {
            SQLDAL sql = SQLDAL.Instance;
            List<Observation> observations = sql.Select<Observation>();

            return observations;
        }
    }
}