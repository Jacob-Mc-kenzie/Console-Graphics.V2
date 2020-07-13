using System;


using CompactGraphics;
namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Graphics graphics = new Graphics();
            ExampleMenu menu = new ExampleMenu(graphics);
            //Contunually draw frames
            while (true)
            {
                //update the UI
                menu.StepFrame();
                //now that all drawing is done, push the frame to the buffer.
                graphics.pushFrame();
            }
        }
    }
}
