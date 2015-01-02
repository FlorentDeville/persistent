using Assets.Scripts.Assets;
using Assets.Scripts.Assets.SpecificAction;
using Assets.Editor.Utilities;

using UnityEditor;
using UnityEngine;

namespace Assets.Editor.CreateAssets
{
    public class CreateAsset_MagicDescription
    {
        [MenuItem("Persistent/Create/Magic Description")]
        public static void CreateAsset()
        {
            MagicDescription newAsset = ScriptableObject.CreateInstance<MagicDescription>();
            AssetDatabase.CreateAsset(newAsset, Helper.GetSelectedPathOrFallback() + "/magicdesc.asset");
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newAsset;
        }
    }

    public class CreateAsset_AttackDescription
    {
        [MenuItem("Persistent/Create/Attack Description")]
        public static void CreateAsset()
        {
            AttackDescription newAsset = ScriptableObject.CreateInstance<AttackDescription>();
            AssetDatabase.CreateAsset(newAsset, Helper.GetSelectedPathOrFallback() + "/attackdesc.asset");
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newAsset;
        }
    }
}
