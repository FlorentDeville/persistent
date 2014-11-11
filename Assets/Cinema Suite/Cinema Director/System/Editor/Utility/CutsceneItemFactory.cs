﻿using CinemaDirector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A factory for creating all the game objects associated with a cutscene.
/// </summary>
public class CutsceneItemFactory
{
    private const string DIRECTOR_GROUP_NAME = "Director Group";
    private const string ACTOR_GROUP_NAME = "Actor Group";
    private const string MULTI_ACTOR_GROUP_NAME = "Multi Actor Group";
    private const string SHOT_NAME_DEFAULT = "Shot";
    private const string AUDIO_CLIP_NAME_DEFAULT = "Audio Clip";
    private const string CURVE_CLIP_NAME_DEFAULT = "Curve Clip";
    private const string SHOT_TRACK_LABEL = "Shot Track";
    private const string AUDIO_TRACK_LABEL = "Audio Track";
    private const string GLOBAL_TRACK_LABEL = "Global Track";
    private const string CURVE_TRACK_LABEL = "Curve Track";
    private const string EVENT_TRACK_LABEL = "Actor Track";

    private const float DEFAULT_SHOT_LENGTH = 5f;
    private const float DEFAULT_GLOBAL_ACTION_LENGTH = 5f;
    private const float DEFAULT_ACTOR_ACTION_LENGTH = 5f;
    private const float DEFAULT_CURVE_LENGTH = 5f;

    /// <summary>
    /// Create a Director group.
    /// </summary>
    public static DirectorGroup CreateDirectorGroup(Cutscene cutscene)
    {
        // Ensure there is only one director group. 
        if (cutscene.DirectorGroup != null)
        {
            return cutscene.DirectorGroup;
        }

        // Create the director group.
        GameObject directorGroupGO = new GameObject(DIRECTOR_GROUP_NAME, typeof(DirectorGroup));
        directorGroupGO.transform.parent = cutscene.transform;
        return directorGroupGO.GetComponent<DirectorGroup>();
    }

    /// <summary>
    /// Create a blank actor track group.
    /// </summary>
    /// <returns></returns>
    public static TrackGroup CreateActorTrackGroup(Cutscene cutscene)
    {
        return CreateActorTrackGroup(cutscene, null);
    }

    /// <summary>
    /// Create a track container for an actor in this cutscene.
    /// </summary>
    /// <param name="transform">The transform of the game object</param>
    /// <returns>the newly created container</returns>
    public static TrackGroup CreateActorTrackGroup(Cutscene cutscene, Transform transform)
    {
        string trackGroupName = ACTOR_GROUP_NAME;
        if (transform != null)
        {
            trackGroupName = string.Format("{0} Group", transform.name);
        }

        GameObject actorGroupGO = new GameObject(trackGroupName, typeof(ActorTrackGroup));
        actorGroupGO.transform.parent = cutscene.transform;

        ActorTrackGroup actorTrackGroup = actorGroupGO.GetComponent<ActorTrackGroup>();
        actorTrackGroup.Actor = transform;

        return actorTrackGroup;
    }

    /// <summary>
    /// Create a multi actor track group and attach it to the given cutscene
    /// </summary>
    /// <param name="cutscene">The cutscene</param>
    /// <returns></returns>
    public static MultiActorTrackGroup CreateMultiActorTrackGroup(Cutscene cutscene)
    {
        GameObject multiActorGroupGO = new GameObject(MULTI_ACTOR_GROUP_NAME, typeof(MultiActorTrackGroup));
        multiActorGroupGO.transform.parent = cutscene.transform;

        return multiActorGroupGO.GetComponent<MultiActorTrackGroup>();
    }

    public static CinemaShot CreateNewShot(ShotTrack shotTrack)
    {
        string name = DirectorHelper.getCutsceneItemName(shotTrack.gameObject, SHOT_NAME_DEFAULT, typeof(CinemaShot));
        GameObject shotGO = new GameObject(name);
        shotGO.transform.parent = shotTrack.transform;

        SortedDictionary<float, CinemaShot> sortedShots = new SortedDictionary<float, CinemaShot>();
        foreach (CinemaShot s in shotTrack.Shots)
        {
            sortedShots.Add(s.CutTime, s);
        }

        float latestTime = 0;
        float length = DEFAULT_SHOT_LENGTH;
        foreach (CinemaShot s in sortedShots.Values)
        {
            if (latestTime >= s.CutTime)
            {
                latestTime = Mathf.Max(latestTime, s.CutTime + s.Duration);
            }
            else
            {
                length = s.CutTime - latestTime;
                break;
            }
        }

        CinemaShot shot = shotGO.AddComponent<CinemaShot>();
        shot.CutTime = latestTime;
        shot.ShotLength = length;

        return shot;
    }

    public static CinemaGlobalAction CreateGlobalAction(GlobalItemTrack track, Type type, string name, float firetime)
    {
        GameObject item = new GameObject(name);
        CinemaGlobalAction action = item.AddComponent(type) as CinemaGlobalAction;

        SortedDictionary<float, CinemaGlobalAction> sortedActions = new SortedDictionary<float, CinemaGlobalAction>();
        foreach (CinemaGlobalAction a in track.Actions)
        {
            sortedActions.Add(a.Firetime, a);
        }

        float latestTime = firetime;
        float length = DEFAULT_GLOBAL_ACTION_LENGTH;
        foreach (CinemaGlobalAction a in sortedActions.Values)
        {
            if (latestTime >= a.Firetime)
            {
                latestTime = Mathf.Max(latestTime, a.Firetime + a.Duration);
            }
            else
            {
                length = a.Firetime - latestTime;
                break;
            }
        }

        action.Firetime = latestTime;
        action.Duration = length;
        item.transform.parent = track.transform;

        return action;
    }

    internal static CinemaActorAction CreateActorAction(ActorItemTrack track, Type type, string name, float firetime)
    {
        GameObject item = new GameObject(name);
        item.transform.parent = track.transform;
        CinemaActorAction action = item.AddComponent(type) as CinemaActorAction;

        SortedDictionary<float, CinemaActorAction> sortedActions = new SortedDictionary<float, CinemaActorAction>();
        foreach (CinemaActorAction a in track.ActorActions)
        {
            sortedActions.Add(a.Firetime, a);
        }

        float latestTime = 0;
        float length = DEFAULT_ACTOR_ACTION_LENGTH;
        foreach (CinemaActorAction a in sortedActions.Values)
        {
            if (latestTime >= a.Firetime)
            {
                latestTime = Mathf.Max(latestTime, a.Firetime + a.Duration);
            }
            else
            {
                length = a.Firetime - latestTime;
                break;
            }
        }

        action.Firetime = latestTime;
        action.Duration = length;

        return action;

    }

    internal static CinemaAudio CreateCinemaAudio(AudioTrack track, AudioClip clip, float firetime)
    {
        string name = DirectorHelper.getCutsceneItemName(track.gameObject, AUDIO_CLIP_NAME_DEFAULT, typeof(CinemaAudio));
        GameObject item = new GameObject(name);
        CinemaAudio cinemaAudio = item.AddComponent<CinemaAudio>();
        AudioSource source = item.AddComponent<AudioSource>();
        source.clip = clip;

        SortedDictionary<float, CinemaAudio> sortedClips = new SortedDictionary<float, CinemaAudio>();
        foreach (CinemaAudio a in track.AudioClips)
        {
            sortedClips.Add(a.Firetime, a);
        }
        
        float latestTime = firetime;
        
        float length = source.clip.length;
        foreach (CinemaAudio a in sortedClips.Values)
        {
            if (!(latestTime < a.Firetime && latestTime + length <= a.Firetime))
            {
                latestTime = a.Firetime + a.Duration;
            }
        }

        cinemaAudio.Firetime = latestTime;
        cinemaAudio.Duration = length;
        cinemaAudio.InTime = 0;
        cinemaAudio.OutTime = length;
        cinemaAudio.ItemLength = length;
        source.playOnAwake = false;
        item.transform.parent = track.transform;

        return cinemaAudio;
    }

    /// <summary>
    /// Add an empty shot track to a given director group
    /// </summary>
    /// <param name="directorGroup">The director group to add a shot track to</param>
    /// <returns>The new shot track</returns>
    internal static ShotTrack CreateShotTrack(DirectorGroup directorGroup)
    {
        GameObject shotTrackGO = new GameObject(SHOT_TRACK_LABEL, typeof(ShotTrack));
        shotTrackGO.transform.parent = directorGroup.transform;
        return shotTrackGO.GetComponent<ShotTrack>();
    }

    internal static AudioTrack CreateAudioTrack(DirectorGroup directorGroup)
    {
        string name = DirectorHelper.getCutsceneItemName(directorGroup.gameObject, AUDIO_TRACK_LABEL, typeof(AudioTrack));
        GameObject audioTrackGO = new GameObject(name, typeof(AudioTrack));

        audioTrackGO.transform.parent = directorGroup.transform;
        return audioTrackGO.GetComponent<AudioTrack>();
    }

    internal static GlobalItemTrack CreateGlobalItemTrack(DirectorGroup directorGroup)
    {
        string name = DirectorHelper.getCutsceneItemName(directorGroup.gameObject, GLOBAL_TRACK_LABEL, typeof(GlobalItemTrack));
        GameObject globalTrackGO = new GameObject(name, typeof(GlobalItemTrack));

        globalTrackGO.transform.parent = directorGroup.transform;
        return globalTrackGO.GetComponent<GlobalItemTrack>();
    }

    internal static ActorItemTrack CreateActorItemTrack(ActorTrackGroup trackGroup)
    {
        GameObject eventTrackGO = new GameObject(EVENT_TRACK_LABEL, typeof(ActorItemTrack));
        eventTrackGO.transform.parent = trackGroup.transform;
        return eventTrackGO.GetComponent<ActorItemTrack>();
    }

    internal static ActorItemTrack CreateActorItemTrack(MultiActorTrackGroup trackGroup)
    {
        GameObject eventTrackGO = new GameObject(EVENT_TRACK_LABEL, typeof(ActorItemTrack));
        eventTrackGO.transform.parent = trackGroup.transform;
        return eventTrackGO.GetComponent<ActorItemTrack>();
    }

    internal static CurveTrack CreateCurveTrack(ActorTrackGroup trackGroup)
    {
        GameObject curveTrackGO = new GameObject(CURVE_TRACK_LABEL, typeof(CurveTrack));
        curveTrackGO.transform.parent = trackGroup.transform;
        return curveTrackGO.GetComponent<CurveTrack>();
    }

    internal static MultiCurveTrack CreateMultiActorCurveTrack(MultiActorTrackGroup trackGroup)
    {
        GameObject curveTrackGO = new GameObject(CURVE_TRACK_LABEL, typeof(MultiCurveTrack));
        curveTrackGO.transform.parent = trackGroup.transform;
        return curveTrackGO.GetComponent<MultiCurveTrack>();
    }

    internal static CinemaGlobalEvent CreateGlobalEvent(GlobalItemTrack track, Type type, string name, float firetime)
    {
        GameObject item = new GameObject(name);
        item.transform.parent = track.transform;
        CinemaGlobalEvent globalEvent = item.AddComponent(type) as CinemaGlobalEvent;

        globalEvent.Firetime = firetime;
        return globalEvent;
    }

    internal static CinemaActorEvent CreateActorEvent(ActorItemTrack track, Type type, string name, float firetime)
    {
        GameObject item = new GameObject(name);
        item.transform.parent = track.transform;
        CinemaActorEvent actorEvent = item.AddComponent(type) as CinemaActorEvent;

        actorEvent.Firetime = firetime;

        return actorEvent;
    }

    internal static CinemaActorClipCurve CreateActorClipCurve(CurveTrack track)
    {
        string name = DirectorHelper.getCutsceneItemName(track.gameObject, CURVE_CLIP_NAME_DEFAULT, typeof(CinemaActorClipCurve));
        GameObject item = new GameObject(name);
        item.transform.parent = track.transform;
        CinemaActorClipCurve clip = item.AddComponent<CinemaActorClipCurve>();

        SortedDictionary<float, CinemaActorClipCurve> sortedItems = new SortedDictionary<float, CinemaActorClipCurve>();
        foreach (CinemaActorClipCurve c in track.TimelineItems)
        {
            sortedItems.Add(c.Firetime, c);
        }

        float latestTime = 0;
        float length = DEFAULT_CURVE_LENGTH;
        foreach (CinemaActorClipCurve c in sortedItems.Values)
        {
            if (latestTime >= c.Firetime)
            {
                latestTime = Mathf.Max(latestTime, c.Firetime + c.Duration);
            }
            else
            {
                length = c.Firetime - latestTime;
                break;
            }
        }

        clip.Firetime = latestTime;
        clip.Duration = length;

        return clip;
    }

    internal static CinemaMultiActorCurveClip CreateMultiActorClipCurve(MultiCurveTrack multiCurveTrack)
    {
        string name = DirectorHelper.getCutsceneItemName(multiCurveTrack.gameObject, CURVE_CLIP_NAME_DEFAULT, typeof(CinemaMultiActorCurveClip));
        GameObject item = new GameObject(name);
        item.transform.parent = multiCurveTrack.transform;
        CinemaMultiActorCurveClip clip = item.AddComponent<CinemaMultiActorCurveClip>();

        SortedDictionary<float, CinemaMultiActorCurveClip> sortedItems = new SortedDictionary<float, CinemaMultiActorCurveClip>();
        foreach (CinemaMultiActorCurveClip c in multiCurveTrack.TimelineItems)
        {
            sortedItems.Add(c.Firetime, c);
        }

        float latestTime = 0;
        float length = DEFAULT_CURVE_LENGTH;
        foreach (CinemaMultiActorCurveClip c in sortedItems.Values)
        {
            if (latestTime >= c.Firetime)
            {
                latestTime = Mathf.Max(latestTime, c.Firetime + c.Duration);
            }
            else
            {
                length = c.Firetime - latestTime;
                break;
            }
        }

        clip.Firetime = latestTime;
        clip.Duration = length;

        return clip;
    }
}
