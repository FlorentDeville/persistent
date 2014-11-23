using Persistent;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Trigger_PressButton))]
public class Trigger_PressButton_Editor : Editor 
{

    Trigger_PressButton CastedTarget
    {
        get { return (Trigger_PressButton)target; }
    }

    GameObject GameObject
    {
        get { return CastedTarget.gameObject; }
    }

    void OnSceneGUI()
    {
        Vector3 worldPosition = GameObject.transform.TransformPoint(CastedTarget.m_FeedbackPosition);
        Vector3 newWorldPosition = Handles.PositionHandle(worldPosition, Quaternion.identity);
        CastedTarget.m_FeedbackPosition = GameObject.transform.InverseTransformPoint(newWorldPosition);
        
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}
