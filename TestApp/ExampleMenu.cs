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
        public ExampleMenu(Graphics graphics) : base(graphics)
        {
            onPage.Add(new Frame('#', new Rect(0, 5, 0, 5)));
            onPage.Add(new Textbox("This is some text", new Rect(1, 4, 1, 4)));
        }
    }
}
