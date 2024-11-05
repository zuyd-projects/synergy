using System.Collections.Generic;
using System.Linq;
using NaturePathfinder.Models;

namespace NaturePathfinder.Services
{
    internal class Graph
    {
        // Adjacency list representation of the graph
        private Dictionary<Area, List<Route>> _adjacencyList;
        // Constructor
        public Graph(List<Area> areas)
        {
            // Initialize the adjacency list
            _adjacencyList = areas.ToDictionary(area => area, area => new List<Route>());
        }
        // Add a route to the graph
        public void AddRoute(Area fromArea, Route route)
        {
            // Add the route to the adjacency list
            _adjacencyList[fromArea].Add(route);
        }
        // Get the routes from an area
        public List<Route> GetRoutesFromArea(Area area)
        {
            // Return the routes from the adjacency list
            return _adjacencyList.ContainsKey(area) ? _adjacencyList[area] : new List<Route>();
        }

        // Get the areas that are connected to an area
        public IEnumerable<Area> Areas => _adjacencyList.Keys;
    }
}