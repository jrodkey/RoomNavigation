using RoomNavigation.Structure;
using RoomNavigationRefactored.Direction;
using System.Collections.Generic;

namespace RoomNavigation.Graph
{
    /// <summary>
    /// Defines a directed graph of nodes.
    /// </summary>
    public class NodeGraph
    {
        private Node m_root;

        public NodeGraph()
        {
            m_root = new Node();
        }

        /// <summary>
        /// Adds a new connection to the graph by traversing through nodes starting at root.
        /// </summary>
        /// <param name="direction">Target direction to face when added</param>
        /// <param name="connection">Coonection node</param>
        /// <param name="parentName">Name of parent</param>
        /// <returns>True, if added successfully, otherwise null</returns>
        public bool AddNewConnection(CardinalDirection direction, Node connection, string parentName = "Unlisted")
        {
            if (connection == null || string.IsNullOrEmpty(parentName))
            {
                return false;
            }

            Queue<Node> nodes = new Queue<Node>();
            return m_root.TraverseAddNeighbor(nodes, direction, parentName, connection);
        }

        /// <summary>
        /// Determines if any path exists between the starting room and an ending room with
        /// a given name.
        /// </summary>
        /// <param name="endingRoomName">Target room name</param>
        /// <returns>True, if path exists, otherwise false</returns>
        public bool PathExistsTo(string endingRoomName)
        {
            return m_root.PathExistsTo(endingRoomName);
        }
    }

    /// <summary>
    /// Defines a result and the path of nodes.
    /// </summary>
    public class PathResponse
    {
        private List<Node> m_roomFound;

        public PathResponse()
        {
            m_roomFound = new List<Node>();
        }

        public List<Node> NodesFound 
        { 
            get { return m_roomFound; }
            set { m_roomFound = value; }
        }

        public bool Result { get;  set; }
    }
}