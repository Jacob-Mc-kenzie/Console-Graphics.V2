using System;


using CompactGraphics;
namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Graphics graphics = new Graphics(150,70);
            ExampleMenu menu = new ExampleMenu(graphics);
            Input I = new Input();
            //graphics.FrameCap = 90;
            //Contunually draw frames
            while (true)
            {
                //update the UI
                menu.StepFrame(I);
                graphics.Draw($"{graphics.Fps} fps",ConsoleColor.White, 0,0);
                graphics.Draw($"{graphics.FrameTime} ftime", ConsoleColor.White, 0, 1);
                graphics.Draw($"{graphics.FrameCap} fcap", ConsoleColor.White, 0, 2);
                
                //now that all drawing is done, push the frame to the buffer.
                graphics.pushFrame();
            }
        }
    }
}
