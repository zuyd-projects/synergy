# Nature Pathfinder

Nature Pathfinder is a console-based application that calculates the shortest route between selected nature areas in Zuid Nederland (Southern Netherlands) using Dijkstra’s algorithm. This application is designed for users interested in finding optimal routes between natural landmarks, parks, and reserves.

🚀 Project Overview

This application uses a graph-based approach where each nature area is represented as a node, and the edges between nodes represent the distances between areas. The Haversine formula calculates the geographical distance between each pair of nature areas based on their centroid coordinates. Using Dijkstra’s algorithm, the application finds and displays the shortest path from a starting area to a destination area.

🗂 Folder Structure

The project is organized into distinct folders for models, helpers, and services, with each folder handling specific parts of the functionality:

NaturePathfinder

├── Models  
│   ├── Area.cs                - Defines the Area class representing each nature area  
│   └── Route.cs               - Defines the Route class representing paths between areas  
├── Helpers  
│   └── DistanceCalculator.cs   - Contains the Haversine formula for distance calculations  
├── Services  
│   ├── Graph.cs               - Manages graph creation and route management between areas  
│   └── Dijkstra.cs            - Contains Dijkstra's algorithm to calculate shortest paths  
└── Program.cs                 - Main entry point where areas are defined and user interaction occurs  

⚙️ Complexity and Big O Analysis

	1.	Graph Creation (BuildGraph in Program.cs):
	•	Complexity: O(N²), where N is the number of areas.
	•	Explanation: Each area is paired with every other area to calculate distances, resulting in ￼ calculations.

	2.	Distance Calculation (CalculateDistance in DistanceCalculator.cs):
	•	Complexity: O(1) (constant time for each pair).
	•	Explanation: The Haversine formula calculates the distance between two geographic points in constant time.

	3.	Dijkstra’s Algorithm (FindShortestPaths in Dijkstra.cs):
	•	Complexity: O((N + E) * log(N)), where N is the number of areas (nodes) and E is the number of routes (edges).
	•	Explanation: Each node and its edges are processed using a priority queue, which results in logarithmic complexity for insertions and deletions.

	4.	Path Reconstruction (GetShortestPath in Dijkstra.cs):
	•	Complexity: O(N), where N is the number of nodes in the path.
	•	Explanation: This method backtracks through the previous nodes to reconstruct the shortest path, visiting each node in the path once.

📝 Requirements

	•	.NET Core SDK (version 4.7.2)

🛠 Usage

	1.	Select Start and End Points:
	•	The application displays a list of all available nature areas. Enter the name of the area you want to start from, then enter the name of your destination area.
	2.	View Results:
	•	The program calculates the shortest path using Dijkstra’s algorithm and displays the route in the console.

Example Output

Available Areas:
- De Meinweg
- Groote Peel
- Maasduinen
- Sint-Pietersberg
- ...

Please enter the name of the starting area: Noorderbos
Please enter the name of the destination area: Sint-Pietersberg

Starting Dijkstra's algorithm from: Noorderbos
...
Shortest path from De Noorderbos to Sint-Pietersberg:
-> Noorderbos
-> Kempen-Broek
-> Sint-Pietersberg

📊 Algorithms and Formulas

	•	Dijkstra’s Algorithm: Finds the shortest path from a starting node to a destination node in a weighted graph.
	•	Haversine Formula: Calculates the great-circle distance between two points on the Earth’s surface, used for determining distances between nature areas.

📐 Assumptions and Limitations

	1.	Static Dataset:
	•	The current dataset is hardcoded with 15 nature areas and their approximate geographical centroids.
	2.	Undirected Graph:
	•	Routes are assumed to be bidirectional, meaning the path from Area A to Area B has the same distance as the path from Area B to Area A.
	3.	Geographical Accuracy:
	•	The distances between areas are approximate and based on centroid coordinates. This may not account for actual travel paths or obstacles.

📜 License

This project is licensed under the MIT License.