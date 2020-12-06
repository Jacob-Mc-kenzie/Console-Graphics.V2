using System;


using CompactGraphics;
namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Graphics graphics = new Graphics(130,50);
            ExampleMenu menu = new ExampleMenu(graphics);
            Input I = new Input();
            graphics.FrameCap = 120;
            //Contunually draw frames
            while (true)
            {
                //update the UI
                menu.StepFrame(I);
                graphics.Draw($"{graphics.Fps} fps",ConsoleColor.White, 0,0);
                graphics.Draw($"{graphics.FrameTime} ftime", ConsoleColor.White, 0, 1);
                graphics.Draw($"{graphics.TimeToFrame} ttf", ConsoleColor.White, 0, 2);
                
                //now that all drawing is done, push the frame to the buffer.
                graphics.pushFrame();
            }
        }
    }
}
