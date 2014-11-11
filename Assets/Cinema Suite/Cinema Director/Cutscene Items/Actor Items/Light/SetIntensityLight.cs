using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItemAttribute("Light", "Set Intensity")]
    public class SetIntensityLight : CinemaActorEvent
    {
        public float Intensity;
        private float cachedIntensity;
        private float previousIntensity;

        public override void Initialize(GameObject actor)
        {
            Light light = actor.GetComponent<Light>();
            if (light != null)
            {
                previousIntensity = light.intensity;
                cachedIntensity = light.intensity;
            }
        }

        public override void Trigger(GameObject actor)
        {
            Light light = actor.GetComponent<Light>();
            if (light != null)
            {
                previousIntensity = light.intensity;
                light.intensity = Intensity;
            }
        }

        public override void Reverse(GameObject actor)
        {
            Light light = actor.GetComponent<Light>();
            if (light != null)
            {
                light.intensity = previousIntensity;
            }
        }

        public override void Stop(GameObject actor)
        {
            Light light = actor.GetComponent<Light>();
            if (light != null)
            {
                light.intensity = cachedIntensity;
            }
        }
    }
}