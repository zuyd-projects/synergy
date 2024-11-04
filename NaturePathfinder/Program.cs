using System;
using System.Collections.Generic;
using System.Linq;
using NaturePathfinder.Models;
using NaturePathfinder.Helpers;
using NaturePathfinder.Services;

namespace NaturePathfinder
{
    internal class Program
    {
        // Helper method to build a graph of areas with routes between them based on the centroids
        internal static Graph BuildGraph(List<Area> areas)
        {
            // Create a graph with the areas as vertices
            var graph = new Graph(areas);
            // Add routes between each pair of areas based on the distance between their centroids
            foreach (var area in areas)
            {
                // Calculate the centroid of the current area
                var centroid1 = area.CalculateCentroid();
                // Calculate the distance between the centroid of the current area and the centroid of each other area
                foreach (var otherArea in areas)
                {
                    // Skip if the other area is the same as the current area
                    if (area == otherArea) continue;
                    // Calculate the centroid of the other area
                    var centroid2 = otherArea.CalculateCentroid();
                    // Calculate the distance between the centroids of the two areas
                    double distance = DistanceCalculator.CalculateDistance(centroid1, centroid2);
                    // Add a route between the two areas with the calculated distance
                    graph.AddRoute(area, new Route(otherArea, distance));
                    // Display the added route
                    Console.WriteLine($"Added route from {area.Name} to {otherArea.Name} with distance: {distance}");
                }
            }
            // Return the built graph
            return graph;
        }

        // Helper method to prompt the user for area selection
        private static Area GetAreaSelection(List<Area> areas, string prompt)
        {
            while (true)
            {
                Console.WriteLine("\nAvailable Areas:");
                foreach (var area in areas)
                {
                    Console.WriteLine($"- {area.Name}");
                }

                Console.Write($"{prompt} ");
                string userInput = Console.ReadLine()?.Trim();

                var selectedArea = areas.FirstOrDefault(a => a.Name.Equals(userInput, StringComparison.OrdinalIgnoreCase));
                if (selectedArea != null)
                {
                    return selectedArea;
                }

                Console.WriteLine("Invalid selection. Please enter a valid area name from the list.");
            }
        }

        internal static void Main()
        {
            List<Area> areas = new List<Area>
            {
                new Area(1, "De Meinweg", "National Park", new List<(double lat, double lng)> { (51.1387, 6.0797) }),
                new Area(2, "Groote Peel", "National Park", new List<(double lat, double lng)> { (51.3153, 5.8392) }),
                new Area(3, "Maasduinen", "National Park", new List<(double lat, double lng)> { (51.5638, 6.1425) }),
                new Area(4, "Sint-Pietersberg", "Nature Reserve", new List<(double lat, double lng)> { (50.8382, 5.6884) }),
                new Area(5, "Brunssummerheide", "Nature Area", new List<(double lat, double lng)> { (50.9073, 5.9733) }),
                new Area(6, "De Peel", "Nature Area", new List<(double lat, double lng)> { (51.4132, 5.8375) }),
                new Area(7, "Schinveld Forest", "Nature Reserve", new List<(double lat, double lng)> { (50.9694, 5.9873) }),
                new Area(8, "Eijsder Beemden", "Nature Area", new List<(double lat, double lng)> { (50.7951, 5.7093) }),
                new Area(9, "Weerterbos", "Forest", new List<(double lat, double lng)> { (51.2536, 5.7431) }),
                new Area(10, "Kempen-Broek", "Cross-border Nature Park", new List<(double lat, double lng)> { (51.1967, 5.7737) }),
                new Area(11, "De Banen", "Nature Reserve", new List<(double lat, double lng)> { (51.3320, 5.8690) }),
                new Area(12, "Sint-Jansberg", "Nature Area", new List<(double lat, double lng)> { (51.7376, 5.9458) }),
                new Area(13, "Vijlenerbos", "Forest", new List<(double lat, double lng)> { (50.7767, 5.9498) }),
                new Area(14, "De Luysen", "Nature Reserve", new List<(double lat, double lng)> { (51.2250, 5.6417) }),
                new Area(15, "Grensmaasvallei", "River Valley Nature Reserve", new List<(double lat, double lng)> { (50.9833, 5.7890) }),
            };

            Graph graph = BuildGraph(areas);

            // Prompt user to select the start and end areas
            Area startArea = GetAreaSelection(areas, "Please enter the name of the starting area:");
            Area endArea = GetAreaSelection(areas, "Please enter the name of the destination area:");

            // Run Dijkstra's algorithm
            var (distances, previousAreas) = Dijkstra.FindShortestPaths(graph, startArea);
            var shortestPath = Dijkstra.GetShortestPath(previousAreas, startArea, endArea);

            // Display the shortest path
            Console.WriteLine($"\nShortest path from {startArea.Name} to {endArea.Name}:");
            foreach (var area in shortestPath)
            {
                Console.WriteLine($" -> {area.Name}");
            }
        }
    }
}