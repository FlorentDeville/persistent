using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Entities.World
{
    public enum ActionId
    {
        //Unknown has to be the first, add new enum after it
        Unknown,

        Attack,
        Fire,
        Slow,
        Ice,

        //Count has to be the last
        Count
    }
}
