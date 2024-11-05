using System;
using System.Collections.Generic;
using System.Linq;
using NaturePathfinder.Models;

namespace NaturePathfinder.Services
{
    internal class Dijkstra
    {
        // Finds the shortest paths from startArea to all reachable areas in the graph
        public static (Dictionary<Area, double> distances, Dictionary<Area, Area> previousAreas) FindShortestPaths(Graph graph, Area startArea)
        {
            // Initialize data structures
            var distances = new Dictionary<Area, double>();
            var previousAreas = new Dictionary<Area, Area>();
            var priorityQueue = new SortedSet<(double distance, Area area)>();

            // Initialize distances and previous areas
            foreach (var area in graph.Areas)
            {
                // Set all distances to infinity and previous areas to null
                distances[area] = double.PositiveInfinity;
                previousAreas[area] = null;
            }
            // Set the distance of the start area to 0
            distances[startArea] = 0;
            // Add the start area to the priority queue
            priorityQueue.Add((0, startArea));
            Console.WriteLine($"Starting Dijkstra's algorithm from: {startArea.Name}");

            // Process the priority queue
            while (priorityQueue.Count > 0)
            {
                // Get the area with the smallest distance
                var (currentDistance, currentArea) = priorityQueue.Min;
                // Remove the area from the priority queue
                priorityQueue.Remove((currentDistance, currentArea));
                Console.WriteLine($"\\nProcessing area: {currentArea.Name}, Distance from start: {currentDistance}");

                // Iterate over the routes from the current area
                foreach (var route in graph.GetRoutesFromArea(currentArea))
                {
                    // Get the neighbor area and calculate the new distance
                    var neighbor = route.TargetArea;
                    var newDist = currentDistance + route.Weight;

                    // Update the distance and previous area if the new distance is smaller
                    if (newDist < distances[neighbor])
                    {
                        Console.WriteLine($"Updating distance of {neighbor.Name} from {distances[neighbor]} to {newDist}");
                        // Remove outdated entry if it exists
                        priorityQueue.Remove((distances[neighbor], neighbor)); // Remove outdated entry if it exists
                        
                        // Update the distance and previous area
                        distances[neighbor] = newDist;
                        previousAreas[neighbor] = currentArea;  // Add this line to update path tracking
                        priorityQueue.Add((newDist, neighbor));
                    }
                }
            }

            // Debug: Print the populated previousAreas for verification
            Console.WriteLine("Populated previousAreas dictionary:");
            foreach (var area in previousAreas)
            {
                if (area.Value != null)
                    Console.WriteLine($"{area.Key.Name} <- {area.Value.Name}");
                else
                    Console.WriteLine($"{area.Key.Name} has no previous area (start or disconnected)");
            }
            
            return (distances, previousAreas);
        }

        // Reconstructs the shortest path from startArea to endArea
        public static List<Area> GetShortestPath(Dictionary<Area, Area> previousAreas, Area startArea, Area endArea)
        {
            var path = new List<Area>();
            var currentArea = endArea;
            Console.WriteLine($"\nReconstructing path from {endArea.Name} back to {startArea.Name}:");

            // Traverse from endArea back to startArea, collecting all nodes
            while (currentArea != null)
            {
                path.Add(currentArea);
                Console.WriteLine($" -> {currentArea.Name}");
        
                // Move to the previous area in the path
                currentArea = previousAreas.ContainsKey(currentArea) ? previousAreas[currentArea] : null;
            }

            // Reverse the path to get the correct order from start to end
            path.Reverse();

            // Verify if the path starts with startArea
            if (path.First() != startArea)
            {
                Console.WriteLine("No path found from start to end area.");
                return new List<Area>(); // Return empty list if no valid path
            }
    
            // Print the final path for verification
            Console.WriteLine("Final path from start to end area:");
            foreach (var area in path)
            {
                Console.WriteLine($" -> {area.Name}");
            }
            return path;
        }
    }
}