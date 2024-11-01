using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotischNederland.Models
{
    internal class Route
    {
        // Alleen-lezen eigenschap voor Id
        public int Id { get; private set; }

        // Andere eigenschappen van Route kunnen hier worden toegevoegd
        // Bijvoorbeeld:
        // public string Name { get; set; }
        // public List<RoutePoint> Points { get; set; }

        // Constructor om Id in te stellen bij het aanmaken van een nieuw Route-object
        public Route(int id)
        {
            Id = id;
        }
    }

}
