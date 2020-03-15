using SFML.Graphics;
using SFML.System;
using System;

namespace GraphicAddon
{
    class LineShape : Shape
    {
        Vector2f direction;
        float _thickness;
        public float Thickness
        {
            get
            {
                return _thickness;
            }
            set
            {
                _thickness = value;
                Update();
            }
        }
        public double Length
        {
            get
            {
                return Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            }
        }
        public override Vector2f GetPoint(uint index)
        {
            Vector2f uintDir = direction / (float)Length;
            Vector2f uintPerp = new Vector2f(-uintDir.Y, uintDir.X);
            Vector2f offset = (Thickness / 2f) * uintPerp;
            switch (index)
            {
                default:
                case 0: return offset;
                case 1: return (direction + offset);
                case 2: return (direction - offset);
                case 3: return (-offset);
            }
        }

        public override uint GetPointCount()
        {
            return 4;
        }
        public LineShape(Vector2f point1, Vector2f point2)
        {
            direction = point2 - point1;
            Position = point1;
            Thickness = 2f;
            FillColor = Color.Black;
        }
        public LineShape(Vector2f point1, Vector2f point2, float thickness) : this(point1, point2)
        {
            Thickness = thickness;
        }
        public LineShape(Vector2f point1, Vector2f point2, Color color) : this(point1, point2)
        {
            FillColor = color;
        }
        public LineShape(Vector2f point1, Vector2f point2, float thickness, Color color) : this(point1, point2)
        {
            Thickness = thickness;
            FillColor = color;
        }


    }
}
