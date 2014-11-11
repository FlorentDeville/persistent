using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItemAttribute("Physics", "Sleep")]
    public class RigidbodySleepEvent : CinemaActorEvent
    {
        public override void Trigger(GameObject actor)
        {
            Rigidbody rb = actor.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.Sleep();
            }
        }

        public override void Reverse(GameObject actor)
        {
            Rigidbody rb = actor.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.WakeUp();
            }
        }
    }
}