
using RoomNavigation.Structure;
using RoomNavigationRefactored.Direction;
using System;
using System.Drawing;

namespace RoomNavigationRefactored.Graph.Extensions
{
    public static class NodeExtension
    {
        /// <summary>
        /// Adjust the node's location depending on its direction.
        /// </summary>
        /// <param name="node">Source node</param>
        /// <param name="cardinalDirection">Direction for adjustment</param>
        public static void PlaceTowards(this Node node, CardinalDirection cardinalDirection)
        {
            Point temp = node.Room.GetLocation();
            switch (cardinalDirection)
            {
                case CardinalDirection.North:
                    node.Room.SetLocation(new System.Drawing.Point(temp.X + node.Room.Height, temp.Y));
                    break;
                case CardinalDirection.South:
                    node.Room.SetLocation(new System.Drawing.Point(temp.X - node.Room.Height, temp.Y));
                    break;
                case CardinalDirection.East:
                    node.Room.SetLocation(new System.Drawing.Point(temp.X, temp.Y + node.Room.Height));
                    break;
                case CardinalDirection.West:
                    node.Room.SetLocation(new System.Drawing.Point(temp.X, temp.Y - node.Room.Height));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Distance to the target node.
        /// </summary>
        /// <param name="node">Source node</param>
        /// <param name="target">Target node</param>
        /// <returns>Calculated distance</returns>
        public static float DistanceTo(this Node node, Node target)
        {
            return (float)Math.Sqrt((node.Room.GetLocation().X - target.Room.GetLocation().X) * (node.Room.GetLocation().X - target.Room.GetLocation().X)
                    + ((node.Room.GetLocation().Y - target.Room.GetLocation().Y) - (node.Room.GetLocation().Y - target.Room.GetLocation().Y)));
        }
    }
}
