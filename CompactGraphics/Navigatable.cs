using System;
using System.Collections.Generic;
using System.Text;

namespace ComapactGraphicsV2
{
    public interface Navigatable
    {

        bool AwaitingNav();
        Type NavDestination(object state);

        void Step(Input.inpuT input, CompactGui G);
    }
}
