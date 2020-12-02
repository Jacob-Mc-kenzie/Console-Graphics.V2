﻿using System;
using System.Collections.Generic;
using CompactGraphics;

namespace CompactGraphics
{
    public class PixelGrid : Widget
    {
        static char pLower = '▄';
        static char tempT = ';';
        static char pFull = (char)1244;

        /// <summary>
        /// A grid of scaled pixels, works by using half height characters.
        /// </summary>
        /// <param name="bounds"></param>
        public PixelGrid(Rect bounds) : base(bounds)
        {
        }

        public void DrawPixel(int x, int y)
        {
            DrawPixel(x, y, ConsoleColor.White);
        }
        public void DrawPixel(int x, int y, ConsoleColor color)
        {
            if (Bounds.Overlaps(Bounds.x1+x, Bounds.y1+(y / 2)))
            {
                if (y % 2 == 0)
                {
                    rendered.image[y / 2][x] = pFull;
                    rendered.forground[y / 2][x] = color;
                }
                else
                    rendered.background[y / 2][x] = color;
            }
        }
        public override void Draw(Graphics g)
        {
            g.Draw(rendered, Bounds.x1, Bounds.y1);

            char[][] image;
            ConsoleColor[][] background;
            ConsoleColor[][] forground;
            int h = Bounds.y2 - Bounds.y1;
            int w = Bounds.x2 - Bounds.x1;
            
            image = new char[h + 1][];
            background = new ConsoleColor[h + 1][];
            forground = new ConsoleColor[h + 1][];

            for (int i = 0; i <= h; i++)
            {
                image[i] = new char[w + 1];
                forground[i] = new ConsoleColor[w + 1];
                background[i] = new ConsoleColor[w + 1];

                for (int j = 0; j <= w; j++)
                {
                    image[i][j] = pFull;
                    forground[i][j] = ConsoleColor.Black;
                    background[i][j] = ConsoleColor.Black;
                }
            }

            rendered = new TFrame(image,background,forground);
        }
    }
}
