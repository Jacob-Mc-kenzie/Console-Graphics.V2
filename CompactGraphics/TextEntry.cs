using System;
using System.Timers;

namespace ComapactGraphicsV2
{
    /// <summary>
    /// A simnple Widget based single line text entry feild.
    /// </summary>
    public class TextEntry : Widget
    {
        private string text;
        private ConsoleColor flashColor;
        public string Text { get { return text; } }
        private Timer eventDelay;
        /// <summary>
        /// Creates a Text entry widget, single line with scrolling overflow.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="maxlenght"></param>
        public TextEntry(int x, int y, int maxlenght)
        {
            eventDelay = new Timer(200);
            this.text = "";
            this.baseBounds = new Rect(x, x + maxlenght, y, y + 1);
            this.Pin = DrawPoint.TopLeft;
            this.Bounds = new Rect(x, x + maxlenght, y, y + 1);
            this.forColor =ConsoleColor.White;
        }

        /// <param name="forground">The color of the text</param>
        /// <param name="placeholder">The text to start with</param>
        public TextEntry(int x, int y, int maxlenght,ConsoleColor forground, string placeholder) : this(x,y,maxlenght)
        {
            this.forColor = forground;
            this.text = placeholder;
        }
        /// <summary>
        /// Change the forgroud color of the specified widget for 200 ms
        /// </summary>
        /// <param name="color">the color to flash to</param>
        public void Flash(ConsoleColor color)
        {
            flashColor = forColor;
            forColor = color;
            eventDelay.Enabled = true;
            eventDelay.Elapsed += Delayed_Flash;
            eventDelay.AutoReset = false;
        }
        /// <summary>
        /// Callback for the event timer.
        /// </summary>
        private void Delayed_Flash(object sender, ElapsedEventArgs e)
        {
            forColor = flashColor;
        }
        /// <summary>
        /// Draws the text entry feild to the screen, draws in reverse from right to left, to enable easier text overflow.
        /// </summary>
        /// <param name="g">the graphics object to draw to</param>
        public override void Draw(CompactGraphics g)
        {
            int diffrence = (Bounds.x2 - Bounds.x1);
            int iDif;
            for (int i = Bounds.x2; i >= Bounds.x1; i--)
            {
                iDif = i - Bounds.x1;
                // if the length of the text is greater than the width of the text box offset the 0 point by the overlap.
                if(text.Length > diffrence)
                {
                    g.Draw(text[iDif+(text.Length - diffrence-1)], forColor, i, Bounds.y1);
                }
                else
                {
                    //otherwise draw either the text of the unflled character.
                    if(iDif < text.Length)
                    {
                        g.Draw(text[iDif], forColor, i, Bounds.y1);
                    }
                    else
                    {
                      g.Draw('_', forColor, i, Bounds.y1);
                    }
                }
                
            }
        }
        /// <summary>
        /// Update the text entry with a key input then draw to the screen buffer.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="keyInfo"></param>
        public override void Draw(CompactGraphics g, ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key != ConsoleKey.Backspace)
            {
                text += keyInfo.KeyChar;
            }
            else if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if(text.Length > 0)
                    text = text.Remove(text.Length - 1);
            }
            Draw(g);
        }

    }
}
