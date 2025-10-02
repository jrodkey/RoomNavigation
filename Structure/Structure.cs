using System.Drawing;

namespace RoomNavigation.Structure
{
    /// <summary>
    /// Defines a structure that houses the basic idea.
    /// </summary>
    public abstract class Structure
    {
        protected Point m_location = new Point();
        protected string m_name = string.Empty;

        public virtual int Width { get; set; }

        public virtual int Height { get; set; }

        public Point GetLocation()
        {
            return m_location;
        }

        public void SetLocation(Point location)
        {
            m_location = location;
        }

        public string GetName()
        {
            return m_name;
        }
    }
}