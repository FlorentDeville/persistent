using Assets.Scripts.Assets;
using System;
using UnityEditor;

namespace AssemblyCSharpEditor
{
    public class CreateAsset_CharactersTable
    {
        [MenuItem("Persistent/Create/Characters Table")]
        public static void CreateAsset()
        {
            CharactersTable newAsset = new CharactersTable();
            AssetDatabase.CreateAsset(newAsset, Helper.GetSelectedPathOrFallback() + "/chartable.asset");
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newAsset;
        }
    }
}
