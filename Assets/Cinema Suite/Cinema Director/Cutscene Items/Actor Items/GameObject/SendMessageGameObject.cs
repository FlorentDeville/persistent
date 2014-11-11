using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItemAttribute("Game Object", "Send Message")]
    public class SendMessageGameObject : CinemaActorEvent
    {
        public string MethodName = string.Empty;
        public object Parameter = null;
        public SendMessageOptions SendMessageOptions = SendMessageOptions.DontRequireReceiver;

        public override void Trigger(GameObject actor)
        {
            actor.SendMessage(MethodName, Parameter, SendMessageOptions);
        }

    }
}