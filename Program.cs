using System;
using System.Collections.Generic;
using System.Diagnostics;
using RoomNavigation.Graph;
using RoomNavigation.Structure;
using RoomNavigation.Structure.Models;
using RoomNavigationRefactored.Direction;

namespace RoomNavigation
{
    public class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            NodeGraph graph = CreateGenericNetwork(10000, out Node node);

            Stopwatch stopwatch = Stopwatch.StartNew();
            graph.PathExistsTo("room" + 100);
            stopwatch.Stop();

            Console.WriteLine(string.Format("Path Finding: {0:00}:{1:00}:{2:00}:{3:00}", stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds, stopwatch.ElapsedMilliseconds / 10));

            stopwatch = Stopwatch.StartNew();
            graph.PathExistsTo("room" + 200);
            stopwatch.Stop();

            Console.WriteLine(string.Format("Path Finding: {0:00}:{1:00}:{2:00}:{3:00}", stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds, stopwatch.ElapsedMilliseconds / 10));

            stopwatch = Stopwatch.StartNew();
            graph.PathExistsTo("room" + 300);
            stopwatch.Stop();

            Console.WriteLine(string.Format("Path Finding: {0:00}:{1:00}:{2:00}:{3:00}", stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds, stopwatch.ElapsedMilliseconds / 10));

            stopwatch = Stopwatch.StartNew();
            graph.PathExistsTo("room" + 500);
            stopwatch.Stop();

            Console.WriteLine(string.Format("Path Finding: {0:00}:{1:00}:{2:00}:{3:00}", stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds, stopwatch.ElapsedMilliseconds / 10));

            stopwatch = Stopwatch.StartNew();
            graph.PathExistsTo("room" + 700);
            stopwatch.Stop();

            Console.WriteLine(string.Format("Path Finding: {0:00}:{1:00}:{2:00}:{3:00}", stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds, stopwatch.ElapsedMilliseconds / 10));

            stopwatch = Stopwatch.StartNew();
            graph.PathExistsTo("room" + 900);
            stopwatch.Stop();

            Console.WriteLine(string.Format("Path Finding: {0:00}:{1:00}:{2:00}:{3:00}", stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds, stopwatch.ElapsedMilliseconds / 10));

            //KITestCase();
        }

        private static NodeGraph CreateGenericNetwork(int count, out Node randomNode)
        {
            List<Node> nodes = new List<Node>();
            NodeGraph graph = new NodeGraph();
            randomNode = new Node();

            Node roomN = new Node(new BasicRoom("StartNorth"));
            Node roomS = new Node(new BasicRoom("StartSouth"));
            Node roomE = new Node(new BasicRoom("StartEast"));
            Node roomW = new Node(new BasicRoom("StartWest"));
            graph.AddNewConnection(CardinalDirection.North, roomN);
            graph.AddNewConnection(CardinalDirection.South, roomS);
            graph.AddNewConnection(CardinalDirection.East, roomE);
            graph.AddNewConnection(CardinalDirection.West, roomW);

            nodes.Add(roomN);
            nodes.Add(roomS);
            nodes.Add(roomE);
            nodes.Add(roomW);

            int i = 0;
            int roomCnt = 0;
            int totalCnt = count - 4;
            Random random = new Random(DateTime.Now.Millisecond);
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (i++ < totalCnt)
            {
                if (i == totalCnt || nodes.Count == 0)
                {
                    break;
                }

                Node node = nodes[0];
                nodes.RemoveAt(0);
                int l = 1;
                int numDirections = random.Next(1, 4);
                while (l <= numDirections)
                {
                    CardinalDirection direction = (CardinalDirection)Enum.ToObject(typeof(CardinalDirection), l);
                    Node neighbor = new Node(node, new BasicRoom(node.Room.GetLocation().X, node.Room.GetLocation().Y, "room" + roomCnt++));
                    node.AddNeighbor(direction, neighbor);
                    nodes.Add(neighbor);
                    l++;
                }
            }
            stopwatch.Stop();
            Console.WriteLine(string.Format("Graph Creation Time: {0:00}:{1:00}:{2:00}:{3:00}", stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds, stopwatch.ElapsedMilliseconds / 10));
            randomNode = nodes[random.Next(0, nodes.Count - 1)];
            return graph;
        }

        private static void KITestCase()
        {
            Console.WriteLine(new string('*', 20));

            // Question 3:
            Console.WriteLine("Question 3:");

            //
            // I believe this is how to use your code.
            // Hopefully I set this test up correctly.
            //

            // Test map:
            // 
            //           B -> C
            //           ^    ^
            //           |    v
            // I -> F <- A    D <-
            // ^    |         |  |
            // |    v         v  |
            // H <- G         E ->

            NodeGraph kiGraph = new NodeGraph();

            Node roomA = new Node(new BasicRoom("A"));
            Node roomB = new Node(new BasicRoom("B"));
            Node roomC = new Node(new BasicRoom("C"));
            Node roomD = new Node(new BasicRoom("D"));
            Node roomE = new Node(new BasicRoom("E"));
            Node roomF = new Node(new BasicRoom("F"));
            Node roomG = new Node(new BasicRoom("G"));
            Node roomH = new Node(new BasicRoom("H"));
            Node roomI = new Node(new BasicRoom("I"));

            kiGraph.AddNewConnection(CardinalDirection.North, roomA);
            kiGraph.AddNewConnection(CardinalDirection.North, roomB, "A");
            kiGraph.AddNewConnection(CardinalDirection.West, roomF, "A");
            kiGraph.AddNewConnection(CardinalDirection.East, roomC, "B");
            kiGraph.AddNewConnection(CardinalDirection.South, roomD, "C");
            kiGraph.AddNewConnection(CardinalDirection.North, roomC, "D");
            kiGraph.AddNewConnection(CardinalDirection.South, roomE, "D");
            kiGraph.AddNewConnection(CardinalDirection.East, roomD, "E");
            kiGraph.AddNewConnection(CardinalDirection.South, roomG, "F");
            kiGraph.AddNewConnection(CardinalDirection.West, roomH, "G");
            kiGraph.AddNewConnection(CardinalDirection.North, roomI, "H");
            kiGraph.AddNewConnection(CardinalDirection.East, roomF, "I");

            Console.WriteLine($"Path from A to C: {kiGraph.PathExistsTo("C")}"); // True

            // NOTE: We are looking for something like this:
            Console.WriteLine($"Path from A to C: {roomA.PathExistsTo("C")}"); // True

            Console.WriteLine(new string('*', 20));
        }
    }
}
