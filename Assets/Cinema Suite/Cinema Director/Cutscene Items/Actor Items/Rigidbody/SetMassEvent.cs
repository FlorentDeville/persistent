using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItemAttribute("Physics", "Set Mass")]
    public class SetMassEvent : CinemaActorEvent
    {
        public float Mass = 1f;

        public override void Trigger(GameObject actor)
        {
            Rigidbody affectedObjectRigidBody = actor.GetComponent<Rigidbody>();

            if (affectedObjectRigidBody != null)
            {
                affectedObjectRigidBody.mass = Mass;
            }
        }
    }
}