using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.Models
{
    public class PointOfInterest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }

        public PointOfInterest(int id, string name, string type, float longitude, float latitude)
        {
            Id = id;
            Name = name;
            Type = type;
            Longitude = longitude;
            Latitude = latitude;
        }

        // Methode om de afstand tussen een gebruiker en dit PointOfInterest te berekenen
        public double CalculateDistance(float userLatitude, float userLongitude)
        {
            double R = 6371e3; // Radius van de aarde in meters
            double phi1 = Latitude * Math.PI / 180;
            double phi2 = userLatitude * Math.PI / 180;
            double deltaPhi = (userLatitude - Latitude) * Math.PI / 180;
            double deltaLambda = (userLongitude - Longitude) * Math.PI / 180;

            double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                       Math.Cos(phi1) * Math.Cos(phi2) *
                       Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // Afstand in meters
        }

        // Methode om te controleren of de gebruiker binnen de straal is en een notificatie te tonen
        public string CheckProximity(float userLatitude, float userLongitude, double radius, string userName)
        {
            double distance = CalculateDistance(userLatitude, userLongitude);
            if (distance <= radius)
            {
                return $"Notificatie: {userName}, je bent binnen {radius} meter van {Name}!";
            }
            return null; // Geeft null terug als de gebruiker buiten de radius is
        }

    }
}
