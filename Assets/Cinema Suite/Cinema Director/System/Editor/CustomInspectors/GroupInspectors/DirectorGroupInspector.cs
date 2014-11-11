using UnityEditor;
using UnityEngine;
using CinemaDirector;

/// <summary>
/// A custom inspector for a director group.
/// </summary>
[CustomEditor(typeof(DirectorGroup), true)]
public class DirectorGroupInspector : Editor
{
    private bool containerFoldout = true;

    #region Language
    GUIContent addTrackContent = new GUIContent("Add New Track", "Add a new track to this actor track group.");
    //GUIContent ordinalContent = new GUIContent("Ordinal", "The ordinal value of this container, for sorting containers in the timeline.");

    GUIContent addShotTrackContent = new GUIContent("Add Shot Track");
    GUIContent addAudioTrackContent = new GUIContent("Add Audio Track");
    GUIContent addActionTrackContent = new GUIContent("Add Action Track");

    GUIContent tracksContent = new GUIContent("Global Tracks", "The tracks associated with this Director Group.");
    #endregion

    /// <summary>
    /// On inspector enable, load the serialized properties
    /// </summary>
    private void OnEnable()
    {
    }

    /// <summary>
    /// Draw the inspector
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.serializedObject.Update();

        DirectorGroup directorGroup = base.serializedObject.targetObject as DirectorGroup;
        TimelineTrack[] tracks = directorGroup.GetTracks();

        if (tracks.Length > 0)
        {
            containerFoldout = EditorGUILayout.Foldout(containerFoldout, tracksContent);
            if (containerFoldout)
            {
                EditorGUI.indentLevel++;

                foreach (TimelineTrack track in tracks)
                {
                    EditorGUILayout.ObjectField(track.name, track, typeof(GlobalTrack), true);
                }
                EditorGUI.indentLevel--;
            }
        }

        if (GUILayout.Button(addTrackContent))
        {
            GenericMenu createMenu = new GenericMenu();
            createMenu.AddItem(addShotTrackContent, false, addShotTrack);
            createMenu.AddItem(addAudioTrackContent, false, addAudioTrack);
            createMenu.AddItem(addActionTrackContent, false, addGlobalItemTrack);

            createMenu.ShowAsContext();
        }

        base.serializedObject.ApplyModifiedProperties();
    }

    internal void addShotTrack()
    {
        DirectorGroup directorGroup = base.serializedObject.targetObject as DirectorGroup;
        Undo.RegisterCreatedObjectUndo(CutsceneItemFactory.CreateShotTrack(directorGroup), "Create Shot Track");
    }

    internal void addAudioTrack()
    {
        DirectorGroup directorGroup = base.serializedObject.targetObject as DirectorGroup;
        Undo.RegisterCreatedObjectUndo(CutsceneItemFactory.CreateAudioTrack(directorGroup), "Create Audio Track");
    }

    internal void addGlobalItemTrack()
    {
        DirectorGroup directorGroup = base.serializedObject.targetObject as DirectorGroup;
        Undo.RegisterCreatedObjectUndo(CutsceneItemFactory.CreateGlobalItemTrack(directorGroup), "Create Global Track");
    }
}
