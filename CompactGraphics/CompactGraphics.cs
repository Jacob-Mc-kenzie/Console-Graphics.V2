using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;

namespace ComapactGraphicsV2
{
    /// <summary>
    /// A datastructure containing the three screen buffers in a given frame.
    /// </summary>
    public struct TFrame
    {
        public char[][] image;
        public ConsoleColor[][] background;
        public ConsoleColor[][] forground;
        public Color[] framePallet;
        public int palletLocation;

        public TFrame(int w, int h)
        {
            framePallet = new Color[16];
            palletLocation = 0;
            image = new char[h + 1][];
            background = new ConsoleColor[h + 1][];
            forground = new ConsoleColor[h + 1][];
            for (int i = 0; i <= h; i++)
            {
                image[i] = new char[w + 1];
                forground[i] = new ConsoleColor[w + 1];
                background[i] = new ConsoleColor[w + 1];

                for (int j = 0; j <= w; j++)
                {
                    image[i][j] = '\0';
                    forground[i][j] = ConsoleColor.White;
                    background[i][j] = ConsoleColor.Black;
                }
            }
        }

        public TFrame(char[][] image, ConsoleColor[][] background, ConsoleColor[][] forground, Color[] pallet)
        {
            palletLocation = 0;
            this.image = image;
            this.background = background;
            this.forground = forground;
            this.framePallet = pallet;
        }
        public TFrame(char[][] image, ConsoleColor[][] background, ConsoleColor[][] forground)
        {
            palletLocation = 0;
            this.image = image;
            this.background = background;
            this.forground = forground;
            this.framePallet = new Color[16];
        }
    }
    /// <summary>
    /// A compacted console visual libary of mine with GUI centered features.
    /// </summary>
    public class CompactGraphics
    {
        /// 
        /// To add, carosell like barrier X for stratup, lists, radio buttons, password entry.
        /// 
        ///
        private int framedelay = 0; // the delay between buffered frames being drawn.
        private int maxQueueLength; // The maximum lenght of the buffer.
        private TFrame currentFrame;
        private Queue<TFrame> frameQueue;
        /// <summary>
        /// The width of the virtual screen.
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// The height of the virtual screen.
        /// </summary>
        public int Height { get; private set; }
        public int TimeToFrame { get; private set; }
        public int TimeToDraw { get; private set; }
        private bool keepgoing = true;
        private int frame_Counter = 45;
        private int fps = 45;
        private int timeOfLastFrame = 0;
        private int timeOfLastDraw = 0;
        private SafeFileHandle f;
        CharInfo[] buf;
        SmallRect rect;
        Thread updateThread;
        Thread updateFpsThread;
        /// <summary>
        /// The current number of frames processed in a second, 
        /// may not be accurate as no new frames are drawn if there are none in the queue.
        /// </summary>
        public int Fps
        {
            get { return fps; }
            set { }
        }
        /// <summary>
        /// Delay the main thread by this much if you want to prevent frame drops.
        /// </summary>
        public int FrameTime
        {
            get { return (fps > 0 ?  (900 /fps) : 0) + 1; }
            set { }
        }
        /// <summary>
        /// The current langth of the frame queue, aka how many frames behind are we, usually 20~50
        /// </summary>
        public int QueueLength
        {
            get { return frameQueue.Count; }
            set { }
        }
        private int last_frame_count = 0;
        /// <summary>
        /// Gets or sets the Maximum framerate. (Experimental)
        /// </summary>
        public int FrameCap
        {
            get { return 1000 / (framedelay + 1); }
            set { framedelay = (value > 0 ? 1000/(value + 1)  : 1); }
        }

        #region native imports
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern SafeFileHandle CreateFile(
           string fileName,
           [MarshalAs(UnmanagedType.U4)] uint fileAccess,
           [MarshalAs(UnmanagedType.U4)] uint fileShare,
           IntPtr securityAttributes,
           [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
           [MarshalAs(UnmanagedType.U4)] int flags,
           IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
          SafeFileHandle hConsoleOutput,
          CharInfo[] lpBuffer,
          Coord dwBufferSize,
          Coord dwBufferCoord,
          ref SmallRect lpWriteRegion);


        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }
        
        #endregion
        /// <summary>
        /// Initilise the virtual screen at the currrent cosole dimensions.
        /// </summary>
        public CompactGraphics() : this(Console.WindowWidth, Console.WindowHeight)
        {

        }
        /// <summary>
        /// Initilises the Virtual screen.
        /// </summary>
        /// <param name="w">width of the screen</param>
        /// <param name="h">height of the screen</param>
        public CompactGraphics(int w, int h)
        {
            // Initilisation=================================
            Console.SetWindowSize(w, h);
            Console.CursorVisible = false;
            //Console.OutputEncoding = System.Text.Encoding.ASCII;
            currentFrame = new TFrame(w, h);
            frameQueue = new Queue<TFrame>();
            buf = new CharInfo[w * h];
            rect = new SmallRect() { Left = 0, Top = 0, Right = (short)w, Bottom = (short)h };
            this.f = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            Height = h;
            Width = w;
            maxQueueLength = 3;
            //thread creation================================
            ThreadStart fpsth = new ThreadStart(updateFps);
            updateFpsThread = new Thread(fpsth);
            updateFpsThread.IsBackground = true;
            updateFpsThread.Start();

            ThreadStart thref = new ThreadStart(FrameUpdater);
            updateThread = new Thread(thref);
            updateThread.IsBackground = true;
            updateThread.Start();
        }
        /// <summary>
        /// Initilises the virtual screen.
        /// </summary>
        /// <param name="w">The desired width</param>
        /// <param name="h">The desired height</param>
        /// <param name="queueLength">The maximum number of frames to buffer</param>
        public CompactGraphics(int w, int h, int queueLength) :this(w,h)
        {
            maxQueueLength = queueLength;
        }
        /// <summary>
        /// Background task, updates the FPS
        /// </summary>
        private void updateFps()
        {
            while (keepgoing)
            {
                fps = (frame_Counter - last_frame_count);
                last_frame_count = frame_Counter;
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Updates the screen by writing to the standard output.
        /// </summary>
        /// <param name="screen">The frame to draw</param>
        private void update(TFrame screen)
        {
            if (!f.IsInvalid)
            {
                    for (int i = 0; i < screen.framePallet.Length; i++)
                    {
                       var tmp = !screen.framePallet[i].IsEmpty ? ExtendedColors.SetColor((ConsoleColor)i, screen.framePallet[i]) : i;
                    }
                
                int y, x;
                //generate the buffer
                for (int i = 0; i < buf.Length; ++i)
                {
                    y = (int)i / Width;
                    x = i % Width;
                    buf[i].Attributes = (short)((int)screen.forground[y][x] | (((int)screen.background[y][x]) << 4));
                    buf[i].Char.UnicodeChar = screen.image[y][x];
                }
                //push it.
                bool b = WriteConsoleOutput(this.f, buf,
                          new Coord() { X = (short)Width, Y = (short)Height },
                          new Coord() { X = 0, Y = 0 },
                          ref rect);
                frame_Counter++;
            }
            else
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Console output handel is invalid");
            }
        }

        /// <summary>
        ///  Runs on a seperate thread and draws the desired frames. also removes the drawn frames from the queue.
        /// </summary>
        internal void FrameUpdater()
        {
            while (keepgoing)
            {
                if (frameQueue.Count >= 1)
                {
                    timeOfLastDraw = DateTime.Now.Millisecond;
                    update(frameQueue.Dequeue());
                    TimeToDraw = Math.Max(0, DateTime.Now.Millisecond - timeOfLastDraw);
                }
                
                //Thread.Sleep(framedelay);

            }
        }
        /// <summary>
        /// For debuging only. forces the next frame to be drawn on the main thread.
        /// </summary>
        public void ForceDraw()
        {
            if (frameQueue.Count >= 1)
                update(frameQueue.Dequeue());
        }


        /// <summary>
        /// Queues up the current frame.
        /// </summary>
        public void pushFrame()
        {
            if(frameQueue.Count < maxQueueLength)
            {
                frameQueue.Enqueue(currentFrame);
                currentFrame = new TFrame(Width,Height);
            }
            TimeToFrame = Math.Max(0,DateTime.Now.Millisecond - timeOfLastFrame);
            
            //Thread.Sleep(FrameTime);
            timeOfLastFrame = System.DateTime.Now.Millisecond;
        }

        /// <summary>
        /// Stop trying to draw frames.
        /// </summary>
        public void quit()
        {
            keepgoing = false;
            updateFpsThread.Abort();
            updateThread.Abort();
        }

        

        public void AddToPallet(Color color, ConsoleColor inplaceof)
        {
            currentFrame.framePallet[(int)inplaceof] = color;
        }

        /// <summary>
        /// Add a character to the current frame.
        /// </summary>
        /// <param name="c"> the character to be drawn</param>
        /// <param name="color"> the color to draw the character</param>
        /// <param name="x">the x position from the left</param>
        /// <param name="y">the y position from the top</param>
        public void Draw(char c, ConsoleColor color, int x, int y)
        {
            if (x < Width && y < Height && x >= 0 && y >= 0)
            {
                currentFrame.image[y][x] = c;
                currentFrame.forground[y][x] = color;
            }
        }



        /// <summary>
        /// Add a string to the current frame.
        /// </summary>
        /// <param name="st">the string to draw</param>
        /// <param name="color">the color to draw the string</param>
        /// <param name="x">the x position from the left</param>
        /// <param name="y">the y position from the top</param>
        public void Draw(string st, ConsoleColor color, int x, int y)
        {
            if (y <= Height)
            {
                for (int i = 0; i < st.Length; i++)
                {
                    if (x + i <= Width && x + i >= 0)
                    {
                        currentFrame.image[y][x + i] = st[i];
                        currentFrame.forground[y][x + i] = color;
                    }
                }
            }

        }

        internal void Draw(TFrame frame, int x, int y, ConsoleColor alphaKey = ConsoleColor.Black)
        {
            for (int i = 0; i < frame.image[0].Length; i++)
            {
                for (int j = 0; j < frame.image.Length; j++)
                {
                    if (frame.image[j][i] != '\0')
                        Draw(frame.image[j][i], frame.forground[j][i], x + i, y + j);
                    if (frame.background[j][i] != alphaKey)
                        DrawBackground(frame.background[j][i], x + i, y + j);
                }
            }
        }

        /// <summary>
        /// change the backgroud color at the specified point.
        /// </summary>
        /// <param name="c"> the character to be drawn</param>
        /// <param name="color"> the color to draw the character</param>
        /// <param name="x">the x position from the left</param>
        /// <param name="y">the y position from the top</param>
        public void DrawBackground(ConsoleColor color, int x, int y)
        {
            if (x <= Width && y <= Height && x >= 0 && y >= 0)
            {
                currentFrame.background[y][x] = color;
            }
        }

        public void DrawBackground(int palletIndex, int x, int y)
        {
            if (x <= Width && y <= Height && x >= 0 && y >= 0)
            {
                currentFrame.background[y][x] = (ConsoleColor)palletIndex;
            }
        }

        public void DrawRectangle(char c, ConsoleColor color, int x1, int x2, int y1, int y2)
        {
            for (int i = x1; i <= x2; i++)
            {
                for (int j = y1; j <= y2; j++)
                {
                    this.Draw(c, color, i, j);
                }
            }
        }
        public void DrawBGRectangle(ConsoleColor color, int x1, int x2, int y1, int y2)
        {
            for (int i = x1; i <= x2; i++)
            {
                for (int j = y1; j <= y2; j++)
                {
                    this.DrawBackground(color, i, j);
                }
            }
        }

        public void DrawANSI(string st, int x, int y)
        {
            if (y <= Height)
            {
                for (int i = 0; i < st.Length; i++)
                {
                    if (x + i <= Width && x + i >= 0)
                    {
                        currentFrame.image[y][x + i] = st[i];
                        
                    }
                }
            }

        }

        public void DrawANSI(char c, int x, int y)
        {
            if (x < Width && y < Height && x >= 0 && y >= 0)
            {
                currentFrame.image[y][x] = c;
            }
        }


        #region polygon rotation from https://www.experts-exchange.com/questions/23316788/Rotate-a-2D-polygon-in-C.html (unused)
        public struct Point
        {
            public int X, Y;
            public Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
        /// <summary>
        /// Rotates a polygon formend by a array of points and returns the transformed shape.
        /// </summary>
        /// <param name="polygon">an array of points representing the points of the polygon.</param>
        /// <param name="centroid">the center of rotation</param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public Point[] rotatePolygon(Point[] polygon, Point centroid, double angle)
        {
            for (int i = 0; i < polygon.Length; i++)
            {
                polygon[i] = rotatePoint(polygon[i], centroid, angle);
            }

            return polygon;
        }

        public Point rotatePoint(Point point, Point centroid, double angle)
        {
            int x = centroid.X + (int)((point.X - centroid.X) * Math.Cos(angle * (Math.PI / 180.0)) - (point.Y - centroid.Y) * Math.Sin(angle * (Math.PI / 180.0)));

            int y = centroid.Y + (int)((point.X - centroid.X) * Math.Sin(angle * (Math.PI / 180.0)) + (point.Y - centroid.Y) * Math.Cos(angle * (Math.PI / 180.0)));

            return new Point(x, y);
        }
        #endregion
    }
}
