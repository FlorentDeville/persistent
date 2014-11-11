using UnityEditor;
using UnityEngine;
using CinemaDirector;

[CutsceneTrackGroupAttribute(typeof(MultiActorTrackGroup))]
public class MultiActorTrackGroupControl : TrackGroupControl
{
    public override void Initialize()
    {
        base.Initialize();
        LabelPrefix = styles.MultiActorGroupIcon.normal.background;
    }

    protected override void addTrackContext()
    {
        GenericMenu createMenu = new GenericMenu();
        createMenu.AddItem(new GUIContent("Add Actor Track"), false, addActorTrack);
        createMenu.AddItem(new GUIContent("Add Curve Track"), false, addCurveTrack);
        createMenu.ShowAsContext();
    }

    private void addCurveTrack()
    {
        MultiActorTrackGroup matg = TrackGroup.Behaviour as MultiActorTrackGroup;
        Undo.RegisterCreatedObjectUndo(CutsceneItemFactory.CreateMultiActorCurveTrack(matg).gameObject, "Create Multi Actor Track");
    }

    private void addActorTrack()
    {
        MultiActorTrackGroup matg = TrackGroup.Behaviour as MultiActorTrackGroup;
        Undo.RegisterCreatedObjectUndo(CutsceneItemFactory.CreateActorItemTrack(matg).gameObject, "Create Actor Track");
    }
}

