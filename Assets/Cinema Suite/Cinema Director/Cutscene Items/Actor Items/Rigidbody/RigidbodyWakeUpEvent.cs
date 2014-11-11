using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItemAttribute("Physics", "Wake Up")]
    public class RigidbodyWakeUpEvent : CinemaActorEvent
    {
        public override void Trigger(GameObject actor)
        {
            Rigidbody rb = actor.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.WakeUp();
            }
        }

        public override void Reverse(GameObject actor)
        {
            Rigidbody rb = actor.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.Sleep();
            }
        }
    }
}