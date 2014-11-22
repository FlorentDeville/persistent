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
            float amountV = 0;
            float amountH = 0;
            if (m_Behavior.m_InputEnabled)
            {
                amountV = Input.GetAxis("Vertical");
                amountH = Input.GetAxis("Horizontal");
            }

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
            const float GRAVITY = 9.81f;
            Vector3 forward = camera.transform.forward;
            forward.y = 0;
			Vector3 displacement = forward * input.normalized.z * speed
				+ camera.transform.right * input.normalized.x * speed 
                - Vector3.up * GRAVITY;

            if(m_Behavior.m_OutlinedMaterialEnabled)
                ApplyShader(camera);

            CharacterController ctrl = m_GameObject.GetComponent<CharacterController>();
            ctrl.Move(displacement);

			base.OnExecute ();
		}

        private void ApplyShader(GameObject _camera)
        {
            Vector3 rayDirection = _camera.transform.position - m_GameObject.transform.position;
            Ray rayPlayerToCamera = new Ray(m_Behavior.gameObject.transform.position, rayDirection.normalized);

            Debug.DrawLine(m_GameObject.transform.position, _camera.transform.position);
            float sphereCastRadius = 0.1f;

            if (Physics.SphereCast(rayPlayerToCamera, sphereCastRadius, rayDirection.magnitude))
                m_GameObject.renderer.material = m_Behavior.m_OutlinedMaterial;
            else
                m_GameObject.renderer.material = m_Behavior.m_DefaultMaterial;
        }

	}
}