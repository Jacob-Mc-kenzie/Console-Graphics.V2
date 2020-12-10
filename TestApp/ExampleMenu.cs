using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComapactGraphicsV2;

namespace TestApp
{
    class ExampleMenu : Menu
    {
        Frame test;
        char lastknown = '\0';
        static ConsoleColor[] colors = { (ConsoleColor)0, (ConsoleColor)1, (ConsoleColor)2, (ConsoleColor)3, (ConsoleColor)4, (ConsoleColor)6, (ConsoleColor)7, (ConsoleColor)8, (ConsoleColor)9, (ConsoleColor)10, (ConsoleColor)11, (ConsoleColor)12, (ConsoleColor)13, (ConsoleColor)14, (ConsoleColor)15 };
        Rect r;
        Life content;
        PixelGrid pixelGrid;
        int cloc = 1000;
        int bounce = 0;
        bool t = true;
        int cof = 15;
        int[] rgb = new int[] { 255, 0, 0 };
        public ExampleMenu(ComapactGraphicsV2.CompactGraphics graphics) : base(graphics)
        {
            test = new Frame('#', new Rect(1, 9, 1, 9),ConsoleColor.White,ConsoleColor.Black, Widget.DrawPoint.Center);
            onPage.Add(test);
            r = new Rect(20, 80, 5, 35);
            content = new Life(80, 60, 100, 50);
            pixelGrid = new PixelGrid(r);
            onPage.Add(pixelGrid);
            //onPage.Add(new Frame('%', r));
            //onPage.Add(new Button(r, "This is some text"));
        }

        public override void StepFrame(Input input)
        {
            base.StepFrame(input);
            //content.Step(pixelGrid);

            StepGrid3(pixelGrid,pixelGrid.Width,pixelGrid.Height);
            //cof = (cof - 1) < 0 ? 15 : cof -1;
            //t = !t;
            //pixelGrid.DrawPixel(5, 4);
            int[] m = input.GetMouse();
            if (input.KeyAvalible && bounce > 3)
            {
                bounce = 0;
                char c = input.ReadKey();
                lastknown = c;
                switch (c)
                {
                    case '=':
                        content = new Life(80, 60, 100, 30);
                        break;
                    case '-':
                        cloc--;
                        break;
                }

            }
            bounce++;
            test.ReSize(new Rect(m[0], m[0] + 8, m[1], m[1] + 8));
            if (r.Overlaps(test.Bounds))
            {
                test.SetColor(ConsoleColor.Red);
            }
            else
                test.SetColor(ConsoleColor.White);

        }

        private void StepGrid(PixelGrid grid,int w, int h, int coloroffset)
        {
            //grid.DrawPixel(25, coloroffset % h);
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    grid.DrawPixel(i, j, colors[(i + coloroffset) % 15]);
                    grid.DrawPixel(j, i, colors[(i + coloroffset) % 15]);
                }
            }
        }
        private void StepGrid2(PixelGrid grid, int w, int h, bool togglestate)
        {
            bool toggle = togglestate;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (toggle)
                        grid.DrawPixel(y, x);
                    toggle = !toggle;
                }
                toggle = !toggle;
            }
        }
        private void StepGrid3(PixelGrid grid, int w, int h)
        {
            int lines = w / 15;
            int color = 1;
            for (int x = 0; x < w; x++)
            {
                switch(x % lines)
                {
                    case 0:
                        color++;
                        break;
                    default:
                        for (int y = 0; y < h; y++)
                        {
                            grid.DrawPixel(x, y, (ConsoleColor)color);
                        }
                        break;

                }
            }
            for (int i = 1; i < 16; i++)
            {
                rgb = stepRainbow(rgb);
                ExtendedColors.SetColor((ConsoleColor)i, System.Drawing.Color.FromArgb(rgb[0], rgb[1], rgb[2]));
            }
        }
        private int[] stepRainbow(int[] from)
        {
            if (from[0] == 255 && from[2] < 255 && from[1] == 0)
                from[2] += 15;
            else if (from[2] == 255 && from[0] > 0)
                from[0] -= 15;
            else if (from[2] == 255 && from[1] < 255)
                from[1] += 15;
            else if (from[1] == 255 && from[2] > 0)
                from[2] -= 15;
            else if (from[1] == 255 && from[0] < 255)
                from[0] += 15;
            else
                from[1] -= 15;

            return from;
        }
    }
}
