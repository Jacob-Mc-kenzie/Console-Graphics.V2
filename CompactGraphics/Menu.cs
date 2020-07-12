using System;
using System.Collections.Generic;

namespace CompactGraphics
{
    /// <summary>
    /// A base for Widget Based Menus
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// The graphics object to link to
        /// </summary>
        protected Graphics g;
        /// <summary>
        /// The list of widgets on page, in draw order.
        /// </summary>
        protected List<Widget> onPage;
        /// <summary>
        /// The numeric status of the menu, used for navigation.
        /// </summary>
        public int Status { get { return status; } }
        protected int status;
        public Menu()
        {
            g = new Graphics(Console.WindowWidth, Console.WindowHeight);
            onPage = new List<Widget>();
        }
        public Menu(Graphics g)
        {
            this.g = g;
            onPage = new List<Widget>();
        }
        /// <summary>
        /// Steps to the next frame by adding all the widgets to the current frame.
        /// DOES NOT PUSH
        /// </summary>
        public virtual void StepFrame()
        {
            foreach (Widget widget in onPage)
            {
                widget.Draw(g);
            }
        }
        /// <summary>
        /// Steps to the next frame by adding all the widgets to teh current frame, giving them the users input.
        /// </summary>
        /// <param name="keyinfo">The input to handle</param>
        public virtual void StepFrame(ConsoleKeyInfo keyinfo)
        {
            foreach (Widget widget in onPage)
            {
                widget.Draw(g, keyinfo);
            }
        }
    }
}
