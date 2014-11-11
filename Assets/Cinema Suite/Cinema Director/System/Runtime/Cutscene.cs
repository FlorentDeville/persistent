// Cinema Suite
using System;
using System.Collections;
using UnityEngine;

namespace CinemaDirector
{
    /// <summary>
    /// The Cutscene is the main Behaviour of Cinema Director.
    /// </summary>
    [ExecuteInEditMode, Serializable]
    public class Cutscene : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private float duration = 30f; // Duration of cutscene in seconds.

        [SerializeField]
        private float runningTime = 0f; // Running time of the cutscene in seconds.

        [SerializeField]
        public float playbackSpeed = 1f; // Multiplier for playback speed.

        [SerializeField]
        private CutsceneState state = CutsceneState.Inactive;

        // Keeps track of the previous time an update was made.
        private float previousTime;

        #endregion

        // Event fired when Cutscene's runtime reaches it's duration.
        public event CutsceneHandler CutsceneFinished;

        // Event fired when Cutscene has been paused.
        public event CutsceneHandler CutscenePaused;

        /// <summary>
        /// Preview play readies the cutscene to be played using UpdateCutscene(). Play() should be used in most cases.
        /// This is necessary for playing the cutscene in edit mode.
        /// </summary>
        public void PreviewPlay()
        {
            if (state == CutsceneState.Inactive)
            {
                EnterPreviewMode();
            }
            else if (state == CutsceneState.Paused)
            {
                resume();
            }

            state = CutsceneState.PreviewPlaying;
        }

        /// <summary>
        /// Plays/Resumes the cutscene from inactive/paused states using a Coroutine.
        /// </summary>
        public void Play()
        {
            if (state == CutsceneState.Inactive)
            {
                state = CutsceneState.Playing;
                initialize();
                StartCoroutine("updateCoroutine");
            }
            else if (state == CutsceneState.Paused)
            {
                state = CutsceneState.Playing;
                StartCoroutine("updateCoroutine");
            }
        }

        /// <summary>
        /// Pause the playback of this cutscene.
        /// </summary>
        public void Pause()
        {
            if (state == CutsceneState.Playing)
            {
                StopCoroutine("updateCoroutine");
            }
            if (state == CutsceneState.PreviewPlaying || state == CutsceneState.Playing || state == CutsceneState.Scrubbing)
            {
                foreach (TrackGroup trackGroup in this.TrackGroups)
                {
                    trackGroup.Pause();
                }
            }
            state = CutsceneState.Paused;

            if (CutscenePaused != null)
            {
                CutscenePaused(this, new CutsceneEventArgs());
            }
        }

        /// <summary>
        /// Stops the cutscene.
        /// </summary>
        public void Stop()
        {
            state = CutsceneState.Inactive;
            this.RunningTime = 0f;
            foreach (TrackGroup trackGroup in this.TrackGroups)
            {
                trackGroup.Stop();
            }

            if (state == CutsceneState.Playing)
            {
                StopCoroutine("updateCoroutine");
            }
        }

        /// <summary>
        /// Updates the cutscene by the amount of time passed.
        /// </summary>
        /// <param name="deltaTime">The delta in time between the last update call and this one.</param>
        public void UpdateCutscene(float deltaTime)
        {
            this.RunningTime += (deltaTime * playbackSpeed);

            foreach (TrackGroup group in this.TrackGroups)
            {
                group.UpdateTrackGroup(this.runningTime, deltaTime * playbackSpeed);
            }
            if (runningTime >= duration || runningTime < 0f)
            {
                if (CutsceneFinished != null)
                {
                    CutsceneFinished(this, new CutsceneEventArgs());
                }
                Debug.Log("Stop!" + runningTime);
                Stop();
            }
        }

        /// <summary>
        /// Play the cutscene from it's given running time to a new time
        /// </summary>
        /// <param name="newTime">The new time to make up for</param>
        public void ScrubToTime(float newTime)
        {
            float deltaTime = newTime - this.RunningTime;

            state = CutsceneState.Scrubbing;
            UpdateCutscene(deltaTime);
            
        }

        /// <summary>
        /// Set the cutscene to the state of a given new running time and do not proceed to play the cutscene
        /// </summary>
        /// <param name="time">The new running time to be set.</param>
        public void SetRunningTime(float time)
        {
            this.RunningTime = time;
            foreach (TrackGroup group in this.TrackGroups)
            {
                group.SetRunningTime(RunningTime);
            }
        }

        /// <summary>
        /// Set the cutscene into an active state.
        /// </summary>
        public void EnterPreviewMode()
        {
            if (state == CutsceneState.Inactive)
            {
                initialize();
                foreach (TrackGroup group in this.TrackGroups)
                {
                    group.SetRunningTime(RunningTime);
                }
                state = CutsceneState.Paused;
            }
        }

        /// <summary>
        /// Set the cutscene into an inactive state.
        /// </summary>
        public void ExitPreviewMode()
        {
            Stop();
        }

        /// <summary>
        /// The duration of this cutscene in seconds.
        /// </summary>
        public float Duration
        {
            get
            {
                return this.duration;
            }
            set
            {
                this.duration = value;
                if (this.duration <= 0f)
                {
                    this.duration = 0.1f;
                }
            }
        }

        /// <summary>
        /// Returns true if this cutscene is currently playing.
        /// </summary>
        public CutsceneState State
        {
            get
            {
                return this.state;
            }
        }

        /// <summary>
        /// The current running time of this cutscene in seconds. Values are restricted between 0 and duration.
        /// </summary>
        public float RunningTime
        {
            get
            {
                return this.runningTime;
            }
            set
            {
                runningTime = Mathf.Clamp(value, 0, duration);
            }
        }

        /// <summary>
        /// Get all track groups in this cutscene.
        /// </summary>
        public TrackGroup[] TrackGroups
        {
            get
            {
                return base.GetComponentsInChildren<TrackGroup>();
            }
        }

        /// <summary>
        /// Get the director group of this cutscene.
        /// </summary>
        public DirectorGroup DirectorGroup
        {
            get
            {
                return base.GetComponentInChildren<DirectorGroup>();
            }
        }

        /// <summary>
        /// Cutscene state is used to determine if the cutscene is currently Playing, Previewing (In edit mode), paused or inactive.
        /// </summary>
        public enum CutsceneState
        {
            Inactive,
            Playing,
            PreviewPlaying,
            Scrubbing,
            Paused
        }

        /// <summary>
        /// An important call to make before sampling the cutscene, to initialize all track groups 
        /// and save states of all actors/game objects.
        /// </summary>
        private void initialize()
        {
            foreach (TrackGroup trackGroup in this.TrackGroups)
            {
                trackGroup.Initialize();
            }
        }

        /// <summary>
        /// Couroutine to call UpdateCutscene while the cutscene is in the playing state.
        /// </summary>
        /// <returns></returns>
        private IEnumerator updateCoroutine()
        {
            while (state == CutsceneState.Playing)
            {
                UpdateCutscene(Time.deltaTime);
                yield return null;
            }
        }

        /// <summary>
        /// Prepare all track groups for resuming from pause.
        /// </summary>
        private void resume()
        {
            foreach (TrackGroup group in this.TrackGroups)
            {
                group.Resume();
            }
        }
    }

    // Delegate for handling Cutscene Events
    public delegate void CutsceneHandler(object sender, CutsceneEventArgs e);

    /// <summary>
    /// Cutscene event arguments. Blank for now.
    /// </summary>
    public class CutsceneEventArgs : EventArgs
    {
        public CutsceneEventArgs()
        {
        }
    }
}