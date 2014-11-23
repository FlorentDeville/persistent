using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class CameraStateFixed : IFSMState<CameraBehavior>
	{
        public override int State { get { return (int)CameraState.eFixed; } }
	}
}

