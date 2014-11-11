using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItemAttribute("Physics", "Apply Torque")]
    public class ApplyTorqueEvent : CinemaActorEvent
    {
        public Vector3 Torque = Vector3.forward;
        public ForceMode ForceMode = ForceMode.Impulse;

        public override void Trigger(GameObject actor)
        {
            Rigidbody affectedObjectRigidBody = actor.GetComponent<Rigidbody>();

            if (affectedObjectRigidBody != null)
            {
                affectedObjectRigidBody.AddTorque(Torque, ForceMode);
            }
        }
    }
}