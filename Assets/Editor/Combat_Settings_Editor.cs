using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Combat_Settings))]
public class Combat_Settings_Editor : Editor
{
    private Combat_Settings CastedTarget
    {
        get { return (Combat_Settings)target; }
    }

    private GameObject Owner
    {
        get { return CastedTarget.gameObject; }
    }

    void OnSceneGUI()
    {
        if (CastedTarget.m_PawnsEnemyPosition != null)
        {
            for (int i = 0; i < CastedTarget.m_PawnsEnemyPosition.Length; ++i)
            {
                Combat_Settings.InnerTransform transform = CastedTarget.m_PawnsEnemyPosition[i];
                Vector3 worldPosition = Owner.transform.TransformPoint(transform.m_Position);
                Quaternion localRotation = Quaternion.Euler(transform.m_Rotation);
                Quaternion worldRotation = Owner.transform.localRotation * localRotation;

                switch(Tools.current)
                {
                    case Tool.Move:
                        Vector3 newWorldPosition = Handles.PositionHandle(worldPosition, worldRotation);
                        transform.m_Position = Owner.transform.InverseTransformPoint(newWorldPosition);
                        break;

                    case Tool.Rotate:
                        Quaternion newWorldRotation = Handles.RotationHandle(worldRotation, worldPosition);
                        Quaternion newLocalRotation = newWorldRotation * Quaternion.Inverse(Owner.transform.localRotation);
                        transform.m_Rotation = newLocalRotation.eulerAngles;
                        break;
                }
                
            }
        }

        if(CastedTarget.m_PawnsPlayerPosition != null)
        {
            for (int i = 0; i < CastedTarget.m_PawnsPlayerPosition.Length; ++i)
            {
                Combat_Settings.InnerTransform transform = CastedTarget.m_PawnsPlayerPosition[i];
                Vector3 worldPosition = Owner.transform.TransformPoint(transform.m_Position);
                Quaternion localRotation = Quaternion.Euler(transform.m_Rotation);
                Quaternion worldRotation = Owner.transform.localRotation * localRotation;

                switch (Tools.current)
                {
                    case Tool.Move:
                        Vector3 newWorldPosition = Handles.PositionHandle(worldPosition, worldRotation);
                        transform.m_Position = Owner.transform.InverseTransformPoint(newWorldPosition);
                        break;

                    case Tool.Rotate:
                        Quaternion newWorldRotation = Handles.RotationHandle(worldRotation, worldPosition);
                        Quaternion newLocalRotation = newWorldRotation * Quaternion.Inverse(Owner.transform.localRotation);
                        transform.m_Rotation = newLocalRotation.eulerAngles;
                        break;
                }

            }
        }

        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}

