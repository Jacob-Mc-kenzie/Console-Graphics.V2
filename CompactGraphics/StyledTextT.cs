using System;
using System.Collections.Generic;
namespace ComapactGraphicsV2
{
    public struct ColoredStringT
    {
        public string text;

        public ColoredStringT(string text)
        {
            this.text = text;
            forground = ConsoleColor.White;
            background = ConsoleColor.Black;
        }

        public ColoredStringT(string text, ConsoleColor forground, ConsoleColor background) : this(text)
        {
            this.forground = forground;
            this.background = background;
        }

        public ConsoleColor forground;
        public ConsoleColor background;
    }

    public class StyledTextT
    {
        public List<ColoredStringT> content;
        public int Length { get; private set; }
        public StyledTextT()
        {
            content = new List<ColoredStringT>();
        }
        public StyledTextT(ColoredStringT basicContent)
        {
            content = new List<ColoredStringT>() { basicContent };
            Length = basicContent.text.Length;
        }
        public StyledTextT(List<ColoredStringT> content)
        {
            int L = 0;
            content = new List<ColoredStringT>();
            this.content = content;
            foreach (var item in content)
            {
                L += item.text.Length;
            }
            Length = L;
        }

        public void Add(ColoredStringT item)
        {
            content.Add(item);
            Length += item.text.Length;
        }
        public void Add(StyledTextT items)
        {
            content.AddRange(items.content);
            Length += items.Length;
        }
        public void Draw(CompactGraphics g, int x, int y, int maxX)
        {
            int offset = 0;
            foreach (var item in content)
            {
                g.Draw(item.text, item.forground, item.background, x + offset, y);
                offset += item.text.Length;
            }
        }
        public void DrawHighlighted(CompactGraphics g, int x, int y, int maxX)
        {
            int offset = 0;
            ConsoleColor invert(ConsoleColor color)
            {
                return color == ConsoleColor.Black ? ConsoleColor.White : color == ConsoleColor.White ? ConsoleColor.Black : color == ConsoleColor.Yellow ? ConsoleColor.DarkYellow : color == ConsoleColor.Blue ? ConsoleColor.DarkBlue : color;
            }
            foreach (var item in content)
            {
                g.Draw(item.text, invert(item.forground), invert(item.background), x + offset, y);
                offset += item.text.Length;
            }
        }
    }
}
