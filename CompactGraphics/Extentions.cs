using System.Collections.Generic;
namespace ComapactGraphicsV2
{
    public static class _
    {
        /// <summary>
        /// returns a list of strings each no longer than the specified max lenght. adapted from http://bryan.reynoldslive.com/post/Wrapping-string-data.aspx
        /// </summary>
        /// <param name="text">the text to wrap</param>
        /// <param name="maxLenght">the max line length in characters</param>
        /// <returns></returns>
        public static List<string> Wrap(this string text, int maxLenght)
        {
            if (text.Length == 0) return new List<string>();

            string[] words = text.Split(' ');
            List<string> lines = new List<string>();
            string currentline = "";

            foreach (string word in words)
            {
                if((currentline.Length > maxLenght) || (currentline.Length + word.Length) > maxLenght)
                {
                    lines.Add(currentline);
                    currentline = "";
                }
                if(currentline.Length > 0)
                    currentline += " " + word;
                else
                    currentline += word;
            }
            if (currentline.Length > 0)
                lines.Add(currentline);
            return lines;
        }
        private static ColoredStringT [] SplitWords(this List<ColoredStringT> strings)
        {
            List<ColoredStringT> words = new List<ColoredStringT>();
            foreach (var item in strings)
                foreach (var word in item.text.Split(' '))
                    words.Add(new ColoredStringT() { text = word, background = item.background, forground = item.forground });
            return words.ToArray();
        }

        /// <summary>
        /// returns a list of Styled Strings wrapped to a specified length;
        /// </summary>
        /// <param name="text">the text to wrap</param>
        /// <param name="maxLenght">the max line length in characters</param>
        /// <returns></returns>
        public static List<StyledTextT> Wrap(this StyledTextT text, int maxLenght)
        {
            if (text.content.Count == 0 || text.Length < maxLenght) return new List<StyledTextT>() {text};
            ColoredStringT[] segmants = text.content.SplitWords();

            List<StyledTextT> lines = new List<StyledTextT>();
            StyledTextT currentline = new StyledTextT();

            foreach (ColoredStringT word in segmants)
            {
                if ((currentline.Length > maxLenght) || (currentline.Length + word.text.Length) > maxLenght)
                {
                    lines.Add(currentline);
                    currentline = new StyledTextT();
                }
                if (currentline.Length > 0)
                {
                    currentline.Add(new ColoredStringT() { text = " " + word.text, background = word.background, forground = word.forground });
                }
                else
                    currentline.Add(word);
            }
            if (currentline.Length > 0)
                lines.Add(currentline);
            return lines;
        }

        /// <summary>
        /// Calculates the new 'top left' to draw from, and bounds to draw in.
        /// </summary>
        /// <param name="r">the rectangle to draw from, assumes top left state</param>
        /// <param name="point">the desired shift</param>
        /// <returns>the shifted rectangle</returns>
        public static Rect OffsetPin(this Rect r, Widget.DrawPoint point)
        {
            switch (point)
            {
                case Widget.DrawPoint.TopLeft:
                    return r;
                case Widget.DrawPoint.Top:
                    return new Rect()
                    {
                        x1 = r.x1 - ((r.x2 - r.x1) / 2),
                        x2 = r.x2 - ((r.x2 - r.x1) / 2),
                        y1 = r.y1,
                        y2 = r.y2
                    };
                case Widget.DrawPoint.TopRight:
                    return new Rect()
                    {
                        x1 = r.x1 - (r.x2 - r.x1),
                        x2 = r.x2 - (r.x2 - r.x1),
                        y1 = r.y1,
                        y2 = r.y2
                    };
                case Widget.DrawPoint.Left:
                    return new Rect()
                    {
                        x1 = r.x1,
                        x2 =  r.x2,
                        y1 = r.y1 - ((r.y2 - r.y1) / 2),
                        y2 = r.y2 - ((r.y2 - r.y1) / 2)
                    };
                case Widget.DrawPoint.Center:
                    return new Rect()
                    {
                        x1 = r.x1 - ((r.x2 - r.x1) / 2),
                        x2 = r.x2 - ((r.x2 - r.x1) / 2),
                        y1 = r.y1 - ((r.y2 - r.y1) / 2),
                        y2 = r.y2 - ((r.y2 - r.y1) / 2)
                    };
                case Widget.DrawPoint.Right:
                    return new Rect()
                    {
                        x1 = r.x1 - ((r.x2 - r.x1) / 2),
                        x2 = r.x2 - ((r.x2 - r.x1) / 2),
                        y1 = r.y1 - (r.y2 - r.y1),
                        y2 = r.y2 - (r.y2 - r.y1) 
                    };
                case Widget.DrawPoint.BottomLeft:
                    return new Rect()
                    {
                        x1 = r.x1,
                        x2 =  r.x2,
                        y1 = r.y1 - (r.y2 - r.y1),
                        y2 = r.y2 - (r.y2 - r.y1)
                    };
                case Widget.DrawPoint.Bottom:
                    return new Rect()
                    {
                        x1 = r.x1 - ((r.x2 - r.x1) / 2),
                        x2 = r.x2 - ((r.x2 - r.x1) / 2),
                        y1 = r.y1 - (r.y2 - r.y1),
                        y2 = r.y2 - (r.y2 - r.y1)
                    };
                case Widget.DrawPoint.BottomRight:
                    return new Rect()
                    {
                        x1 = r.x1 - (r.x2 - r.x1),
                        x2 = r.x2 - (r.x2 - r.x1),
                        y1 = r.y1 - (r.y2 - r.y1),
                        y2 = r.y2 - (r.y2 - r.y1)
                    };
                default:
                    return r;
            }
        }

        public static bool Overlaps(this Rect rect, int x, int y)
        {
            return (x >= rect.x1 && x <= rect.x2 && y >= rect.y1 && y <= rect.y2);
        }
        public static bool Overlaps(this Rect rect, Rect rect2)
        {
            return !(rect.x2 < rect2.x1 || rect.x1 > rect2.x2 || rect.y2 < rect2.y1 || rect.y1 > rect2.y2);
            //return (((rect.x1 >= rect2.x1 && rect.x1 < rect2.x2) || (rect.x2 >= rect2.x1 && rect.x2 < rect2.x2)) && ((rect.y1 >= rect2.y1 && rect.y1 < rect2.y2) || (rect.y2 >= rect.y1 && rect.y2 < rect2.y2)));    
        }
    }

}
