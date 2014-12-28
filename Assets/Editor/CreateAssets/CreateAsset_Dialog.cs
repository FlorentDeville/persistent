using Assets.Editor.Utilities;
using System;
using UnityEditor;

namespace AssemblyCSharpEditor
{
	public class CreateAsset_Dialog
	{
		[MenuItem("Persistent/Create/Dialog")]
		public static void CreateAsset()
		{
			Core_Dialog newAsset = new Core_Dialog();
		    AssetDatabase.CreateAsset(newAsset, Helper.GetSelectedPathOrFallback() + "/Dialog.asset");
		    EditorUtility.FocusProjectWindow();
		    Selection.activeObject = newAsset;
		}
	}
}

