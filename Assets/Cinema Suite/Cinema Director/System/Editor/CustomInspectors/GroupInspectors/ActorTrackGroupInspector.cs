using UnityEditor;
using UnityEngine;
using CinemaDirector;

/// <summary>
/// A custom inspector for a cutscene.
/// </summary>
[CustomEditor(typeof(ActorTrackGroup), true)]
public class ActorTrackGroupInspector : Editor
{
    private SerializedProperty actor;
    private bool containerFoldout = true;

    #region Language
    //GUIContent ordinalContent = new GUIContent("Ordinal", "The ordinal value of this container, for sorting containers in the timeline.");
    GUIContent addTrackContent = new GUIContent("Add New Track", "Add a new track to this actor track group.");

    GUIContent addCurveTrackContent = new GUIContent("Add Curve Track");
    GUIContent addEventTrackContent = new GUIContent("Add Event Track");

    GUIContent tracksContent = new GUIContent("Actor Tracks", "The tracks associated with this Actor Group.");
    #endregion

    /// <summary>
    /// On inspector enable, load the serialized properties
    /// </summary>
    private void OnEnable()
    {
        this.actor = base.serializedObject.FindProperty("actor");
    }

    /// <summary>
    /// Draw the inspector
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.serializedObject.Update();

        ActorTrackGroup actorGroup = base.serializedObject.targetObject as ActorTrackGroup;
        TimelineTrack[] tracks = actorGroup.GetTracks();
        EditorGUILayout.PropertyField(actor);

        if (tracks.Length > 0)
        {
            containerFoldout = EditorGUILayout.Foldout(containerFoldout, tracksContent);
            if (containerFoldout)
            {
                EditorGUI.indentLevel++;

                foreach (TimelineTrack track in tracks)
                {
                    EditorGUILayout.ObjectField(track.name, track, typeof(ActorTrack), true);
                }
                EditorGUI.indentLevel--;
            }
        }
        if (GUILayout.Button(addTrackContent))
        {
            GenericMenu createMenu = new GenericMenu();
            createMenu.AddItem(addCurveTrackContent, false, addCurveTrack);
            createMenu.AddItem(addEventTrackContent, false, addEventTrack);

            createMenu.ShowAsContext();
        }

        base.serializedObject.ApplyModifiedProperties();
    }

    internal void addCurveTrack()
    {
        ActorTrackGroup actorTrackGroup = base.serializedObject.targetObject as ActorTrackGroup;
        Undo.RegisterCreatedObjectUndo(CutsceneItemFactory.CreateCurveTrack(actorTrackGroup).gameObject, "Create Curve Track");
    }

    internal void addEventTrack()
    {
        ActorTrackGroup actorTrackGroup = base.serializedObject.targetObject as ActorTrackGroup;
        Undo.RegisterCreatedObjectUndo(CutsceneItemFactory.CreateActorItemTrack(actorTrackGroup).gameObject, "Create Actor Track");
    }
}
