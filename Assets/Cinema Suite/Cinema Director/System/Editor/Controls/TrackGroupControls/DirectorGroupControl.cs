using UnityEditor;
using UnityEngine;
using CinemaDirector;

[CutsceneTrackGroupAttribute(typeof(DirectorGroup))]
public class DirectorGroupControl : TrackGroupControl
{
    protected override void addTrackContext()
    {
        GenericMenu createMenu = new GenericMenu();
        createMenu.AddItem(new GUIContent("Add Shot Track"), false, addShotTrack);
        createMenu.AddItem(new GUIContent("Add Audio Track"), false, addAudioTrack);
        createMenu.AddItem(new GUIContent("Add Global Track"), false, addGlobalTrack);

        createMenu.ShowAsContext();
    }

    private void addGlobalTrack()
    {
        DirectorGroup dg = TrackGroup.Behaviour as DirectorGroup;
        Undo.RegisterCreatedObjectUndo(CutsceneItemFactory.CreateGlobalItemTrack(dg), "Create Global Track");
    }

    private void addAudioTrack()
    {
        DirectorGroup dg = TrackGroup.Behaviour as DirectorGroup;
        Undo.RegisterCreatedObjectUndo(CutsceneItemFactory.CreateAudioTrack(dg), "Create Audio Track");
    }

    private void addShotTrack()
    {
        DirectorGroup dg = TrackGroup.Behaviour as DirectorGroup;
        Undo.RegisterCreatedObjectUndo(CutsceneItemFactory.CreateShotTrack(dg), "Create Shot Track");
    }
}
