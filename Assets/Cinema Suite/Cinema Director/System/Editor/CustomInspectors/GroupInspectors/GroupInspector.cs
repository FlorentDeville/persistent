using UnityEditor;
using UnityEngine;
using CinemaDirector;

/// <summary>
/// A custom inspector for a cutscene.
/// </summary>
[CustomEditor(typeof(TrackGroup), true)]
public class GroupInspector : Editor
{
    private SerializedProperty ordinal;

    #region Language
    GUIContent ordinalContent = new GUIContent("Ordinal", "The ordinal value of this container, for sorting containers in the timeline.");
    #endregion

    /// <summary>
    /// On inspector enable, load the serialized properties
    /// </summary>
    private void OnEnable()
    {
        this.ordinal = base.serializedObject.FindProperty("ordinal");
    }

    /// <summary>
    /// Draw the inspector
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.serializedObject.Update();

        EditorGUILayout.PropertyField(this.ordinal, ordinalContent);

        base.serializedObject.ApplyModifiedProperties();
    }
}
