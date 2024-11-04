using System.Collections.Generic;
using System.Linq;

namespace ExotischNederland.Models
{
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
}