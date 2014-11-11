using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItem("Cutscene", "Stop Cutscene")]
    public class StopCutscene : CinemaGlobalEvent
    {

        public Cutscene cutscene;

        public override void Trigger()
        {
            if (cutscene != null)
            {
                cutscene.Stop();
            }
        }
    }
}