using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItemAttribute("Game Object", "Disable Game Object")]
    public class DisableGameObjectGlobal : CinemaGlobalEvent
    {
        public GameObject target;
        private bool cachedState;
        private bool previousState;

        public override void Initialize()
        {
            if (target != null)
            {
                cachedState = target.activeInHierarchy;
            }
        }

        public override void Trigger()
        {
            if (target != null)
            {
                previousState = target.activeInHierarchy;
                target.SetActive(false);
            }
        }

        public override void Reverse()
        {
            if (target != null)
            {
                target.SetActive(previousState);
            }
        }

        public override void Stop()
        {
            target.SetActive(cachedState);
        }
    }
}