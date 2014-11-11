using UnityEngine;
using System.Collections;

using AssemblyCSharp;

namespace Persistent
{
	public class PlayerStateWorld : IFSMState 
	{
		PlayerBehavior m_Behavior;
		
		public PlayerStateWorld (FSMRunner _Runner, GameObject _obj)
		{
			m_Runner = _Runner;
			m_GameObject = _obj;
			m_State = (int)PlayerState.eWorld;
			m_Behavior = m_GameObject.GetComponent<PlayerBehavior>();
		}
		
		public override void OnEnter ()
		{
			
		}
		
		public override void OnExecute ()
		{
			//Get the joystick inputs
			float amountV = Input.GetAxis("Vertical");
			float amountH = Input.GetAxis("Horizontal");
			
			Vector3 input = new Vector3(amountH, 0, amountV);
			
			float speed = 0;
			if(input.magnitude > 0.99f)
				speed = m_Behavior.m_NormalSpeed;
			else
				speed = m_Behavior.m_SlowSpeed;
			
			//Get the camera frame
			GameObject camera = GameObject.Find("Main Camera");
			if(camera == null)
			{
				Debug.LogError("Can't find game object called Main Camera");
				Debug.DebugBreak();
			}

			//Compute the displacment
            Vector3 forward = camera.transform.forward;
            forward.y = 0;
			Vector3 displacement = forward * input.normalized.z * speed
				+ camera.transform.right * input.normalized.x * speed;

            CharacterController ctrl = m_GameObject.GetComponent<CharacterController>();
            ctrl.Move(displacement);

			base.OnExecute ();
		}
	}
}