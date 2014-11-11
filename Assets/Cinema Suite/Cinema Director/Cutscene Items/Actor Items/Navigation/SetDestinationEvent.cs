using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItemAttribute("Navigation", "Set Destination")]
    public class SetDestinationEvent : CinemaActorEvent
    {
        public Vector3 target;
        public override void Trigger(GameObject actor)
        {
            NavMeshAgent agent = actor.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.SetDestination(target);
            }
        }

        public override void Reverse(GameObject actor)
        {
        }
    }
}