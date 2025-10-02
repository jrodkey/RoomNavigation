
using RoomNavigation.Graph;
using RoomNavigation.Structure.Models;
using RoomNavigationRefactored.Direction;
using RoomNavigationRefactored.Graph.Extensions;
using System;
using System.Collections.Generic;

namespace RoomNavigation.Structure
{
    /// <summary>
    /// Defines a graph node with a structure and a dictionary of neighbors. 
    /// </summary>
    public class Node
    {
        private float m_localCost;
        private float m_globalCost;
        private Node m_parent;
        private BasicRoom m_roomStructure;
        private Dictionary<CardinalDirection, List<Node>> m_pathNeighborsMap;

        public Node() : this(null, new BasicRoom("Unlisted"))
        { }

        public Node(BasicRoom structure) : this(null, structure)
        { }

        public Node(Node parent, BasicRoom structure)
        {
            m_globalCost = 0;
            m_localCost = 0;
            m_parent = parent;
            m_roomStructure = structure;
            m_pathNeighborsMap = new Dictionary<CardinalDirection, List<Node>>
            {
                {CardinalDirection.North, new List<Node>()},
                {CardinalDirection.South, new List<Node>()},
                {CardinalDirection.East, new List<Node>()},
                {CardinalDirection.West, new List<Node>()},
            };
        }

        public void Dispose()
        {
            m_roomStructure = null;
            foreach (KeyValuePair<CardinalDirection, List<Node>> pair in m_pathNeighborsMap)
            {
                pair.Value.Clear();
            }
            m_pathNeighborsMap = null;
        }

        public string Name
        {
            get { return m_roomStructure.GetName(); }
        }

        public BasicRoom Room
        {
            get { return m_roomStructure; }
            set { m_roomStructure = value; }
        }

        public List<Node> NorthNeighbors
        {
            get { return m_pathNeighborsMap[CardinalDirection.North]; }
        }

        public List<Node> SouthNeighbors
        {
            get { return m_pathNeighborsMap[CardinalDirection.South]; }
        }

        public List<Node> EastNeighbors
        {
            get { return m_pathNeighborsMap[CardinalDirection.East]; }
        }

        public List<Node> WestNeighbors
        {
            get { return m_pathNeighborsMap[CardinalDirection.West]; }
        }

        /// <summary>
        /// Gets a gathered list of nodes for each direction.
        /// </summary>
        /// <returns>List of nodes</returns>
        public List<Node> GetAllNeighbors()
        {
            List<Node> allNeighbors = new List<Node>();
            allNeighbors.AddRange(NorthNeighbors);
            allNeighbors.AddRange(SouthNeighbors);
            allNeighbors.AddRange(EastNeighbors);
            allNeighbors.AddRange(WestNeighbors);
            return allNeighbors;
        }

        /// <summary>
        /// Checks each list of neighbors at their direction and sees if any one 
        /// of them exist.
        /// </summary>
        /// <returns></returns>
        public bool HasNeighbors()
        {
            return (NorthNeighbors.Count > 0 || SouthNeighbors.Count > 0 ||
                    EastNeighbors.Count > 0 || WestNeighbors.Count > 0);
        }

        /// <summary>
        /// Adds the node to the given direction, if it hasn't been added.
        /// </summary>
        /// <param name="direction">Target direction</param>
        /// <param name="node">Target node</param>
        public void AddNeighbor(CardinalDirection direction, Node node)
        {
            if (!m_pathNeighborsMap[direction].Contains(node))
            {
                m_pathNeighborsMap[direction].Add(node);
                node.PlaceTowards(direction);
            }
        }

        /// <summary>
        /// Removes the node from the given direction, if possible.
        /// </summary>
        /// <param name="direction">Target direction</param>
        /// <param name="node">Target node</param>
        public void RemoveNeighbor(CardinalDirection direction, Node node)
        {
            if (m_pathNeighborsMap[direction].Contains(node))
            {
                m_pathNeighborsMap[direction].Remove(node);
            }
        }

        /// <summary>
        /// Determines if a path exists between the current room and an ending room with
        /// a given name.
        /// </summary>
        /// <param name="endingRoomName">Target room name</param>
        /// <returns>True, if path exists, otherwise false</returns>
        public bool PathExistsTo(string endingRoomName)
        {
            Queue<Node> nodes = new Queue<Node>();
            PathResponse response = new PathResponse();
            TraverseFindNeighbor(new List<string>(), endingRoomName, response);
            //TraverseFindNeighborBFS(nodes, new List<string>(), endingRoomName, response);
            if (response.Result)
            {
                string newPath = response.NodesFound[0].Name;
                for (int i = 1; i < response.NodesFound.Count; ++i)
                {
                    Node node = response.NodesFound[i];
                    newPath += "->" + node.Name;
                }
                Console.WriteLine("New Path: " + newPath);
            }

            return response.Result;
        }

        /// <summary>
        /// Determines if a path exists between the current room and an ending room with
        /// a given name.
        /// </summary>
        /// <param name="endingRoom">Target room</param>
        /// <returns>True, if path exists, otherwise false</returns>
        public bool PathExistsTo(Node endingRoom)
        {
            Queue<Node> nodes = new Queue<Node>();
            PathResponse response = new PathResponse();

            m_localCost = 0;
            m_globalCost = this.DistanceTo(endingRoom);

            TraverseFindNeighborAStar(nodes, new List<string>(), endingRoom, response);
            if (response.Result)
            {
                string newPath = response.NodesFound[0].Name;
                for (int i = 1; i < response.NodesFound.Count; ++i)
                {
                    Node node = response.NodesFound[i];
                    newPath += "->" + node.Name;
                }
                Console.WriteLine("New Path: " + newPath);
            }

            return response.Result;
        }

        /// <summary>
        /// Recursively traverses through the neighbors and adds the connection to the parent, once found.
        /// </summary>
        /// <param name="nodes">Composed queue of nodes</param>
        /// <param name="direction">Target direction</param>
        /// <param name="targetNodeName">Target name of the parent node</param>
        /// <param name="connection">Connection node</param>
        /// <returns>True, if successful, otherwise false</returns>
        public bool TraverseAddNeighbor(Queue<Node> nodes, CardinalDirection direction, string targetNodeName, Node connection)
        {
            // If this node's name is equal to target node, add the connection and return true.
            if (targetNodeName.Equals(Name))
            {
                connection.Room.SetLocation(this.Room.GetLocation());
                AddNeighbor(direction, connection);
                return true;
            }

            // Queue up all of the neighbors that exists and start to dequeue them.
            foreach (KeyValuePair<CardinalDirection, List<Node>> kvp in m_pathNeighborsMap)
            {
                foreach (Node neighbor in kvp.Value)
                {
                    nodes.Enqueue(neighbor);
                }
            }

            Node node = nodes.Dequeue();
            while (node != null)
            {
                if (node.TraverseAddNeighbor(nodes, direction, targetNodeName, connection))
                {
                    return true;
                }

                node = nodes.Dequeue();
            }

            return false;
        }

        /// <summary>
        /// Recursively traverses through the neighbors until we find the given node.
        /// </summary>
        /// <param name="seenNames">List of node names that we've visited</param>
        /// <param name="targetNodeName">Target name</param>
        /// <param name="response">PathResponse result</param>
        public void TraverseFindNeighbor(List<string> seenNames, string targetNodeName, PathResponse response)
        {
            // If this node's name is equal to target node, set the responses result as true and return.
            if (targetNodeName.Equals(Name))
            {
                response.NodesFound.Insert(0, this);
                response.Result = true;
                return;
            }

            // Otherwise, add the name as visited and recurse through the neighbors.
            if (!seenNames.Contains(Name))
            {
                seenNames.Add(Name);
                foreach (KeyValuePair<CardinalDirection, List<Node>> kvp in m_pathNeighborsMap)
                {
                    foreach (Node neighbor in kvp.Value)
                    {
                        neighbor.TraverseFindNeighbor(seenNames, targetNodeName, response);
                        if (response.Result)
                        {
                            response.NodesFound.Insert(0, this);
                            return;
                        }
                    }
                }
            }

            response.Result = false;
            return;
        }

        /// <summary>
        /// Recursively traverses through the neighbors until we find the given node.
        /// </summary>
        /// <param name="seenNames">List of node names that we've visited</param>
        /// <param name="targetNodeName">Target name</param>
        /// <param name="response">PathResponse result</param>
        public void TraverseFindNeighborBFS(Queue<Node> nodes, List<string> seenNames, string targetNodeName, PathResponse response)
        {
            // If this node's name is equal to target node, set the responses result as true and return.
            if (targetNodeName.Equals(Name))
            {
                response.NodesFound.Insert(0, this);
                response.Result = true;
                return;
            }

            // Otherwise, add the name as visited, gather up the remaining nodes and recurse through the neighbors.
            if (!seenNames.Contains(Name))
            {
                seenNames.Add(Name);

                foreach (KeyValuePair<CardinalDirection, List<Node>> kvp in m_pathNeighborsMap)
                {
                    foreach (Node neighbor in kvp.Value)
                    {
                        nodes.Enqueue(neighbor);
                    }
                }

                Node node = nodes.Dequeue();
                while (node != null)
                {
                    node.TraverseFindNeighborBFS(nodes, seenNames, targetNodeName, response);
                    if (response.Result)
                    {
                        response.NodesFound.Insert(0, this);
                        return;
                    }

                    node = nodes.Dequeue();
                }
            }

            response.Result = false;
            return;
        }

        /// <summary>
        /// Recursively traverses through the neighbors until we find the given node.
        /// </summary>
        /// <param name="seenNames">List of node names that we've visited</param>
        /// <param name="targetNodeName">Target name</param>
        /// <param name="response">PathResponse result</param>
        public void TraverseFindNeighborAStar(Queue<Node> nodes, List<string> seenNames, Node targetNode, PathResponse response)
        {
            // If this node's name is equal to target node, set the responses result as true and return.
            if (targetNode.Name.Equals(Name))
            {
                response.NodesFound.Insert(0, this);
                response.Result = true;
                return;
            }

            m_localCost = m_parent.m_localCost + m_localCost;

            // Otherwise, add the name as visited and recurse through the neighbors.
            if (!seenNames.Contains(Name))
            {
                seenNames.Add(Name);
                foreach (KeyValuePair<CardinalDirection, List<Node>> kvp in m_pathNeighborsMap)
                {
                    foreach (Node neighbor in kvp.Value)
                    {
                        neighbor.TraverseFindNeighbor(seenNames, targetNodeName, response);
                        if (response.Result)
                        {
                            response.NodesFound.Insert(0, this);
                            return;
                        }
                    }
                }
            }

            response.Result = false;
            return;
        }
    }
}