using UnityEngine;
using System.Collections;

namespace CinemaDirector
{
    [CutsceneItem("GUITexture", "Fade Texture")]
    public class FadeTexture : CinemaGlobalAction
    {
        public GUITexture target;
        public Color tint = Color.grey;

        void Awake()
        {
            if (this.target != null)
            {
                this.target.enabled = false;
                this.target.color = Color.clear;
            }
        }

        public override void Trigger()
        {
            if (this.target != null)
            {
                this.target.enabled = true;
                this.target.color = Color.clear;
            }
        }

        public override void ReverseTrigger()
        {
            End();
        }

        public override void UpdateTime(float time, float deltaTime)
        {
            if (this.target != null)
            {
                this.target.enabled = true;
                float transition = time / Duration;
                if (transition <= 0.25f)
                {
                    FadeToColor(Color.clear, tint, (transition / 0.25f));
                }
                else if (transition >= 0.75f)
                {
                    FadeToColor(tint, Color.clear, (transition - 0.75f) / .25f);
                }
            }
        }

        public override void SetTime(float time, float deltaTime)
        {
            if (this.target != null)
            {
                if (time >= 0 && time <= Duration)
                {
                    UpdateTime(time, deltaTime);
                }
                else if (target.enabled)
                {
                    this.target.enabled = false;
                }
            }
        }

        public override void End()
        {
            if (this.target != null)
            {
                this.target.enabled = false;
            }
        }

        public override void ReverseEnd()
        {
            Trigger();
        }

        public override void Stop()
        {
            if (this.target != null)
            {
                this.target.enabled = false;
            }
        }

        private void FadeToColor(Color from, Color to, float transition)
        {
            this.target.color = Color.Lerp(from, to, transition);
        }
    }
}