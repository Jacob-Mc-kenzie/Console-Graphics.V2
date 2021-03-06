﻿using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System;


namespace ComapactGraphicsV2
{

    public class Input
    {
        //===================
        // ToDo:
        // more advanced key state, support for paste, keydown, repeatkey etc.
        //===================
        public struct inpuT
        {
            public bool EventType;
            public bool KeyAvalible;
            public ConsoleKey key;
            public char KeyChar;
            public int MouseX, MouseY, buttonState;
        }
        NativeMethods.ConsoleHandle handle;
        NativeMethods.INPUT_RECORD record;
        Thread updateThread;
        int mode;
        private bool keyAvalible = false;
        public bool KeyAvalible { get { return mouseMode ? keyAvalible : Console.KeyAvailable; } set => keyAvalible = value; }
        private bool quit = false;
        private int x = 0, y = 0;
        private char c = '\0';
        private int butstate = 0;
        uint recordLen = 0;
        ushort charcode = 0;
        private bool mouseMode;
        public bool MouseMode { set {
                if (!value)
                {
                    quit = true;
                    x = -100;
                    y = -100;
                    updateThread.Abort();
                }
                else
                {
                    quit = false;
                    updateThread.Start();
                }
                mouseMode = value;
            }
            get { return mouseMode; }
        }
        public Input()
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
            mouseMode = true;
        }
        public Input(bool mouseMode)
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
            if (mouseMode)
                updateThread.Start();
            this.mouseMode = mouseMode;

        }

        private async void UpdateThread()
        {
            while (!quit)
            {
                if (!(await Task.Run(()=> {return NativeMethods.ReadConsoleInput(handle, ref record, 1, ref recordLen); }))) { throw new Win32Exception(); }
                switch (record.EventType)
                {
                    case NativeMethods.MOUSE_EVENT:
                        {
                            x = record.MouseEvent.dwMousePosition.X;
                            y = record.MouseEvent.dwMousePosition.Y;
                            butstate = record.MouseEvent.dwButtonState;
                            
                        }
                        break;

                    case NativeMethods.KEY_EVENT:
                        {
                            c = record.KeyEvent.UnicodeChar;
                            charcode = record.KeyEvent.wVirtualKeyCode;
                            KeyAvalible = record.KeyEvent.bKeyDown;
                        }
                        break;
                }
            }
        }
        public void Quit()
        {
            quit = true;
        }
        /// <summary>
        /// If a key is avalible return it.
        /// </summary>
        /// <returns>the key or null</returns>
        public ConsoleKey ReadKey(bool display = false)
        {
            if (mouseMode)
            {
                KeyAvalible = false;
                if (display)
                    Console.Write(this.c);
                return (ConsoleKey)charcode;
            }
            ConsoleKeyInfo c = Console.ReadKey(!display);
            this.c = c.KeyChar;
            return c.Key;
        }
        /// <summary>
        /// This method should be used by widgets only.
        /// </summary>
        /// <returns></returns>
        public char ReadLastKey()
        {
            //throw new NotImplementedException("Not possible under re-write");
            return c;
        }

        public int[] GetMouse()
        {
            return new[] { x, y };
        }
        public int GetMouseState()
        {
            return butstate;
        }

        public inpuT GetInput()
        {
            return new inpuT() { key = ReadKey(), MouseX = x, MouseY = y, buttonState = butstate, KeyAvalible = KeyAvalible, KeyChar =  c};
        }

    }
}
