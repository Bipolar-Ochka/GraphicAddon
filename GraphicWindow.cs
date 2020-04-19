using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using SFML.Graphics;
using SFML.Window;
using System.Diagnostics;

namespace GraphicAddon
{
    public class GraphicWindow
    {
        private float zoom = 1f;
        private bool moving = false;
        private Vector2f oldPos = new Vector2f();
        private List<Drawable> toDraw = new List<Drawable>();
        private readonly float LineThickness = 2f;
        private uint rectangleLimit = 50000;
        private Color rectangleColor = Color.Black;
        private Color axisColor = Color.Blue;
        private float axisStep = 0.5f;
        public RenderWindow Window { get; private set; }
        private GraphicWindow()
        {
            ContextSettings set = new ContextSettings(1, 1, 4);
            Window = new RenderWindow(new VideoMode(800, 600), "Graphic", Styles.Default, set);
            DispatchWindowEvents(20, true);
        }

        private void DrawAll()
        {
            if(this.Window != null)
            {
                foreach(Drawable item in this.toDraw)
                {
                    this.Window.Draw(item);
                }
            }
        }
        public GraphicWindow(uint height=800, uint width=600, uint frameLimit=30, bool VSync=true, uint anitialisingLvl=4)
        {
            ContextSettings set = new ContextSettings(1, 1, anitialisingLvl);
            Window = new RenderWindow(new VideoMode(width, height), "Graphic", Styles.Default, set);
            DispatchWindowEvents(frameLimit, VSync);
        }
        private void DispatchWindowEvents(uint framerateLimit, bool isVSync)
        {
            Window.SetFramerateLimit(framerateLimit);
            Window.SetVerticalSyncEnabled(isVSync);
            Window.Closed += new EventHandler(Window_Closed);
            Window.Resized += new EventHandler<SizeEventArgs>(Window_Resized);
            Window.MouseButtonPressed += new EventHandler<MouseButtonEventArgs>(Window_MouseButtonPressed);
            Window.MouseButtonReleased += new EventHandler<MouseButtonEventArgs>(Window_MouseButtonReleased);
            Window.MouseMoved += new EventHandler<MouseMoveEventArgs>(Window_MouseMoved);
            Window.MouseWheelScrolled += new EventHandler<MouseWheelScrollEventArgs>(Window_MouseWheelMoved);

            Window.SetActive();
        }

        public void ShowWindow()
        {
            while (Window.IsOpen)
            {
                Window.DispatchEvents();
                Window.Clear(Color.White);
                DrawAll();
                Window.Display();
            }
        }
        public void RenderThread()
        {
            Window.SetActive(true);
            while (Window.IsOpen)
            {
                Window.DispatchEvents();
                Window.Clear(Color.White);
                DrawAll();
                Window.Display();
            }
            //Window.SetActive(false);

        }

        public void SetRectangleLimit(uint limit)
        {
            this.rectangleLimit = limit;
        }

        public void ChangeWindowSettings(uint width, uint height, uint anitialisingLvl, uint fpsLimit, bool isVsync, int maxItemCount)
        {
            ContextSettings set = new ContextSettings(1, 1, anitialisingLvl);
            Window = new RenderWindow(new VideoMode(width, height), "Graphic", Styles.Default, set);
            DispatchWindowEvents(fpsLimit, isVsync);
            SetRectangleLimit((uint)maxItemCount);
        }
        public void GetRectangleFromMethod(double xPosition, double yPosition, double width, double height)
        {
            if(toDraw == null)
            {
                toDraw = new List<Drawable>();
            }
            if (toDraw?.Count > rectangleLimit)
            {
                return;
            }
            if(width <1e-3 || height < 1e-3)
            {
                return;
            }
            float scaleMultiplier = 100;
            double yReal = -yPosition;
            RectangleShape newRect = new RectangleShape();
            newRect.Size = new Vector2f((float)width*scaleMultiplier, (float)height*scaleMultiplier);
            newRect.Position = new Vector2f((float)(xPosition) * scaleMultiplier, (float)(yReal) * scaleMultiplier);
            newRect.OutlineThickness = LineThickness;
            newRect.OutlineColor = rectangleColor;
            toDraw.Add(newRect);
            if(toDraw.Count == 1)
            {
                var axis = Optionals.getAxisLines((RectangleShape)toDraw[0], axisColor, LineThickness, (float)(xPosition + width), (float)(yPosition), axisStep, (float)xPosition, (float)(yPosition-height));
                toDraw.AddRange(axis.lines);
                toDraw.AddRange(axis.marks);
            }
        }

        #region WINDOW_EVENTS
        private void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            if (e.Button == Mouse.Button.Left)
            {
                moving = true;
                oldPos = window.MapPixelToCoords(new Vector2i(e.X, e.Y));
            }
            else if (e.Button == Mouse.Button.Right)
            {
                foreach (Drawable item in toDraw)
                {
                    if (item is RectangleShape itemRect) { 
                       itemRect.OutlineThickness = LineThickness; 
                    }
                }
                window.SetView(window.DefaultView);
            }
        }

        private void Window_MouseWheelMoved(object sender, MouseWheelScrollEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            if (e.Delta < 0)
            {
                zoom = Math.Min(2, zoom + 0.1f);
            }
            else
            {
                zoom = Math.Max(0.5f, zoom - 0.1f);
            }
            View view = window.GetView();
            view.Zoom(zoom);
            window.SetView(view);
            foreach (Drawable item in toDraw)
            {
                if (item is RectangleShape itemRect)
                {
                    itemRect.OutlineThickness *= zoom;
                }
            }
        }


        private void Window_Resized(object sender, SizeEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }
        private void Window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            if (moving)
            {
                Vector2f newPos = window.MapPixelToCoords(new Vector2i(e.X, e.Y));
                Vector2f delta = oldPos - newPos;
                View view = window.GetView();
                view.Center = view.Center + delta;
                window.SetView(view);
                oldPos = window.MapPixelToCoords(new Vector2i(e.X, e.Y));
            }
        }

        private void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                moving = false;
            }
        }
        #endregion
    }
}
