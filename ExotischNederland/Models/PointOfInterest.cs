using ExotischNederland.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.Models
{
    internal class PointOfInterest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }

        public PointOfInterest(Dictionary<string, object> _values)
        {
            Id = (int)_values["Id"];
            Name = (string)_values["Name"];
            Type = (string)_values["Type"];
            Longitude = (float)_values["Longitude"];
            Latitude = (float)_values["Latitude"];
        }

        public static PointOfInterest Create(string _name, string _type, float _longitude, float _latitude)
        {
            SQLDAL sql = SQLDAL.Instance;
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "Name", _name },
                { "Type", _type },
                { "Longitude", _longitude },
                { "Latitude", _latitude }
            };

            int id = sql.Insert("PointOfInterest", values);
            return Find(id);
        }

        public static PointOfInterest Find(int _id)
        {
            SQLDAL sql = SQLDAL.Instance;
            return sql.Find<PointOfInterest>("Id", _id.ToString());
        }

        // Update an existing PointOfInterest
        public static void Update(int _id, string _name, string _type, float _longitude, float _latitude)
        {
            SQLDAL sql = SQLDAL.Instance;
            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "Name", _name },
                { "Type", _type },
                { "Longitude", _longitude },
                { "Latitude", _latitude }
            };
            sql.Update("PointOfInterest", _id, values);
        }

        // Delete a PointOfInterest by Id
        public static void Delete(int _id)
        {
            SQLDAL sql = SQLDAL.Instance;
            sql.Delete("PointOfInterest", _id);
        }

        // Methode om de afstand tussen een gebruiker en dit PointOfInterest te berekenen
        public double CalculateDistance(float _userLatitude, float _userLongitude)
        {
            double R = 6371e3; // Radius van de aarde in meters
            double phi1 = Latitude * Math.PI / 180;
            double phi2 = _userLatitude * Math.PI / 180;
            double deltaPhi = (_userLatitude - Latitude) * Math.PI / 180;
            double deltaLambda = (_userLongitude - Longitude) * Math.PI / 180;

            double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                       Math.Cos(phi1) * Math.Cos(phi2) *
                       Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // Afstand in meters
        }

        // Methode om alle Points of Interest op te halen uit de database
        public static List<PointOfInterest> GetAll()
        {
            SQLDAL sql = SQLDAL.Instance;
            return sql.Select<PointOfInterest>();
        }

        // Methode om nabijheid van gebruiker te controleren en notificatie te genereren
        internal static List<string> CheckProximityForUser(User _user, double _radius)
        {
            List<string> notifications = new List<string>();
            List<PointOfInterest> pointsOfInterest = GetAll();

            foreach (var poi in pointsOfInterest)
            {
                double distance = poi.CalculateDistance(_user.CurrentLatitude, _user.CurrentLongitude);
                if (distance <= _radius)
                {
                    notifications.Add($"Notificatie: {_user.Name}, je bent binnen {_radius} meter van {poi.Name}!");
                }
            }

            return notifications;
        }
    }
}
