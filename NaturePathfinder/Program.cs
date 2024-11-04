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
            // Create a graph with the areas
            var graph = new Graph(areas);
            // Create routes between all pairs of areas
            foreach (var area in areas) 
            {
                // Calculate the centroid of the area
                var centroid1 = area.CalculateCentroid();
                // Create routes to all other areas
                foreach (var otherArea in areas)
                {
                    // Skip if the areas are the same
                    if (area == otherArea) continue;
                    // Calculate the centroid of the other area
                    var centroid2 = otherArea.CalculateCentroid();
                    // Calculate the distance between the centroids
                    double distance = DistanceCalculator.CalculateDistance(centroid1, centroid2);
                    distance += new Random().NextDouble() * 0.5; // Adds a small random factor to vary the distances

                    // Create bidirectional routes to ensure connectivity
                    graph.AddRoute(area, new Route(otherArea, distance));
                    graph.AddRoute(otherArea, new Route(area, distance));
                    // Output the added route
                    Console.WriteLine($"Added route between {area.Name} and {otherArea.Name} with distance {distance:F2} km");
                }
            }

            return graph;
        }

        // Helper method to prompt the user for area selection
        private static Area GetAreaSelection(List<Area> areas, string prompt)
        {
            // Loop until a valid area is selected
            while (true)
            {
                // Display the available areas
                Console.WriteLine("\nAvailable Areas:");
                // Display the names of the areas
                foreach (var area in areas)
                {
                    // Display the area name
                    Console.WriteLine($"- {area.Name}");
                }
                // Prompt the user to select an area
                Console.Write($"{prompt} ");
                // Read the user input
                string userInput = Console.ReadLine()?.Trim();
                // Find the selected area by name
                var selectedArea = areas.FirstOrDefault(a => a.Name.Equals(userInput, StringComparison.OrdinalIgnoreCase));
                // Return the selected area if found
                if (selectedArea != null)
                {
                    return selectedArea;
                }
                // Display an error message if the area is not found
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
                new Area(16, "Leudal", "Nature Reserve", new List<(double lat, double lng)> { (51.2500, 5.9500) }),
                new Area(17, "De Krang", "Nature Area", new List<(double lat, double lng)> { (51.2324, 5.8475) }),
                new Area(18, "Limburgse Kempen", "Nature Park", new List<(double lat, double lng)> { (51.1671, 5.4898) }),
                new Area(19, "Nationaal Park De Zoom - Kalmthoutse Heide", "National Park", new List<(double lat, double lng)> { (51.4005, 4.4696) }),
                new Area(20, "Ravenvennen", "Nature Reserve", new List<(double lat, double lng)> { (51.4211, 6.2056) }),
                new Area(21, "Walem", "Nature Reserve", new List<(double lat, double lng)> { (50.8616, 5.8891) }),
                new Area(22, "Kaldenbroek", "Nature Area", new List<(double lat, double lng)> { (51.4323, 6.1308) }),
                new Area(23, "Beegderheide", "Heathland", new List<(double lat, double lng)> { (51.2168, 5.8872) }),
                new Area(24, "Itteren en Borgharen", "Floodplain Area", new List<(double lat, double lng)> { (50.8858, 5.6746) }),
                new Area(25, "Oude Maasarm", "Nature Area", new List<(double lat, double lng)> { (50.9375, 5.7278) }),
                new Area(26, "Stramprooierbroek", "Nature Reserve", new List<(double lat, double lng)> { (51.1472, 5.6875) }),
                new Area(27, "Grenspark Maas-Swalm-Nette", "Cross-border Nature Park", new List<(double lat, double lng)> { (51.2039, 6.1769) }),
                new Area(28, "Stevensweert en Ohé", "Nature Reserve", new List<(double lat, double lng)> { (51.1256, 5.8569) }),
                new Area(29, "Mookerheide", "Nature Reserve", new List<(double lat, double lng)> { (51.7572, 5.8813) }),
                new Area(30, "Landgoed de Hamert", "Nature Reserve", new List<(double lat, double lng)> { (51.5214, 6.1612) }),
                new Area(31, "Noorderbos", "Nature Reserve", new List<(double lat, double lng)> { (53.0000, 6.0000) })
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
            // Display the path
            foreach (var area in shortestPath)
            {
                Console.WriteLine($" -> {area.Name}");
            }
        }
    }
}