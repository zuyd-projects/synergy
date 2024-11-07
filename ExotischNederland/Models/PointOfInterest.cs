using ExotischNederland.DAL;
using System;
using System.Collections.Generic;

namespace ExotischNederland.Models
{
    internal class PointOfInterest
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Type { get; private set; }
        public float Longitude { get; private set; }
        public float Latitude { get; private set; }

        public PointOfInterest(Dictionary<string, object> _values)
        {
            this.Id = Convert.ToInt32(_values["Id"]);
            this.Name = Convert.ToString(_values["Name"]);
            this.Description = Convert.ToString(_values["Description"]);
            this.Type = Convert.ToString(_values["Type"]);
            this.Longitude = Convert.ToSingle(_values["Longitude"]);
            this.Latitude = Convert.ToSingle(_values["Latitude"]);
        }

        public static PointOfInterest Create(string _name, string _description, string _type, float _longitude, float _latitude)
        {
            SQLDAL sql = SQLDAL.Instance;
            var values = new Dictionary<string, object>
            {
                { "Name", _name },
                { "Description", _description },
                { "Type", _type },
                { "Longitude", _longitude },
                { "Latitude", _latitude }
            };
            int id = sql.Insert("PointOfInterest", values);
            values["Id"] = id; // Add the generated Id to the values dictionary
            return new PointOfInterest(values);
        }

        public static PointOfInterest Find(int _id)
        {
            SQLDAL sql = SQLDAL.Instance;
            return sql.Find<PointOfInterest>("Id", _id.ToString());
        }

        // Update method to modify an existing PointOfInterest
        public void Update(string newName, string newDescription, string newType, float newLongitude, float newLatitude)
        {
            Name = newName;
            Description = newDescription;
            Type = newType;
            Longitude = newLongitude;
            Latitude = newLatitude;

            SQLDAL sql = SQLDAL.Instance;
            var values = new Dictionary<string, object>
            {
                { "Name", newName },
                { "Description", newDescription },
                { "Type", newType },
                { "Longitude", newLongitude },
                { "Latitude", newLatitude }
            };

            sql.Update("PointOfInterest", this.Id, values);
        }

        // Delete method to remove an existing PointOfInterest
        public static void Delete(int poiId)
        {
            SQLDAL sql = SQLDAL.Instance;
            sql.Delete("PointOfInterest", poiId);
        }
        public static List<PointOfInterest> GetAll()
        {
            SQLDAL sql = SQLDAL.Instance;
            return sql.Select<PointOfInterest>();
        }
    }
}