using System;
using System.Collections.Generic;
using System.Text;

namespace ComapactGraphicsV2
{
    public class Button : Textbox
    {
        ConsoleColor backColor;
        ConsoleColor hover_color;
        ConsoleColor ClickColor;
        ConsoleColor tempfor, tempback;
        int mouseDown;
        Func<Input, int> OnClick;
        public Button(Rect bounds, string text) : base(text, bounds)
        {
            forColor = ConsoleColor.Black;
            backColor = ConsoleColor.White;
            hover_color = ConsoleColor.Gray;
            ClickColor = ConsoleColor.DarkGray;
            tempfor = ConsoleColor.Black;
            mouseDown = 0;
            OnClick = (Input i) => { return -1; };
        }
        public Button(Rect bounds, string text, ConsoleColor forground, ConsoleColor background) : this(bounds, text)
        {
            this.forColor = forground;
            this.backColor = background;
        }
        public Button(Rect bounds, string text, ConsoleColor forground, ConsoleColor background, ConsoleColor hover_bg, ConsoleColor click_bg, ConsoleColor mouse_fg) : this(bounds, text,forground,background)
        {
            this.hover_color = hover_bg;
            this.ClickColor = click_bg;
            this.tempfor = mouse_fg;
        }
        public void BindOnClick(Func<Input, int> method)
        {
            this.OnClick = method;
        }

        public override void Draw(CompactGraphics g)
        {
            for (int i = 0; i < Bounds.y2 - Bounds.y1; i++)
            {
                if(i < lines.Count)
                    if (Bounds.y1 + i < Bounds.y2)
                        g.Draw(lines[i], forColor, Bounds.x1, Bounds.y1 + i);
                g.DrawBGRectangle(backColor, Bounds.x1, Bounds.x2, Bounds.y1 + i, Bounds.y1 + i +1);
            }
        }

        public override void Draw(CompactGraphics g, Input mouse)
        {
            int[] m = mouse.GetMouse();
            if (Bounds.Overlaps(m[0], m[1]))
            {
                switch (mouse.GetMouseState())
                {
                    case 0:
                        tempback = hover_color;
                        if (mouseDown == 1)
                            OnClick(mouse);
                        mouseDown = 0;
                        break;
                    case 1 when mouseDown == 0 || mouseDown == 1:
                        tempback = ClickColor;
                        mouseDown = 1;
                        break;
                    default:
                        tempback = backColor;
                        mouseDown = 0;
                        break;
                }

            }
            else
                tempback = backColor;
            for (int i = 0; i < lines.Count; i++)
                {
                    if (Bounds.y1 + i < Bounds.y2)
                    {
                        g.Draw(lines[i], tempfor, Bounds.x1, Bounds.y1 + i);
                        g.DrawBGRectangle(tempback, Bounds.x1, Bounds.x2-1, Bounds.y1 + i, Bounds.y1 + i);
                    }
                }
        }

    }
}
