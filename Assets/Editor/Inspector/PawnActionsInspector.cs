using Assets.Scripts.Component.Actions;
using Assets.Scripts.Entities.Combat;
using UnityEditor;

[CustomEditor(typeof(PawnActions))]
public class PawnActionsInspector : Editor
{
    public override void OnInspectorGUI()
    {
        PawnActions script = (PawnActions)target;
        script.m_AttackType = (ActionType)EditorGUILayout.EnumPopup("Attack Type", script.m_AttackType);
        serializedObject.Update();

        ++EditorGUI.indentLevel;
        switch(script.m_AttackType)
        {
            case ActionType.CloseAction:
                SerializedProperty prop = serializedObject.FindProperty("m_Attack");
                if (prop != null)
                    EditorGUILayout.PropertyField(prop, true);

                script.m_Attack.m_Pawn = script.gameObject;
                break;

            default:
                break;
        }
        serializedObject.ApplyModifiedProperties();
        --EditorGUI.indentLevel;
    }
}

