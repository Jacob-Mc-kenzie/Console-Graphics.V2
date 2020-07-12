using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System;


namespace CompactGraphics
{

    public class Mouse
    {
        NativeMethods.ConsoleHandle handle;
        NativeMethods.INPUT_RECORD record;
        Thread updateThread;
        int mode;
        public bool KeyAvalible = false;
        private int x = 0, y = 0;
        private char c = '\0';
        private int butstate = 0;
        uint recordLen = 0;
        public Mouse()
        {
            handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);

            mode = 0;
            if (!(NativeMethods.GetConsoleMode(handle, ref mode))) { throw new Win32Exception(); }

            mode |= NativeMethods.ENABLE_MOUSE_INPUT;
            mode &= ~NativeMethods.ENABLE_QUICK_EDIT_MODE;
            mode |= NativeMethods.ENABLE_EXTENDED_FLAGS;

            if (!(NativeMethods.SetConsoleMode(handle, mode))) { throw new Win32Exception(); }
            record = new NativeMethods.INPUT_RECORD();

            ThreadStart thref = new ThreadStart(UpdateThread);
            updateThread = new Thread(thref);
            updateThread.IsBackground = true;
            updateThread.Start();
        }

        private async void UpdateThread()
        {
            while (true)
            {
                if (!(await Task.Run(()=> {return NativeMethods.ReadConsoleInput(handle, ref record, 1, ref recordLen); }))) { throw new Win32Exception(); }
                switch (record.EventType)
                {
                    case NativeMethods.MOUSE_EVENT:
                        {
                            x = record.MouseEvent.dwMousePosition.X;
                            y = record.MouseEvent.dwMousePosition.Y;
                            butstate = record.MouseEvent.dwButtonState;
                            KeyAvalible = false;
                            
                        }
                        break;

                    case NativeMethods.KEY_EVENT:
                        {
                            c = record.KeyEvent.UnicodeChar;
                            KeyAvalible = true;
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// If a key is avalible return it.
        /// </summary>
        /// <returns>the key or null</returns>
        public char ReadKey(bool display = false)
        {
            if(KeyAvalible)
            {
                KeyAvalible = false;
                if (display)
                    Console.Write(c);
                return c;
            }
            return '\0';
        }

        public int[] GetMouse()
        {
            return new[] { x, y };
        }
        public int GetMouseState()
        {
            return butstate;
        }

    }
}
