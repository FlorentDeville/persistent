using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItem("Utility", "Load Level")]
    public class LoadLevelEvent : CinemaGlobalEvent
    {
        public int Level = 0;
        public override void Trigger()
        {
            if (Application.isPlaying)
            {
                Application.LoadLevel(Level);
            }
        }

        public override void Reverse()
        {
        }
    }
}