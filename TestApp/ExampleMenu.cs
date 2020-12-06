using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompactGraphics;

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
        int cof = 0;
        public ExampleMenu(Graphics graphics) : base(graphics)
        {
            //onPage.Add(new Frame('#', new Rect(0, 5, 0, 5)));
            //Random rng = new Random();
            //List<Rect> existing = new List<Rect>();
            //for (int i = 0; i < 5; i++)
            //{
            //    int x1, x2, y1, y2;
            //    x1 = rng.Next(1, Console.WindowWidth-9);
            //    x2 = x1 + 8;
            //    y1 = rng.Next(1, Console.WindowHeight - 5);
            //    y2 = y1 + 4;
            //    Rect r = new Rect(x1, x2, y1, y2);
            //    if(existing.Count != 0)
            //        foreach (var item in existing)
            //        {
            //            item
            //        }
            //    onPage.Add(new Button(new Rect(x1, x2, y1, y2), $"This is some text {i}"));
            //}
            test = new Frame('#', new Rect(1, 9, 1, 9),ConsoleColor.White,ConsoleColor.Black, Widget.DrawPoint.Center);
            onPage.Add(test);
            r = new Rect(20, 100, 5, 35);
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
            for (int i = 0; i < g.Width; i++)
            {
                for (int j = 0; j < g.Height; j++)
                {
                    g.Draw('@', ConsoleColor.Green, i, j);
                }
            }
            StepGrid2(pixelGrid,pixelGrid.Width,pixelGrid.Height, t);
            //cof++;
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
            g.Draw($"lastk: {lastknown}", ConsoleColor.Yellow, 0, 3);
            g.Draw($"character: {(char)cloc}", ConsoleColor.White, 0, 4);
            g.Draw($"code: {cloc}", ConsoleColor.White, 0, 5);
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
            grid.DrawPixel(25, coloroffset % h);
            //for (int i = 0; i < w; i++)
            //{
                //for (int j = 0; j < h; j++)
                //{
                //    grid.DrawPixel(i, j, colors[(j+coloroffset) % 15] == ConsoleColor.White ? ConsoleColor.White : ConsoleColor.Black);
                    //grid.DrawPixel(i, j, colors[(i+coloroffset) % 14]);
                //}
            //}
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
    }
}
