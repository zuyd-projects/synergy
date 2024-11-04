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
            // Print the start of the algorithm
            Console.WriteLine($"Starting Dijkstra's algorithm from: {startArea.Name}");
            // Process the priority queue
            while (priorityQueue.Count > 0)
            {
                // Get the area with the smallest distance
                var (currentDistance, currentArea) = priorityQueue.Min;
                // Remove the area from the priority queue
                priorityQueue.Remove((currentDistance, currentArea));
                // Print the current area and distance
                Console.WriteLine($"\nProcessing area: {currentArea.Name}, Distance from start: {currentDistance}");
                // Iterate over the routes from the current area
                foreach (var route in graph.GetRoutesFromArea(currentArea))
                {
                    // Get the neighbor area and calculate the new distance
                    var neighbor = route.TargetArea;
                    // Skip the neighbor if it has already been visited
                    var newDist = currentDistance + route.Weight;
                    // Update the distance and previous area if the new distance is smaller
                    if (newDist < distances[neighbor])
                    {
                        // Print the updated distance
                        Console.WriteLine($"Updating distance of {neighbor.Name} from {distances[neighbor]} to {newDist}");
                        // Remove outdated entry if it exists
                        priorityQueue.Remove((distances[neighbor], neighbor)); // Remove outdated entry if it exists
                        // Update the distance and previous area
                        distances[neighbor] = newDist;
                        // Add the neighbor to the priority queue
                        previousAreas[neighbor] = currentArea;
                        // Add the neighbor to the priority queue
                        priorityQueue.Add((newDist, neighbor));
                    }
                }
            }
            // Return the distances and previous areas
            return (distances, previousAreas);
        }
        
        // Reconstructs the shortest path from startArea to endArea
        public static List<Area> GetShortestPath(Dictionary<Area, Area> previousAreas, Area startArea, Area endArea)
        {
            // Initialize the path
            var path = new List<Area>();
            // Start from the end area
            var currentArea = endArea;
            // Print the start of the path reconstruction
            Console.WriteLine($"\nReconstructing path from {endArea.Name} back to {startArea.Name}:");
            // Reconstruct the path
            while (currentArea != null)
            {
                // Add the current area to the path
                path.Add(currentArea);
                // Print the current area
                Console.WriteLine($" -> {currentArea.Name}");
                // Move to the previous area
                currentArea = previousAreas[currentArea];
            }
            // Reverse the path
            path.Reverse();
            // Return an empty list if no path exists
            if (path.First() != startArea)
            {
                // Print a message if no path exists
                Console.WriteLine("No path found from start to end area.");
                // Return an empty list if no path exists
                return new List<Area>(); // Return an empty list if no path exists
            }
            // Return the path
            return path;
        }
    }
}