using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItemAttribute("Physics", "Toggle Gravity")]
    public class ToggleGravityEvent : CinemaActorEvent
    {
        public override void Trigger(GameObject actor)
        {
            Rigidbody affectedObjectRigidBody = actor.GetComponent<Rigidbody>();

            if (affectedObjectRigidBody != null)
            {
                affectedObjectRigidBody.useGravity = !affectedObjectRigidBody.useGravity;
            }
        }
    }
}