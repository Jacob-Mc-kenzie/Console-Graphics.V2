using System;
using System.Collections.Generic;
using System.Text;

namespace ComapactGraphicsV2
{
    public class DrawAction : Widget
    {
        private Action action;
        private Action<Input.inpuT> inputAction;
        public DrawAction(Action onDraw)
        {
            action = onDraw;
        }
        public DrawAction(Action<Input.inpuT> inputAction)
        {
            this.inputAction = inputAction;
        }
        public override void Draw(CompactGraphics g)
        {
            action();
        }
        public override void Draw(CompactGraphics g, Input.inpuT input)
        {
            inputAction(input);
        }
    }
}
