using Persistent.WorldEntity;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spawner_Behavior))]
public class Spawner_Behavior_Editor : Editor
{
    private Spawner_Behavior CastedTarget
    {
        get { return (Spawner_Behavior)target; }
    }

    private GameObject Owner
    {
        get { return CastedTarget.gameObject; }
    }

    void OnSceneGUI()
    {
        if(CastedTarget.m_portals != null)
        {
            for (int i = 0; i < CastedTarget.m_portals.Length; ++i)
            {
                Vector3 worldPosition = Owner.transform.TransformPoint(CastedTarget.m_portals[i]);
                Vector3 newWorldPosition = Handles.PositionHandle(worldPosition, Quaternion.identity);
                CastedTarget.m_portals[i] = Owner.transform.InverseTransformPoint(newWorldPosition);
            }
        }

       if(CastedTarget.m_waypoints != null)
       {
           for(int i = 0; i < CastedTarget.m_waypoints.Length; ++i)
           {
               Vector3 worldPosition = Owner.transform.TransformPoint(CastedTarget.m_waypoints[i]);
               Vector3 newWorldPosition = Handles.PositionHandle(worldPosition, Quaternion.identity);
               CastedTarget.m_waypoints[i] = Owner.transform.InverseTransformPoint(newWorldPosition);
           }
       }

        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}
