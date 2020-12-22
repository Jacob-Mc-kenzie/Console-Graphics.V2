using System;
using System.Collections.Generic;
using System.Text;
using ComapactGraphicsV2;
namespace ComapactGraphicsV2
{
    public class ListBox : Widget
    {
        private ICollection<Textbox> contents;
        public ICollection<Textbox> Contents { get => contents; }
        public bool Border = true;
        public ConsoleColor borderColor;
        public ConsoleColor TextColor { get => forColor; set => forColor = value; }

        int height, width;
        public ListBox(ICollection<Textbox> contents, Rect Bounds) : base(Bounds)
        {
            this.contents = contents;
            height = Bounds.y2 - Bounds.y1;
            width = Bounds.x2 - Bounds.x1;
            borderColor = ConsoleColor.White;
        }

        public override void Draw(CompactGraphics g)
        {
            if (Border)
            {
                for (int y = 0; y < height; y++)
                {
                    g.DrawBackground(borderColor, Bounds.x1, Bounds.y1 + y);
                }
            }
        }

    }
}
