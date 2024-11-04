using System;
namespace NaturePathfinder.Helpers
{
    internal static class DistanceCalculator
    {
        // Calculates the geographical distance between two points using the Haversine formula
        public static double CalculateDistance((double lat, double lng) point1, (double lat, double lng) point2)
        {
            double R = 6371; // Earth radius in kilometers
            double dLat = (point2.lat - point1.lat) * Math.PI / 180.0; // Convert to radians
            double dLng = (point2.lng - point1.lng) * Math.PI / 180.0; // Convert to radians
            // Calculate the distance using the Haversine formula (https://en.wikipedia.org/wiki/Haversine_formula)
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + 
                       Math.Cos(point1.lat * Math.PI / 180.0) * Math.Cos(point2.lat * Math.PI / 180.0) *
                       Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)); // Calculate the central angle
            return R * c; // Distance in kilometers
        }
    }
}