using UnityEngine;
using System.Collections;

namespace CinemaDirector
{
    [CutsceneItemAttribute("Audio Source", "Play Clip At Point")]
    public class PlayClipAtPointAudioEvent : CinemaGlobalEvent
    {
        public AudioClip Clip;
        public Vector3 Position;
        public float VolumeScale = 1f;

        public override void Trigger()
        {
            AudioSource.PlayClipAtPoint(Clip, Position, VolumeScale);
        }

    }
}