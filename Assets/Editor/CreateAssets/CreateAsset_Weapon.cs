using Assets.Scripts.Assets;
using System;
using UnityEditor;

namespace AssemblyCSharpEditor
{
    public class CreateAsset_Weapon
    {
        [MenuItem("Persistent/Create/Weapon")]
        public static void CreateAsset()
        {
            Weapon newAsset = new Weapon();
            AssetDatabase.CreateAsset(newAsset, Helper.GetSelectedPathOrFallback() + "/Weapon.asset");
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newAsset;
        }
    }
}
