using System;
namespace CompactGraphics
{
    /// <summary>
    /// A simple Rectangle with a border widget.
    /// </summary>
    public class Frame : Widget
    {
        protected char borderchar;
        protected ConsoleColor backcolor;
        /// <summary>
        /// Creates a New simple frame.
        /// </summary>
        /// <param name="c">the border character</param>
        /// <param name="r">the bounding box to draw to</param>
        public Frame(char c, Rect r)
        {
            this.borderchar = c;
            this.baseBounds = r;
            forColor = ConsoleColor.White;
            backcolor = ConsoleColor.Black;
            Pin = DrawPoint.TopLeft;
            this.Bounds = r;
        }
        /// <summary>
        /// Creates a New simple Frame
        /// </summary>
        /// <param name="c">The border character</param>
        /// <param name="r">The bounding box to draw to</param>
        /// <param name="forcolor">The color of the border</param>
        /// <param name="backcolor">The color of the background</param>
        /// <param name="p">The Relative pin direction</param>
        public Frame(char c,Rect r, ConsoleColor forcolor, ConsoleColor backcolor, DrawPoint p) :this(c,r)
        {
            this.forColor = forcolor;
            this.backcolor = backcolor;
            Pin = p;
            this.Bounds = r.OffsetPin(this.Pin);
        }

        public override void Draw(Graphics g)
        {
            for (int i = Bounds.x1; i <= Bounds.x2; i++)
            {
                for (int j = Bounds.y1; j <= Bounds.y2; j++)
                {
                    if(i == Bounds.x1 || i == Bounds.x2 || j == Bounds.y1 || j == Bounds.y2)
                    {
                        g.Draw(borderchar, forColor, i, j);
                    }
                    g.DrawBackground(backcolor, i, j);
                }
            }
        }


        public override void Draw(Graphics g, ConsoleKeyInfo keyInfo)
        {
            Draw(g);
        }

        public override void Draw(Graphics g, Input I)
        {
            Draw(g);
        }

    }
}
