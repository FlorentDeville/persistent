using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.UI
{
    public enum WidgetEvent
    {
        Up,
        Down,
        Left,
        Right,
        Submit,
        Cancel,
        Select,
        Unselect,

        //All new widget event has to be defined before Count.
        Count
    }
}
