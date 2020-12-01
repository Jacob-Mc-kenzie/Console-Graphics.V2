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
        Rect r;
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
            //onPage.Add(test);
            r = new Rect(20, 30, 10, 14);
            PixelGrid pixelGrid = new PixelGrid(r);
            //onPage.Add(new Frame('%', r));
            //onPage.Add(new Button(r, "This is some text"));
        }

        public override void StepFrame(Input input)
        {
            base.StepFrame(input);
            int[] m = input.GetMouse();
            if (input.KeyAvalible)
            {
                char c = input.ReadKey();
                lastknown = c;
                switch (c)
                {
                    case '=':
                        g.FrameCap += 5;
                        break;
                    case '-':
                        g.FrameCap -= 5;
                        break;
                }

            }
            g.Draw($"lastk: {lastknown}", ConsoleColor.Yellow, 0, 3);
            test.ReSize(new Rect(m[0], m[0] + 8, m[1], m[1] + 8));
            if (r.Overlaps(test.Bounds))
            {
                test.SetColor(ConsoleColor.Red);
            }
            else
                test.SetColor(ConsoleColor.White);
        }
    }
}
