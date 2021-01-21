using System;
using System.Collections.Generic;
using System.Text;

namespace ComapactGraphicsV2
{
    public class DrawAction : Widget
    {
        private bool isIteractable;
        private Action action;
        private Action<Input.inpuT> inputAction;
        public DrawAction(Action onDraw)
        {
            isIteractable = false;
            action = onDraw;
        }
        public DrawAction(Action<Input.inpuT> inputAction)
        {
            isIteractable = true;
            this.inputAction = inputAction;
        }
        public override void Draw(CompactGraphics g)
        {
            action();
        }
        public override void Draw(CompactGraphics g, Input.inpuT input)
        {
            if (isIteractable)
                inputAction(input);
            else
                action();
        }
    }
}
