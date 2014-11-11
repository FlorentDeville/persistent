using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItemAttribute("Physics", "Apply Force")]
    public class ApplyForceEvent : CinemaActorEvent
    {
        public Vector3 Force = Vector3.forward;
        public ForceMode ForceMode = ForceMode.Impulse;

        public override void Trigger(GameObject actor)
        {
            Rigidbody affectedObjectRigidBody = actor.GetComponent<Rigidbody>();

            if (affectedObjectRigidBody != null)
            {
                affectedObjectRigidBody.AddForce(Force, ForceMode);
            }
        }
    }
}