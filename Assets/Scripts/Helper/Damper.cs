using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Scripts.Helper
{
    public class DamperVector3
    {
        public Vector3 CurrentPos { get; set; }

        public Vector3 IdealPos { get; set; }

        public float Speed { get; set; }

        public DamperVector3(float _speed)
        {
            Speed = _speed;
        }

        public Vector3 Execute(float _dt)
        {
			Vector3 dir = IdealPos - CurrentPos;

			float param = (_dt * Speed) / dir.magnitude;
			if(param > 1) param = 1;
            if (param < 0) param = 0;
            return CurrentPos + (dir * param);
        }

    }

    public class DamperFloat
    {
        public float CurrentValue { get; set; }

        public float IdealValue { get; set; }

        public float Speed { get; set; }

        public DamperFloat(float _speed)
        {
            Speed = _speed;
        }

        public float ComputeValue(float _dt)
        {
            float dir = IdealValue - CurrentValue;

            float param = 0;
            
            if(IdealValue < CurrentValue)
                param = (_dt * Speed) / -dir;
            else
                param = (_dt * Speed) / dir;

            if (param > 1) param = 1;
            if (param < 0) param = 0;
            return CurrentValue + (dir * param);
        }
    }
}
