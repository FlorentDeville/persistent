using System.Reflection;
using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItemAttribute("Game Object", "Disable Behaviour")]
    public class DisableBehaviour : CinemaActorEvent
    {
        public Component Behaviour;
        private bool cachedState;

        public override void Initialize(GameObject actor)
        {
            if (actor != null)
            {
                Component b = actor.GetComponent(Behaviour.GetType()) as Component;
                if (b != null)
                {
                    PropertyInfo fieldInfo = Behaviour.GetType().GetProperty("enabled");
                    cachedState = (bool)fieldInfo.GetValue(b, null);
                }
            }
        }

        public override void Trigger(GameObject actor)
        {
            Component b = actor.GetComponent(Behaviour.GetType()) as Component;
            if (b != null)
            {
                Behaviour.GetType().GetMember("enabled");

                PropertyInfo fieldInfo = Behaviour.GetType().GetProperty("enabled");
                fieldInfo.SetValue(Behaviour, false, null);
            }
        }

        public override void Reverse(GameObject actor)
        {
            Component b = actor.GetComponent(Behaviour.GetType()) as Component;
            if (b != null)
            {
                PropertyInfo fieldInfo = Behaviour.GetType().GetProperty("enabled");
                fieldInfo.SetValue(b, true, null);
            }
        }

        public override void Stop(GameObject actor)
        {
            if (actor != null)
            {
                Component b = actor.GetComponent(Behaviour.GetType()) as Component;
                if (b != null)
                {
                    PropertyInfo fieldInfo = Behaviour.GetType().GetProperty("enabled");
                    fieldInfo.SetValue(b, cachedState, null);
                }
            }
        }
    }
}