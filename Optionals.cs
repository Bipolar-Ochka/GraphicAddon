using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace GraphicAddon
{
    static class Optionals
    {
        
        internal static (List<LineShape> lines, List<Text> marks) getAxisLines(RectangleShape shape, Color color, float thickness, float xMax, float yMax, float step, float xMin = 0, float yMin = 0)
        {
            int offset = 60;
            int lenghtDiv = 10;
            float thicknessOfDiv = 2f;
            List<LineShape> lines = new List<LineShape>();
            int countForX = (int)((xMax - xMin) / step);
            int countForY = (int)((yMax - yMin) / step);
            Vector2f yLine = new Vector2f(shape.Position.X - thickness, shape.Position.Y - offset - thickness);
            Vector2f zeroLine = new Vector2f(shape.Position.X - thickness, shape.Position.Y + shape.Size.Y + thickness);
            Vector2f xLine = new Vector2f(shape.Position.X + shape.Size.X + offset - thickness, shape.Position.Y + shape.Size.Y + thickness);
            LineShape yAxis = new LineShape(yLine, zeroLine, thickness, color);
            LineShape xAxis = new LineShape(zeroLine, xLine, thickness, color);
            float stepOnX = (float)(xAxis.Length - offset + thickness) / countForX;
            float stepOnY = (float)(yAxis.Length - offset - thickness) / countForY;
            lines.Add(yAxis);
            lines.Add(xAxis);
            List<Text> numberMarks = new List<Text>();
            Font font = new Font($"{Environment.CurrentDirectory}\\tmr.ttf");
            uint fontSize = 13;
            for (int i = 0; i < countForX + 1; i++)
            {
                lines.Add(new LineShape(new Vector2f(zeroLine.X + (i * stepOnX), zeroLine.Y), new Vector2f(zeroLine.X + (i * stepOnX), zeroLine.Y + lenghtDiv), thicknessOfDiv, color));
                Text mark = new Text(Math.Round(xMin + i * step, 1).ToString(), font, fontSize);
                mark.Position = new Vector2f(zeroLine.X + (i * stepOnX), zeroLine.Y + lenghtDiv + fontSize / 2);
                mark.OutlineColor = color;
                mark.FillColor = color;
                numberMarks.Add(mark);
            }
            for (int i = 0; i < countForY + 1; i++)
            {
                lines.Add(new LineShape(new Vector2f(zeroLine.X, zeroLine.Y - (i * stepOnY)), new Vector2f(zeroLine.X - lenghtDiv, zeroLine.Y - (i * stepOnY)), thicknessOfDiv, color));
                Text mark = new Text(Math.Round(yMin + i * step, 1).ToString(), font, fontSize);
                mark.Position = new Vector2f(zeroLine.X - lenghtDiv - mark.DisplayedString.Length * fontSize / 2, zeroLine.Y - (i * stepOnY) - fontSize / 2 - thicknessOfDiv);
                mark.OutlineColor = color;
                mark.FillColor = color;
                numberMarks.Add(mark);
            }

            return (lines, numberMarks);
        }
    }
}
