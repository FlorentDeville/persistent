using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(CancelButton))]
public class CancelButtonInspector : UnityEditor.UI.ButtonEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        SerializedProperty prop = serializedObject.FindProperty("onCancel");
        if(prop != null)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onCancel"), true);

        prop = serializedObject.FindProperty("onSelect");
        if(prop != null)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onSelect"), true);
        serializedObject.ApplyModifiedProperties();
    }
}

