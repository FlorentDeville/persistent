using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItemAttribute("Game Object", "Enable Game Object")]
    public class EnableGameObjectGlobal : CinemaGlobalEvent
    {
        public GameObject target;
        private bool cachedState;
        private bool previousState;

        public override void Initialize()
        {
            if (target != null)
            {
                cachedState = target.activeInHierarchy;
                previousState = cachedState;
            }
        }

        public override void Trigger()
        {
            if (target != null)
            {
                previousState = target.activeInHierarchy;
                target.SetActive(true);
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