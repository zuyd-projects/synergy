using System;
using System.Collections.Generic;
using System.Linq;

namespace ExotischNederland
{
    internal class Area
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<(double lat, double lng)> PolygonCoordinates { get; set; }

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
            double latSum = 0, lngSum = 0;
            int numPoints = PolygonCoordinates.Count;

            foreach (var point in PolygonCoordinates)
            {
                latSum += point.lat;
                lngSum += point.lng;
            }

            return (latSum / numPoints, lngSum / numPoints);
        }
    }

    internal class Route
    {
        public Area TargetArea { get; set; }
        public double Weight { get; set; } // Distance or time

        public Route(Area targetArea, double weight)
        {
            TargetArea = targetArea;
            Weight = weight;
        }
    }

    internal class Graph
    {
        private Dictionary<Area, List<Route>> _adjacencyList;

        public Graph(List<Area> areas)
        {
            _adjacencyList = areas.ToDictionary(area => area, area => new List<Route>());
        }

        public void AddRoute(Area fromArea, Route route)
        {
            _adjacencyList[fromArea].Add(route);
        }

        public List<Route> GetRoutesFromArea(Area area)
        {
            return _adjacencyList.ContainsKey(area) ? _adjacencyList[area] : new List<Route>();
        }

        public IEnumerable<Area> Areas => _adjacencyList.Keys;
    }

    internal class Dijkstra
    {
        // Runs Dijkstra's algorithm to find shortest paths from startArea
        // Complexity: O((N + E) * log(N)) due to priority queue operations
        public static (Dictionary<Area, double> distances, Dictionary<Area, Area> previousAreas) FindShortestPaths(Graph graph, Area startArea)
        {
            var distances = new Dictionary<Area, double>();
            var previousAreas = new Dictionary<Area, Area>();
            var priorityQueue = new SortedSet<(double distance, Area area)>();

            // Initialize distances and previous areas
            foreach (var area in graph.Areas)
            {
                distances[area] = double.PositiveInfinity;
                previousAreas[area] = null;
            }

            // Set the distance to the start area to 0 and add it to the queue
            distances[startArea] = 0;
            priorityQueue.Add((0, startArea));

            Console.WriteLine($"Starting Dijkstra's algorithm from: {startArea.Name}");

            while (priorityQueue.Count > 0)
            {
                // Extract the node with the smallest tentative distance
                var (currentDistance, currentArea) = priorityQueue.Min;
                priorityQueue.Remove((currentDistance, currentArea));
                
                Console.WriteLine($"\nProcessing area: {currentArea.Name}, Distance from start: {currentDistance}");

                // Examine each neighbor of the current node
                foreach (var route in graph.GetRoutesFromArea(currentArea))
                {
                    var neighbor = route.TargetArea;
                    var newDist = currentDistance + route.Weight;

                    // If a shorter path to the neighbor is found, update it
                    if (newDist < distances[neighbor])
                    {
                        Console.WriteLine($"Updating distance of {neighbor.Name} from {distances[neighbor]} to {newDist}");
                        priorityQueue.Remove((distances[neighbor], neighbor)); // Remove outdated entry if it exists
                        distances[neighbor] = newDist;
                        previousAreas[neighbor] = currentArea;
                        priorityQueue.Add((newDist, neighbor));
                    }
                }
            }

            return (distances, previousAreas);
        }

        // Reconstructs the shortest path from startArea to endArea
        // Complexity: O(N), where N is the number of areas in the path
        public static List<Area> GetShortestPath(Dictionary<Area, Area> previousAreas, Area startArea, Area endArea)
        {
            var path = new List<Area>();
            for (var at = endArea; at != null; at = previousAreas[at])
            {
                path.Add(at);
            }
            path.Reverse();

            if (path.First() == startArea)
                Console.WriteLine("\nShortest path reconstructed successfully.");
            else
                Console.WriteLine("\nNo path found from start to end area.");

            return path;
        }
    }

    internal class Program
    {
        // Calculates the geographical distance between two points
        // Complexity: O(1)
        public static double CalculateDistance((double lat, double lng) point1, (double lat, double lng) point2)
        {
            double R = 6371; // Earth radius in kilometers
            double dLat = (point2.lat - point1.lat) * Math.PI / 180.0;
            double dLng = (point2.lng - point1.lng) * Math.PI / 180.0;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(point1.lat * Math.PI / 180.0) * Math.Cos(point2.lat * Math.PI / 180.0) *
                       Math.Sin(dLng / 2) * Math.Sin(dLng / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        internal static Graph BuildGraph(List<Area> areas)
        {
            var graph = new Graph(areas);

            foreach (var area in areas)
            {
                var centroid1 = area.CalculateCentroid();

                foreach (var otherArea in areas)
                {
                    if (area == otherArea) continue;

                    var centroid2 = otherArea.CalculateCentroid();
                    double distance = CalculateDistance(centroid1, centroid2);

                    // Adding bidirectional route for this example
                    graph.AddRoute(area, new Route(otherArea, distance));
                    Console.WriteLine($"Added route from {area.Name} to {otherArea.Name} with distance: {distance}");
                }
            }

            return graph;
        }

        internal static void Main()
        {
            // Hardcoded nature areas
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
            // Select start and end areas
            Area startArea = areas.First(a => a.Name == "De Meinweg");
            Area endArea = areas.First(a => a.Name == "Grensmaasvallei");

            // Run Dijkstra's algorithm
            var (distances, previousAreas) = Dijkstra.FindShortestPaths(graph, startArea);

            // Reconstruct and display shortest path
            var shortestPath = Dijkstra.GetShortestPath(previousAreas, startArea, endArea);

            Console.WriteLine($"\nShortest path from {startArea.Name} to {endArea.Name}:");
            foreach (var area in shortestPath)
            {
                Console.WriteLine($" -> {area.Name}");
            }
        }
    }
}