using System;
using System.Collections.Generic;

namespace ComapactGraphicsV2
{
    /// <summary>
    /// A base for Widget Based Menus
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// The graphics object to link to
        /// </summary>
        protected CompactGraphics g;
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
            throw new Exception("Cannot call default constructor");
        }
        public Menu(CompactGraphics g)
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
        /// Steps to the next frame by adding all the widgets to the current frame, giving them the users input.
        /// </summary>
        /// <param name="keyinfo">The input to handle</param>
        public virtual void StepFrame(ConsoleKeyInfo keyinfo)
        {
            foreach (Widget widget in onPage)
            {
                widget.Draw(g, keyinfo);
            }
        }
        /// <summary>
        /// Steps to the next frame by adding all the widgets to the current frame, giving them the users input.
        /// </summary>
        /// <param name="input">The input to handle</param>
        public virtual void StepFrame(Input input)
        {
            foreach (Widget widget in onPage)
            {
                widget.Draw(g, input);
            }
        }
    }
}
