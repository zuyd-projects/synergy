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
            // Initialize the distances and previous areas dictionaries
            var distances = new Dictionary<Area, double>();
            var previousAreas = new Dictionary<Area, Area>();
            var priorityQueue = new SortedSet<(double distance, Area area)>();
            // Initialize the distances and previous areas dictionaries
            foreach (var area in graph.Areas)
            {
                // Set the distance to infinity and the previous area to null
                distances[area] = double.PositiveInfinity;
                // Set the distance to infinity and the previous area to null
                previousAreas[area] = null;
            }
            // Set the distance of the start area to 0
            distances[startArea] = 0;
            // Add the start area to the priority queue
            priorityQueue.Add((0, startArea));
            // Print the start area
            Console.WriteLine($"Starting Dijkstra's algorithm from: {startArea.Name}");
            // Process the priority queue
            while (priorityQueue.Count > 0)
            {
                // Get the area with the smallest distance
                var (currentDistance, currentArea) = priorityQueue.Min;
                // Remove the area from the priority queue
                priorityQueue.Remove((currentDistance, currentArea));
                // Print the current area
                Console.WriteLine($"\nProcessing area: {currentArea.Name}, Distance from start: {currentDistance}");
                // Process the routes from the current area
                foreach (var route in graph.GetRoutesFromArea(currentArea))
                {
                    // Get the neighbor area
                    var neighbor = route.TargetArea;
                    // Calculate the new distance
                    var newDist = currentDistance + route.Weight;
                    // Print the neighbor area
                    if (newDist < distances[neighbor])
                    {
                        // Print the updated distance
                        Console.WriteLine($"Updating distance of {neighbor.Name} from {distances[neighbor]} to {newDist}");
                        // Update the distance and previous area
                        priorityQueue.Remove((distances[neighbor], neighbor));
                        // Update the distance and previous area
                        distances[neighbor] = newDist;
                        // Update the distance and previous area
                        previousAreas[neighbor] = currentArea;
                        // Add the neighbor area to the priority queue
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
            // Reconstruct the path
            for (var at = endArea; at != null; at = previousAreas[at])
            {
                // Add the area to the path
                path.Add(at);
            }
            // Reverse the path
            path.Reverse();
            // Return the path
            return path.First() == startArea ? path : new List<Area>();
        }
    }
}