using System;
using System.Collections.Generic;
using System.Text;
using ComapactGraphicsV2;
namespace ComapactGraphicsV2
{
    public struct StyledStringT
    {
        public string text;

        public StyledStringT(string text)
        {
            this.text = text;
            forground = ConsoleColor.White;
            background = ConsoleColor.Black;
        }

        public StyledStringT(string text, ConsoleColor forground, ConsoleColor background) : this(text)
        {
            this.forground = forground;
            this.background = background;
        }

        public ConsoleColor forground;
        public ConsoleColor background;
    }
    public struct ListItemT
    {
        public List<StyledStringT> content;
        public ListItemT(StyledStringT basicContent)
        {
            content = new List<StyledStringT>() { basicContent };
        }
        public ListItemT(List<StyledStringT> content)
        {
            this.content = content;
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
    public class ListBox : Widget
    {
        private List<ListItemT> contents;
        public List<ListItemT> Contents { get => contents; }
        public bool Border = true;
        public ConsoleColor borderColor;
        public ConsoleColor TextColor { get => forColor; set => forColor = value; }

        int height, width, selectedIndex, pageIndexOffset, pageIndexStep;
        public int SelectedIndex { get => selectedIndex; set => selectedIndex = value; }
        public bool autoPaginate;
        public ListBox(List<ListItemT> contents, Rect Bounds) : base(Bounds)
        {
            this.contents = contents;
            height = Bounds.y2 - Bounds.y1;
            width = Bounds.x2 - Bounds.x1;
            borderColor = ConsoleColor.White;
            pageIndexOffset = 0;
            pageIndexStep = height;
            autoPaginate = false;
        }

        public ListBox(List<ListItemT> contents, Rect Bounds, bool autoPaginate) : this(contents, Bounds)
        {
            this.autoPaginate = autoPaginate;
        }

        public bool SetPage(int pageIndex)
        {
            if(pageIndex < 0 || pageIndex * pageIndexStep > contents.Count)
                return false;
            pageIndexOffset = pageIndex * pageIndexStep;
            return true;

        }
        public int GetNumberOfPages()
        {
            return contents.Count / height + 1;
        }

        public override void Draw(CompactGraphics g)
        {
            if (Border)
            {
                for (int y = 0; y < height; y++)
                {
                    g.DrawBackground(borderColor, Bounds.x1, Bounds.y1 + y);    
                }
            }

                for (int y = 0; y < height; y++)
                {
                    if (y + pageIndexOffset < contents.Count)
                    {
                        //g.DrawBackground(ConsoleColor.Red, Bounds.x1 + 4, y);
                        if (selectedIndex == y + pageIndexOffset)
                        {
                            contents[y + pageIndexOffset].DrawHighlighted(g, Bounds.x1 + 2, Bounds.y1 + y, Bounds.x2);
                        }
                        else
                        {
                            contents[y + pageIndexOffset].Draw(g, Bounds.x1 + 2, Bounds.y1 + y, Bounds.x2);
                        }

                    }

                }
        }

        public override void Draw(CompactGraphics g, Input.inpuT input)
        {
            
            g.Draw($"key: {input.key}", ConsoleColor.White, 0, 26);
            if (input.KeyAvalible)
            {
                
                switch (input.key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedIndex > 0)
                        {
                            selectedIndex--;
                            if(autoPaginate && selectedIndex < pageIndexOffset && pageIndexOffset - pageIndexStep >= 0)
                                pageIndexOffset -= pageIndexStep;
                        } 
                        break;
                    case ConsoleKey.DownArrow:
                        if (selectedIndex < contents.Count)
                        {
                            selectedIndex++;
                            if( autoPaginate && selectedIndex >= pageIndexOffset+pageIndexStep && pageIndexOffset + pageIndexStep < contents.Count)
                                pageIndexOffset += pageIndexStep;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (pageIndexOffset + pageIndexStep < contents.Count)
                            pageIndexOffset += pageIndexStep;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (pageIndexOffset - pageIndexStep >= 0)
                            pageIndexOffset -= pageIndexStep;
                        break;
                }
            }
            Draw(g);
        }

    }
}
