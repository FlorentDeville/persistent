using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItemAttribute("Light", "Set Light Colour")]
    public class SetLightColour : CinemaActorEvent
    {
        public Color Color;
        private Color cachedColor;
        private Color previousColor;

        public override void Initialize(GameObject actor)
        {
            Light light = actor.GetComponent<Light>();
            if (light != null)
            {
                previousColor = light.color;
                cachedColor = light.color;
            }
        }

        public override void Trigger(GameObject actor)
        {
            Light light = actor.GetComponent<Light>();
            if (light != null)
            {
                previousColor = light.color;
                light.color = Color;
            }
        }

        public override void Reverse(GameObject actor)
        {
            Light light = actor.GetComponent<Light>();
            if (light != null)
            {
                light.color = previousColor;
            }
        }

        public override void Stop(GameObject actor)
        {
            Light light = actor.GetComponent<Light>();
            if (light != null)
            {
                light.color = cachedColor;
            }
        }
    }
}