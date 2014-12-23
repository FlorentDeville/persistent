using AssemblyCSharpEditor;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class CreateAsset_DialogActor
{
	[MenuItem("Persistent/Create/Dialog Actor")]
	public static void CreateAsset()
	{
		Core_DialogActor newAsset = new Core_DialogActor();
	    AssetDatabase.CreateAsset(newAsset, Helper.GetSelectedPathOrFallback() + "/DialogActor.asset");
	    EditorUtility.FocusProjectWindow();
	    Selection.activeObject = newAsset;
	}
}
