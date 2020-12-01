using System;
using System.Collections.Generic;
using CompactGraphics;

namespace CompactGraphics
{
    public class PixelGrid : Widget
    {
        static char pLower = '▄';
        static char pFull = '█';

        /// <summary>
        /// A grid of scaled pixels, works by using half height characters.
        /// </summary>
        /// <param name="bounds"></param>
        public PixelGrid(Rect bounds) : base(bounds)
        {

            rendered = new TFrame(bounds.x2 - bounds.x1, bounds.y2 - bounds.y1);
        }

        public void DrawPixel(int x, int y)
        {
            DrawPixel(x, y, ConsoleColor.White);
        }
        public void DrawPixel(int x, int y, ConsoleColor color)
        {
            if (Bounds.Overlaps(x, y / 2))
            {
                if (y % 2 == 0)
                {
                    rendered.image[y / 2][x] = pLower;
                    rendered.forground[y / 2][x] = color;
                }
                else
                    rendered.background[y / 2][x] = color;
            }
        }
    }
}
