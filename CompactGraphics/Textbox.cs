using System;
using System.Collections.Generic;
namespace CompactGraphics
{
    /// <summary>
    /// A simple Widget based textbox, read only, acts like a WinForms Label kind of.
    /// </summary>
    public class Textbox : Widget
    {
        internal List<string> lines;
        private System.ConsoleColor flashColor;
        private System.Timers.Timer eventDelay;
        /// <summary>
        /// Create a new Textbox.
        /// </summary>
        /// <param name="text">The text to contain.</param>
        /// <param name="r">The bounding box to draw in, used to wrap text</param>
        public Textbox(string text, Rect r)
        {
            eventDelay = new System.Timers.Timer(200);
            this.baseBounds = r;
            this.Bounds = r;
            lines = text.Wrap(Math.Abs(r.x2 - r.x1));
            forColor = ConsoleColor.White;
            this.Pin = DrawPoint.TopLeft;
        }
        /// <summary>
        /// Create a new textbox.
        /// </summary>
        /// <param name="text">The text to contain</param>
        /// <param name="r">The bounds to draw in</param>
        /// <param name="forcolor">The color of the text</param>
        public Textbox(string text, Rect r, ConsoleColor forcolor) : this(text, r)
        {
            this.forColor = forcolor;
        }
        /// <summary>
        /// Create a new TextBox
        /// </summary>
        /// <param name="text">The text to contain</param>
        /// <param name="forcolor">The color of the text</param>
        /// <param name="r">The Bounds to draw in</param>
        /// <param name="p">The relaitve location of the given top left corner</param>
        public Textbox(string text, ConsoleColor forcolor, Rect r, DrawPoint p) : this(text, r)
        {
            this.forColor = forcolor;
            this.Pin = p;
            Bounds = baseBounds.OffsetPin(p);
        }
        /// <summary>
        /// Change the forgroud color of the specified widget for 200 ms
        /// </summary>
        /// <param name="color">the color to flash to</param>
        public void Flash(System.ConsoleColor color)
        {
            flashColor = forColor;
            forColor = color;
            eventDelay.Enabled = true;
            eventDelay.Elapsed += Delayed_Flash;
            eventDelay.AutoReset = false;
        }
        /// <summary>
        /// Callback for the event timer.
        /// </summary>
        private void Delayed_Flash(object sender, System.Timers.ElapsedEventArgs e)
        {
            forColor = flashColor;
        }

        public override void Draw(Graphics g)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (Bounds.y1 + i <= Bounds.y2)
                {
                    g.Draw(lines[i], forColor, Bounds.x1, Bounds.y1 + i);
                }
            }

        }
        /// <summary>
        /// Sets the text in the Textbox.
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            lines = text.Wrap(Math.Abs(baseBounds.x2 - baseBounds.x1));
        }
        public override void Draw(Graphics g, ConsoleKeyInfo keyInfo)
        {
            Draw(g);
            //ToDo
        }


    }
}
