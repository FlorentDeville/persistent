// Cinema Suite
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
    public delegate void ShotBeginsEventHandler(object sender, ShotEventArgs e);
    public delegate void ShotEndsEventHandler(object sender, ShotEventArgs e);

    public class ShotEventArgs : EventArgs
    {
        public CinemaShot shot;

        public ShotEventArgs(CinemaShot shot)
        {
            this.shot = shot;
        }
    }

    /// <summary>
    /// A track that sorts shots and manages associated cameras.
    /// </summary>
    public class ShotTrack : GlobalTrack
    {
        public event ShotBeginsEventHandler ShotBegins;

        public override void Initialize()
        {
            base.elapsedTime = 0f;
            foreach (CinemaShot shot in this.Shots)
            {
                shot.Initialize();
                if (shot.Firetime == 0)
                {
                    shot.Trigger();
                }
                else
                {
                    shot.End();
                }
            }
        }

        public override void UpdateTrack(float time, float deltaTime)
        {
            float previousTime = base.elapsedTime;
            base.elapsedTime = time;
            foreach (CinemaShot shot in this.Shots)
            {
                float endTime = shot.CutTime + shot.Duration;
                if ((previousTime <= shot.CutTime) && (base.elapsedTime >= shot.CutTime) && (base.elapsedTime < endTime))
                {
                    shot.Trigger();
                    if (ShotBegins != null)
                    {
                        ShotBegins(this, new ShotEventArgs(shot));
                    }
                }
                else if ((previousTime >= endTime) && (base.elapsedTime < endTime) && (base.elapsedTime >= shot.CutTime))
                {
                    shot.Trigger();
                    if (ShotBegins != null)
                    {
                        ShotBegins(this, new ShotEventArgs(shot));
                    }
                }
                else if ((previousTime >= shot.CutTime) && (previousTime < endTime) && (base.elapsedTime > endTime))
                {
                    shot.End();
                }
                else if ((previousTime > shot.CutTime) && (previousTime < endTime) && (base.elapsedTime < shot.CutTime))
                {
                    shot.End();
                }
            }
        }

        public override void SetTime(float time)
        {
            elapsedTime = time;

            foreach (CinemaShot shot in this.Shots)
            {
                float endTime = shot.CutTime + shot.Duration;
                if ((elapsedTime >= shot.CutTime) && (elapsedTime < endTime))
                {
                    shot.Trigger();
                    if (ShotBegins != null)
                    {
                        ShotBegins(this, new ShotEventArgs(shot));
                    }
                }
                else
                {
                    shot.End();
                }
            }
        }

        public override void Stop()
        {
            foreach (CinemaShot shot in this.Shots)
            {
                shot.Stop();
            }
        }

        public CinemaShot[] GetSortedShots()
        {
            CinemaShot[] shots = GetComponentsInChildren<CinemaShot>();
            Array.Sort<CinemaShot>(shots);

            return shots;
        }

        public CinemaShot[] Shots
        {
            get
            {
                return GetComponentsInChildren<CinemaShot>();
            }
        }

        public override TimelineItem[] TimelineItems
        {
            get
            {
                return GetComponentsInChildren<CinemaShot>();
            }
        }
    }
}