using System.Collections.Generic;

namespace NaturePathfinder.Models
{
    internal class Area
    {
        // Properties
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<(double lat, double lng)> PolygonCoordinates { get; set; }

        // Constructor
        public Area(int id, string name, string description, List<(double lat, double lng)> polygonCoordinates)
        {
            Id = id;
            Name = name;
            Description = description;
            PolygonCoordinates = polygonCoordinates;
        }

        // Calculates the centroid (center) of the polygon representing the area
        // Complexity: O(N), where N is the number of points in the polygon
        public (double lat, double lng) CalculateCentroid()
        {
            // Calculate the average latitude and longitude
            double latSum = 0, lngSum = 0;
            int numPoints = PolygonCoordinates.Count;
            // Calculate the sum of latitudes and longitudes
            foreach (var point in PolygonCoordinates)
            {
                latSum += point.lat;
                lngSum += point.lng;
            }
            // Return the centroid
            return (latSum / numPoints, lngSum / numPoints);
        }
    }
}