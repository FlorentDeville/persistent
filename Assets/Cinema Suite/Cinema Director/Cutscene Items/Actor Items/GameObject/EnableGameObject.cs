using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItemAttribute("Game Object", "Enable")]
    public class EnableGameObject : CinemaActorEvent
    {
        private bool cachedState;

        public override void Initialize(GameObject actor)
        {
            if (actor != null)
            {
                cachedState = actor.activeSelf;
            }
        }

        public override void Trigger(GameObject actor)
        {
            actor.SetActive(true);
        }

        public override void Reverse(GameObject actor)
        {
            actor.SetActive(false);
        }

        public override void Stop(GameObject actor)
        {
            if (actor != null)
            {
                actor.SetActive(cachedState);
            }
        }
    }
}