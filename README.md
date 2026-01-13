# Room Navigation

A C# console application that implements a graph-based room navigation system with pathfinding capabilities. This project demonstrates efficient graph traversal and path-finding algorithms in a spatial context.

## Overview

This application creates a network of interconnected rooms represented as a directed graph structure. Each room can have connections in cardinal directions (North, South, East, West) to other rooms. The system can determine if a path exists between any two rooms in the network.

## Features

- **Graph-Based Navigation**: Rooms are represented as nodes in a directed graph
- **Cardinal Direction System**: Rooms connect via North, South, East, and West directions
- **Path Finding**: Efficiently determines if a path exists between any two rooms
- **Performance Testing**: Includes benchmarking capabilities to measure pathfinding performance
- **Scalable Architecture**: Can handle large networks with thousands of rooms

## Technical Details

### Key Components

- **NodeGraph**: The main graph structure that manages the network of rooms
- **Node**: Represents individual rooms and their connections
- **BasicRoom**: Concrete implementation of a room with location and name
- **CardinalDirection**: Enumeration for directional connections between rooms

### Algorithms

The application uses graph traversal algorithms to:
- Add new connections between rooms
- Find paths between any two rooms in the network
- Traverse the graph efficiently using queue-based approaches

## Requirements

- .NET Framework 4.5 or higher
- Visual Studio 2017 or later (recommended)

## Building the Project

1. Open the solution file `RoomNavigationRefactored.sln` in Visual Studio
2. Build the solution using `Ctrl+Shift+B` or through the Build menu
3. The executable will be generated in `bin/Debug/net45/`

Alternatively, use the .NET CLI:

```bash
dotnet build RoomNavigationRefactored.sln
```

## Running the Application

Execute the compiled application:

```bash
cd bin/Debug/net45
RoomNavigationRefactored.exe
```

The program will:
1. Generate a large network of 10,000 interconnected rooms
2. Perform pathfinding tests to various target rooms
3. Display performance metrics for each pathfinding operation

## Usage Example

The application demonstrates pathfinding by:

```csharp
// Create a network with 10,000 rooms
NodeGraph graph = CreateGenericNetwork(10000, out Node node);

// Test if a path exists to a specific room
bool pathExists = graph.PathExistsTo("room100");
```

## Performance

The application includes built-in performance benchmarking that measures:
- Time taken to find paths to various rooms
- Scalability with large networks (tested with 10,000+ rooms)
- Multiple pathfinding operations with timing metrics
