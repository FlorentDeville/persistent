using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItem("Cutscene", "Pause Cutscene")]
    public class PauseCutscene : CinemaGlobalEvent
    {

        public Cutscene cutscene;

        public override void Trigger()
        {
            if (cutscene != null)
            {
                cutscene.Pause();
            }
        }
    }
}