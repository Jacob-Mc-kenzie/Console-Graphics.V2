using System;


using CompactGraphics;
namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Graphics graphics = new Graphics();

            //Contunually draw frames
            while (true)
            {
                //draw the frame counter at the top of the screen.
                graphics.Draw($"Fps: {graphics.Fps}", ConsoleColor.White, 0, 0);
                //now that all drawing is done, push the frame to the buffer.
                graphics.pushFrame();
            }
        }
    }
}
