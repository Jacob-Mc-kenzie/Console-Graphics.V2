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
            width = Math.Abs(x2 - x1);
            height = Math.Abs(y2 - y1);
        }

    }


    /// <summary>
    /// A wrapper for easy Navigation between Menus
    /// 
    /// </summary>
    public class CompactGui
    {
        private struct NavItemT
        {
            public string name;
            public int stackLocation;
            public Type value;
            public object state;
        }
        Dictionary<string,NavItemT> NavHyrachy;
        int currentIndex;
        List<string> TravelLog;
        CompactGraphics g;
        Menu current;
        Type IndexMenu;

        public CompactGui(CompactGraphics g)
        {
            NavHyrachy = new Dictionary<string, NavItemT>();
            TravelLog = new List<string>();
            currentIndex = -1;
            this.g = g;
            current = new Menu(g);
            IndexMenu = typeof(Menu);
        }
        public CompactGui(CompactGraphics g, Menu Index) : this(g)
        {
            current = Index;
            IndexMenu = Index.GetType();
        }

        public bool AddNavItem(Type menu, string name)
        {
            if (DoesExist(name))
                return false;
            NavHyrachy.Add(name, new NavItemT() { name = name, stackLocation = ++currentIndex, value = menu });
            return true;
        }
        public bool GoBack(out Menu result)
        {
            result = new Menu(g);
            if (TravelLog.Count == 0)
                return false;
            NavItemT desired = NavHyrachy[ListPop(TravelLog)];
            result = (Menu)Activator.CreateInstance(desired.value, g, desired.state);
            return true;
            
        }

        public void Step(Input.inpuT userInput)
        {

        }

        private bool DoesExist(string name)
        {
            return NavHyrachy.ContainsKey(name);
        }
        private bool NavigateTo(string name)
        {
            if (!DoesExist(name))
                return false;
            NavItemT desired = NavHyrachy[name];
            current = (Menu)Activator.CreateInstance(desired.value, g, desired.state);
            return true;

        }
        private bool NavigateTo(string name, object state)
        {
            return false;
        }
        private T ListPop<T>(List<T> list)
        {
            T item = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return item;
        }

    }
}
