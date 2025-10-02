using System.Drawing;

namespace RoomNavigation.Structure.Models
{
    /// <summary>
    /// Defines a very basic room.
    /// </summary>
    public class BasicRoom : Structure
    {
        public BasicRoom() :
            this(string.Empty)
        { }

        public BasicRoom(string name) :
            this(0, 0, name)
        { }

        public BasicRoom(int x, int y, string name)
        {
            m_name = name;
            m_location = new Point(x, y);
            Width = 10;
            Height = 10;
        }

        public override int Width { get; set; }

        public override int Height { get; set; }
    }
}