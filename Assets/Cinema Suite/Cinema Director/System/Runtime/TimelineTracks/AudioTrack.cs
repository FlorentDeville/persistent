using System;
using System.Collections;
using UnityEngine;

namespace CinemaDirector
{
    public class AudioTrack : GlobalTrack
    {
        public override void SetTime(float time)
        {
            foreach (CinemaAudio cinemaAudio in AudioClips)
            {
                float audioTime = time - cinemaAudio.Firetime;
                cinemaAudio.SetTime(audioTime);
            }
        }

        public override void Pause()
        {
            foreach (CinemaAudio cinemaAudio in AudioClips)
            {
                cinemaAudio.Pause();
            }
        }

        public override void UpdateTrack(float time, float deltaTime)
        {
            float elapsedTime = base.elapsedTime;
            base.elapsedTime = time;

            foreach (CinemaAudio cinemaAudio in AudioClips)
            {
                if (cinemaAudio != null)
                {
                    if (((elapsedTime < cinemaAudio.Firetime) || (elapsedTime <= 0f)) && (((base.elapsedTime >= cinemaAudio.Firetime))))
                    {
                        cinemaAudio.Trigger();
                    }
                    if ((base.elapsedTime > cinemaAudio.Firetime) && (base.elapsedTime <= (cinemaAudio.Firetime + cinemaAudio.Duration)))
                    {
                        float audioTime = time - cinemaAudio.Firetime;
                        cinemaAudio.UpdateTime(audioTime, deltaTime);
                    }
                    if (((elapsedTime <= (cinemaAudio.Firetime + cinemaAudio.Duration)) && (base.elapsedTime > (cinemaAudio.Firetime + cinemaAudio.Duration))))
                    {
                        cinemaAudio.End();
                    }
                }
            }
        }

        public override void Resume()
        {
            foreach (CinemaAudio cinemaAudio in this.AudioClips)
            {
                if (((base.Cutscene.RunningTime > cinemaAudio.Firetime)) && (base.Cutscene.RunningTime < (cinemaAudio.Firetime + cinemaAudio.Duration)))
                {
                    cinemaAudio.Resume();
                }
            }
        }

        public override void Stop()
        {
            base.elapsedTime = 0f;
            foreach (CinemaAudio cinemaAudio in AudioClips)
            {
                cinemaAudio.Stop();
            }
        }

        /// <summary>
        /// Get all cinema audio objects associated with this audio track
        /// </summary>
        public CinemaAudio[] AudioClips
        {
            get
            {
                return GetComponentsInChildren<CinemaAudio>();
            }
        }
    }
}