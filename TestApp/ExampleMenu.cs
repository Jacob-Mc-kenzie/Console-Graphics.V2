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
        bool firstrun = true;
        int[] rgb = new int[] { 255, 0, 0 };
        List<int[]> values;
        ExtendedColors pallet;
        List<ListItemT> listItems;
        public ExampleMenu(ComapactGraphicsV2.CompactGraphics graphics) : base(graphics)
        {
            Random rng = new Random();
            listItems = new List<ListItemT>();
            for (int i = 0; i < 200; i++)
            {
                listItems.Add(new ListItemT() { content = new List<StyledStringT>() { new StyledStringT($"S{rng.Next()}") } });
            }
            test = new Frame('#', new Rect(1, 9, 1, 9),ConsoleColor.White,ConsoleColor.Black, Widget.DrawPoint.Center);
            onPage.Add(test);
            r = new Rect(20, 40, 0, 20);
            content = new Life(80, 60, 100, 50);
            pallet = new ExtendedColors();
            pixelGrid = new PixelGrid(r);
            //onPage.Add(pixelGrid);


            onPage.Add(new ListBox(listItems, new Rect(20, 120, 10, 20),true));
            //onPage.Add(new ListBox(new List<Textbox>(), new Rect(10,40,5,40)));
            //onPage.Add(new Frame('%', r));
            //onPage.Add(new Button(r, "This is some text"));
        }

        public override void StepFrame(Input input)
        {
            base.StepFrame(input);
            //content.Step(pixelGrid);
            g.Draw($"{rgb[0]}, {rgb[1]}, {rgb[2]}", ConsoleColor.Red, 0, 6);
            g.Draw($"Frame w: {pixelGrid.Bounds.width}, Frame h: {pixelGrid.Bounds.height}", ConsoleColor.Green, 0, 7);
            //StepGrid3(pixelGrid,pixelGrid.Width,pixelGrid.Height);
            StepGrid(pixelGrid, r.width, r.height/2, cof);
            //cof = (cof - 1) < 1 ? 15 : cof -1;
            cof = ((cof + 1) % 14) + 1;

            //t = !t;
            //pixelGrid.DrawPixel(5, 4);
            int[] m = input.GetMouse();
            if (input.KeyAvalible && bounce > 3)
            {
                bounce = 0;
                ConsoleKey c = input.ReadKey();
                lastknown = c.ToString()[0];
                switch (c)
                {
                    case ConsoleKey.OemPlus:
                        pixelGrid.ReSize(new Rect(pixelGrid.Bounds.x1, pixelGrid.Bounds.x2, pixelGrid.Bounds.y1, pixelGrid.Bounds.y2 + 1));
                        break;
                    case ConsoleKey.OemMinus:
                        pixelGrid.ReSize(new Rect(pixelGrid.Bounds.x1, pixelGrid.Bounds.x2, pixelGrid.Bounds.y1, pixelGrid.Bounds.y2 - 1));
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
            //for (int i = 0; i < w; i++)
            //{
            //    for (int j = 0; j <= i; j++)
            //    {
            //        grid.DrawPixel(i, j, colors[((i + coloroffset) % 14) + 1]);
            //        grid.DrawPixel(j, i, colors[((i + coloroffset) % 14) + 1]);
            //    }
            //}
            int half = w / 2;
            //bottm right
            for (int i = 0; i < half; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    grid.DrawPixel(i + half, j + half+1, colors[((i + coloroffset) % 14) + 1]);
                    grid.DrawPixel(j + half, i + half+1, colors[((i + coloroffset) % 14) + 1]);
                }
            }
            ////top left
            for (int i = half; i > 0; i--)
            {
                for (int j = half; j >= i; j--)
                {
                    grid.DrawPixel(i, j, colors[(((half - i) + coloroffset) % 14) + 1]);
                    grid.DrawPixel(j, i, colors[(((half - i) + coloroffset) % 14) + 1]);
                }
            }
            //top right
            for (int i = 0; i < half; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    grid.DrawPixel(i + half, half -j, colors[((i + coloroffset) % 14) + 1]);
                    grid.DrawPixel(j + half, half -i, colors[((i + coloroffset) % 14) + 1]);
                }
            }
            for (int i = half; i > 0; i--)
            {
                for (int j = half; j >= i; j--)
                {
                    grid.DrawPixel(i, w-j, colors[(((half - i) + coloroffset) % 14) + 1]);
                    grid.DrawPixel(j, w-i, colors[(((half - i) + coloroffset) % 14) + 1]);
                }
            }

            if (cof % 2 == 1)
                AddRainbow();
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
                switch (x % lines)
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

            AddRainbow();

        }

        private void AddRainbow()
        {
            if (firstrun)
            {
                for (int i = 1; i < 16; i++)
                {
                    rgb = stepRainbow(rgb);
                    pallet.SetColor("", System.Drawing.Color.FromArgb(rgb[0], rgb[1], rgb[2]), (ConsoleColor)i);
                }
                firstrun = false;
            }  
            else
                ShiftByOne();
        }

        private void ShiftByOne()
        {
            rgb = stepRainbow(rgb);
            System.Drawing.Color last = pallet.GetColor(ConsoleColor.White);
            System.Drawing.Color next = System.Drawing.Color.FromArgb(rgb[0], rgb[1], rgb[2]);
            for (int i = 15; i > 0; i--)
            {
                last = pallet.GetColor((ConsoleColor)i);
                pallet.SetColor("",next,(ConsoleColor)i);
                next = last;
            }
        }
        private int[] stepRainbow(int[] from)
        {
            if (from[0] == 255 && from[2] < 255 && from[1] == 0)
                from[2] += 51;
            else if (from[2] == 255 && from[0] > 0)
                from[0] -= 51;
            else if (from[2] == 255 && from[1] < 255)
                from[1] += 51;
            else if (from[1] == 255 && from[2] > 0)
                from[2] -= 51;
            else if (from[1] == 255 && from[0] < 255)
                from[0] += 51;
            else
                from[1] -= 51;

            return from;
        }
    }
}
