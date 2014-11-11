using UnityEngine;
using System.Collections;

namespace CinemaDirector
{
    [CutsceneItem("Transitions", "Fade from Black")]
    public class FadeFromBlack : CinemaGlobalAction
    {
        private Color From = Color.black;
        private Color To = Color.clear;

        void Awake()
        {
            if (guiTexture == null)
            {
                gameObject.transform.position = Vector3.zero;
                gameObject.transform.localScale = new Vector3(100, 100, 100);
                gameObject.AddComponent<GUITexture>();
                guiTexture.enabled = false;
                guiTexture.texture = new Texture2D(1, 1);
                guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
                guiTexture.color = Color.clear;
            }
        }

        public override void Trigger()
        {
            guiTexture.enabled = true;
            guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
            guiTexture.color = From;
        }

        public override void ReverseTrigger()
        {
            End();
        }

        public override void UpdateTime(float time, float deltaTime)
        {
            guiTexture.enabled = true;
            float transition = time / Duration;
            FadeToColor(From, To, transition);
        }

        public override void SetTime(float time, float deltaTime)
        {
            if (time >= 0 && time <= Duration)
            {
                UpdateTime(time, deltaTime);
            }
            else if (guiTexture.enabled)
            {
                guiTexture.enabled = false;
            }
        }

        public override void End()
        {
            guiTexture.enabled = false;
        }

        public override void ReverseEnd()
        {
            Trigger();
        }

        public override void Stop()
        {
            if (guiTexture != null)
            {
                guiTexture.enabled = false;
            }
        }

        private void FadeToColor(Color from, Color to, float transition)
        {
            guiTexture.color = Color.Lerp(from, to, transition);
        }
    }
}
