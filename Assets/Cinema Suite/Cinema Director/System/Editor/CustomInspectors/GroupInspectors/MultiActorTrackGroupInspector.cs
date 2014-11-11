using UnityEditor;
using UnityEngine;
using CinemaDirector;

/// <summary>
/// A custom inspector for a cutscene.
/// </summary>
[CustomEditor(typeof(MultiActorTrackGroup), true)]
public class MultiActorTrackGroupInspector : Editor
{
    //private SerializedProperty actors;

    #region Language;

    GUIContent addTrackContent = new GUIContent("Add New Track", "Add a new track to this multi-actor track group.");
    GUIContent addEventTrackContent = new GUIContent("Add Event Track");
    GUIContent addCurveTrackContent = new GUIContent("Add Curve Track");

    #endregion

    /// <summary>
    /// On inspector enable, load the serialized properties
    /// </summary>
    private void OnEnable()
    {
        //this.actors = base.serializedObject.FindProperty("actors");
    }

    /// <summary>
    /// Draw the inspector
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        //base.serializedObject.Update();

        //EditorGUILayout.PropertyField(actors, actorsContent);

        if (GUILayout.Button(addTrackContent))
        {
            GenericMenu createMenu = new GenericMenu();
            createMenu.AddItem(addEventTrackContent, false, addEventTrack);
            createMenu.AddItem(addCurveTrackContent, false, addCurveTrack);
            createMenu.ShowAsContext();
        }

        //base.serializedObject.ApplyModifiedProperties();
    }

    private void addCurveTrack()
    {
        MultiActorTrackGroup actorTrackGroup = base.serializedObject.targetObject as MultiActorTrackGroup;
        Undo.RegisterCreatedObjectUndo(CutsceneItemFactory.CreateMultiActorCurveTrack(actorTrackGroup).gameObject, "Create Multi Actor Track");
    }

    private void addEventTrack()
    {
        MultiActorTrackGroup actorTrackGroup = base.serializedObject.targetObject as MultiActorTrackGroup;
        Undo.RegisterCreatedObjectUndo(CutsceneItemFactory.CreateActorItemTrack(actorTrackGroup).gameObject, "Create Actor Track");
    }
}
