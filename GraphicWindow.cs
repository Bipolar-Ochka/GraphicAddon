using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace GraphicAddon
{
    public class GraphicWindow
    {
        private float zoom = 1f;
        private bool moving = false;
        private Vector2f oldPos = new Vector2f();
        public RenderWindow Window { get; private set; }
        private GraphicWindow()
        {
            ContextSettings set = new ContextSettings(1, 1, 4);
            Window = new RenderWindow(new VideoMode(800, 600), "Graphic", Styles.Default, set);
            DispatchWindowEvents(20, true);
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
                
                Window.Display();
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
                //foreach (RectangleShape item in shapes)
                //{
                //    item.OutlineThickness = ShapeThickness;
                //}
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
            //view.Size = window.DefaultView.Size;
            view.Zoom(zoom);
            window.SetView(view);
            //foreach (RectangleShape item in shapes)
            //{
            //    item.OutlineThickness *= zoom;
            //}
        }

        private void RescalingObjects() 
        { 

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
