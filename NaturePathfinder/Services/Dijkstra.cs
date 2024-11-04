using System;
using System.Collections.Generic;

namespace ExotischNederland.Models
{
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
}