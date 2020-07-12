namespace CompactGraphics
{

    public class Widget
    {
        /// <summary>
        /// A empty constructor, so you don't have to call base if you don't want a white rectangle.
        /// </summary>
        public Widget()
        {

        }
        public Widget(Rect bounds)
        {
            this.baseBounds = bounds;
            this.Bounds = bounds;
            this.rendered = new TFrame(bounds.x2 - bounds.x1, bounds.y2 - bounds.y1);
            this.forColor = System.ConsoleColor.White;

        }
        /// <summary>
        /// The point to align to on widgets.
        /// </summary>
        public enum DrawPoint { TopLeft, Top, TopRight, Left, Center, Right, BottomLeft, Bottom, BottomRight }
        /// <summary>
        /// Unused, intended for mouse interaction.
        /// </summary>
        public enum State { Up, Down, Selected, None };
        protected State state;
        /// <summary>
        /// The forground color of a widget.
        /// </summary>
        protected System.ConsoleColor forColor;
        /// <summary>
        /// The actual allignment of the widget.
        /// </summary>
        public DrawPoint Pin { get; protected set; }
        /// <summary>
        /// The current location of the widget bounds.
        /// </summary>
        public Rect Bounds { get; protected set; }
        /// <summary>
        /// The initilised bounds, un shifted by pinning.
        /// </summary>
        protected Rect baseBounds;
        /// <summary>
        /// Un-used, intended to speed up draw times of infrequently updated widgets with complex draw cycles.
        /// </summary>
        internal TFrame rendered;
        /// <summary>
        /// Update the forground color of a widget.
        /// </summary>
        /// <param name="color">The color to change to</param>
        public virtual void SetColor(System.ConsoleColor color)
        {
            this.forColor = color;
        }
        /// <summary>
        /// Draws the widget to the current frame of the Graphics object
        /// </summary>
        /// <param name="g">The Graphics to draw to</param>
        public virtual void Draw(Graphics g)
        {
            g.Draw(rendered, Bounds.x1, Bounds.y1);
        }
        /// <summary>
        /// Draws the widget to the current frame, after evaluating updating input.
        /// </summary>
        /// <param name="g">The Graphics to draw to</param>
        /// <param name="keyInfo">The User input to be handled.</param>
        public virtual void Draw(Graphics g, System.ConsoleKeyInfo keyInfo)
        {
            throw new System.NotImplementedException();
        }
        public virtual void Draw(Graphics g, Mouse mouse)
        {
            Draw(g);
            //throw new System.NotImplementedException();
        }
        /// <summary>
        /// Using the initilised bounds, re-align the widget.
        /// </summary>
        /// <param name="point">The direction to allign to</param>
        public virtual void PinTo(DrawPoint point)
        {
            this.Pin = point;
            this.Bounds = baseBounds.OffsetPin(Pin);
        }
        /// <summary>
        /// Modify the Initilised bounds, new bounds will be calculated based on the curent Pin
        /// </summary>
        /// <param name="rect">The new bounds</param>
        public virtual void ReSize(Rect rect)
        {
            this.baseBounds = rect;
            this.Bounds = rect.OffsetPin(this.Pin);
        }
    }
}
