using System;
using System.Drawing;

using ComapactGraphicsV2;
namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] rgb = new int[] { 255,0,0};
            CompactGraphics graphics;
            if(args.Length == 2)
                graphics = new CompactGraphics(int.Parse(args[0]),int.Parse(args[1]));
            else
                graphics = new CompactGraphics(500, 200);
            //CompactGraphics.Graphics.SetColor(ConsoleColor.White, 244, 106, 7);
            ExampleMenu menu = new ExampleMenu(graphics);
            ExtendedColors customPallet = new ExtendedColors();
            Input I = new Input();
            customPallet.SetColor("Sea Green", Color.FromArgb(46, 139, 87), ConsoleColor.Green);
            //graphics.FrameCap = 120;
            //Contunually draw frames
            while (true)
            {
                //rgb = stepRainbow(rgb);
                //graphics.AddToPallet(Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                //ExtendedColors.SetColor(ConsoleColor.White,Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                //update the UI
                menu.StepFrame(I);
                graphics.Draw($"{graphics.Fps} fps",ConsoleColor.White, 0,0);
                graphics.Draw($"{graphics.TimeToDraw} drawTime", customPallet.GetColor("Sea Green"), 0, 1);
                graphics.Draw($"{graphics.TimeToFrame} ttf", ConsoleColor.White, 0, 2);
                //graphics.Draw($"{rgb[0]}, {rgb[1]}, {rgb[2]}", ConsoleColor.Red, 0, 6);
                //now that all drawing is done, push the frame to the buffer.
                graphics.pushFrame();
                //Console.WriteLine("\u001b[31mHello World!\u001b[0m");
                //System.Threading.Thread.Sleep(32);
            }
        }

        static int[] stepRainbow(int[] from)
        {
            if (from[0] == 255 && from[2] < 255 && from[1] == 0)
                from[2]+= 5;
            else if (from[2] == 255 && from[0] > 0)
                from[0]-= 5;
            else if (from[2] == 255 && from[1] < 255)
                from[1]+= 5;
            else if (from[1] == 255 && from[2] > 0)
                from[2]-= 5;
            else if (from[1] == 255 && from[0] < 255)
                from[0]+= 5;
            else
                from[1]-= 5;

            return from;
        }
    }
}
