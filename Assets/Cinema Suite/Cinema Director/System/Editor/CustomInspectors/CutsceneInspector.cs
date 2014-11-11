using UnityEditor;
using UnityEngine;
using CinemaDirector;

/// <summary>
/// A custom inspector for a cutscene.
/// </summary>
[CustomEditor(typeof(Cutscene))]
public class CutsceneInspector : Editor
{
    private SerializedProperty duration;
    private SerializedProperty runningTime;
    private SerializedProperty playbackSpeed;
    private bool containerFoldout = true;

    #region Language
        GUIContent durationContent = new GUIContent("Duration", "The duration of the cutscene.");
        GUIContent runningTimeContent = new GUIContent("Running Time", "The current running time of the cutscene.");
        GUIContent groupsContent = new GUIContent("Track Groups", "Organizational units of a cutscene.");
        GUIContent addGroupContent = new GUIContent("Add Group", "Add a new container to the cutscene.");

        GUIContent addDirectorGroupContent = new GUIContent("Add Director Group");
        GUIContent addActorGroupContent = new GUIContent("Add Actor Group");
        GUIContent addMultiActorGroupContent = new GUIContent("Add Multi Actor Group");
    #endregion

    /// <summary>
    /// On inspector enable, load the serialized properties
    /// </summary>
    private void OnEnable()
    {
        this.duration = base.serializedObject.FindProperty("duration");
        this.runningTime = base.serializedObject.FindProperty("runningTime");
        this.playbackSpeed = base.serializedObject.FindProperty("playbackSpeed");
    }

    /// <summary>
    /// Draw the inspector
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.serializedObject.Update();

        EditorGUILayout.PropertyField(this.duration, durationContent);
        EditorGUILayout.PropertyField(this.runningTime, runningTimeContent);
        EditorGUILayout.PropertyField(this.playbackSpeed, new GUIContent("Playback Speed"));

        containerFoldout = EditorGUILayout.Foldout(containerFoldout, groupsContent);

        if (containerFoldout)
        {
            EditorGUI.indentLevel++;
            Cutscene c = base.serializedObject.targetObject as Cutscene;
            foreach (TrackGroup container in c.TrackGroups)
            {
                EditorGUILayout.ObjectField(container.name, container, typeof(TrackGroup), true);
            }
            EditorGUI.indentLevel--;
            if (GUILayout.Button(addGroupContent))
            {
                GenericMenu createMenu = new GenericMenu();
                createMenu.AddItem(addDirectorGroupContent, false, addDirectorGroup);
                createMenu.AddItem(addActorGroupContent, false, addActorGroup);
                createMenu.AddItem(addMultiActorGroupContent, false, addMultiActorGroup);

                createMenu.ShowAsContext();
            }
        }
       
        base.serializedObject.ApplyModifiedProperties();
    }

    internal void addDirectorGroup()
    {
        Cutscene cutscene = base.serializedObject.targetObject as Cutscene;
        CutsceneItemFactory.CreateDirectorGroup(cutscene);
    }

    internal void addActorGroup()
    {
        Cutscene c = base.serializedObject.targetObject as Cutscene;
        CutsceneItemFactory.CreateActorTrackGroup(c);
    }

    internal void addMultiActorGroup()
    {
        Cutscene c = base.serializedObject.targetObject as Cutscene;
        CutsceneItemFactory.CreateMultiActorTrackGroup(c);
    }
}
