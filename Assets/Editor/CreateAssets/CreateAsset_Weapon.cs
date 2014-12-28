using Assets.Editor.Utilities;
using Assets.Scripts.Assets;

using UnityEditor;

namespace AssemblyCSharpEditor
{
    public class CreateAsset_Weapon
    {
        [MenuItem("Persistent/Create/Weapon")]
        public static void CreateAsset()
        {
            Weapon newAsset = Weapon.CreateInstance<Weapon>();
            AssetDatabase.CreateAsset(newAsset, Helper.GetSelectedPathOrFallback() + "/Weapon.asset");
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newAsset;
        }
    }

    public class CreateAsset_Item
    {
        [MenuItem("Persistent/Create/Item")]
        public static void CreateAsset()
        {
            Item newAsset = Item.CreateInstance<Item>();
            AssetDatabase.CreateAsset(newAsset, Helper.GetSelectedPathOrFallback() + "/Item.asset");
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newAsset;
        }
    }
}
