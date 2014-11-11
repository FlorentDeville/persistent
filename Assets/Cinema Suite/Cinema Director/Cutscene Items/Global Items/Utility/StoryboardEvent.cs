using System;
using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItem("Utility", "Storyboard")]
    public class StoryboardEvent : CinemaGlobalEvent
    {
        public static int Count = 0;
        public override void Trigger()
        {
            Application.CaptureScreenshot(string.Format(@"Assets\{0}{1}.png", this.gameObject.name, Count++));
        }
    }
}