using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItem("Debug", "Log Error")]
    public class DebugLogErrorEvent : CinemaGlobalEvent
    {

        public string message = "Error Message";

        public override void Trigger()
        {
            Debug.LogError(message);
        }

        public override void Reverse()
        {
            Debug.LogError(string.Format("Reverse: {0}", message));
        }
    }
}