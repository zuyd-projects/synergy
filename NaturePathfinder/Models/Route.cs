namespace NaturePathfinder.Models
{
    internal class Route
    {
        // Target area of the route
        public Area TargetArea { get; set; }
        // Weight of the route
        public double Weight { get; set; } // Distance or time
        // Constructor
        public Route(Area targetArea, double weight)
        {
            // Initialize the target area and weight
            TargetArea = targetArea;
            // Initialize the target area and weight
            Weight = weight;
        }
    }
}