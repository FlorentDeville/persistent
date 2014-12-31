using Assets.Scripts.Helper;
using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class CameraStateFollowing : IFSMState<CameraBehavior>
	{
		float m_angleY;

        DamperVector3 m_PositionDamper;

        public override int State { get { return (int)CameraState.eFollowing; } }

        public override void Initialize()
        {
            m_angleY = 0;
            m_PositionDamper = new DamperVector3(m_Behavior.m_DampingSpeed);
        }
		
		public override void OnEnter ()
		{
            m_angleY = m_Behavior.m_InitialOrientation;
		}
		
		public override void OnExecute ()
		{
			//Compute rotation of camera
			float input = Input.GetAxis("Camera_Horizontal");
			m_angleY += input * m_Behavior.m_CameraRotationSpeed;
			
			Quaternion q = Quaternion.AngleAxis(m_angleY, new Vector3(0, 1, 0));
			Transform targetTransform = m_Behavior.m_Player_GameObject.transform;
			Vector3 front = q * targetTransform.forward;
			
            //Compute position of camera
			Vector3 pos = targetTransform.position 
				- front * m_Behavior.m_CameraDistance 
				+ Vector3.up * m_Behavior.m_CameraHeight;
			
			m_GameObject.transform.position = pos;
            m_GameObject.transform.LookAt(targetTransform.position);

            //Apply transparency to obstacles
            //ApplyAlpha(pos, targetTransform.position);
		}

        private void ApplyAlpha(Vector3 _from, Vector3 _to)
        {
            const float MIN_DISTANCE = 1f;

            Vector3 rayStart = _from;
            Vector3 rayDirection = _to - _from;
            while (rayDirection.sqrMagnitude > MIN_DISTANCE)
            {
                RaycastHit castResult = new RaycastHit();
                Ray obstacleRay = new Ray(rayStart, rayDirection.normalized);

                Debug.DrawRay(rayStart, rayDirection, Color.red);
                if (!Physics.Raycast(obstacleRay, out castResult, rayDirection.magnitude))
                    return;

                GameObject obj = castResult.collider.gameObject;
                while (obj != null)
                {
                    RendererController ctrl = obj.GetComponent<RendererController>();
                    if (ctrl != null)
                    {
                        ctrl.ApplyAlpha(m_Behavior.m_GameObjectAlpha);
                        obj = null;
                    }
                    else
                    {
                        if (obj.transform.parent != null)
                            obj = obj.transform.parent.gameObject;
                        else
                            obj = null;
                    }
                }

                rayStart = rayStart + rayDirection.normalized * (castResult.distance + 0.01f);
                rayDirection = _to - rayStart;
            }
        }
	}
}

