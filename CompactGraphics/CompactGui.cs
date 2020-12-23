using System;
using System.Collections.Generic;
using System.Text;

namespace ComapactGraphicsV2
{
    /// <summary>
    /// A simple structure to store the bounds of a rectangle;
    /// </summary>
    public struct Rect 
    {
        public int x1, x2, y1, y2, width, height;
        public Rect(int x1, int x2, int y1, int y2)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
            width = x2 - x1;
            height = y2 - y1;
        }

    }


    /// <summary>
    /// Originally intended to be a base class inherited by Menu.cs for overarching gui design, but As I didn't need anything other than menus
    /// was never implemented.
    /// </summary>
    public static class CompactGui
    {
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
