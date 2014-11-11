using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItem("Cutscene", "Play Cutscene")]
    public class PlayCutscene : CinemaGlobalEvent
    {

        public Cutscene cutscene;

        public override void Trigger()
        {
            if (cutscene != null)
            {
                cutscene.Play();
            }
        }

    }
}