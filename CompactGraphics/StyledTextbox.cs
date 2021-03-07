using System;
using System.Collections.Generic;
using System.Text;

namespace ComapactGraphicsV2
{
    public class StyledTextbox : Widget
    {
        StyledTextT original;
        List<StyledTextT> lines;
        public StyledTextbox(StyledTextT text, Rect r)
        {
            this.baseBounds = r;
            this.Bounds = r;
            original = text;
            lines = text.Wrap(r.width);
            forColor = ConsoleColor.White;
            this.Pin = DrawPoint.TopLeft;
        }
        public void SetText(StyledTextT text)
        {
            original = text;
            lines = text.Wrap(Bounds.width);
        }
        public void AppendText(ColoredStringT text)
        {
            original.Add(text);
            lines = original.Wrap(Bounds.width);

        }
        public override void Draw(CompactGraphics g)
        {
            int yOffset = 0;
            foreach (StyledTextT line in lines)
            {
                if(yOffset < Bounds.height)
                {
                    line.Draw(g, Bounds.x1, Bounds.y1 + yOffset, Bounds.x2);
                }
            }
        }
    }
}
