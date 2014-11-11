using System;
using System.IO;
using UnityEditor;

namespace AssemblyCSharpEditor
{
	public class Helper
	{
		public static string GetSelectedPathOrFallback()
		{
			string path = "Assets";
			
			foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
			{
				path = AssetDatabase.GetAssetPath(obj);
				if ( !string.IsNullOrEmpty(path) && File.Exists(path) ) 
				{
					path = Path.GetDirectoryName(path);
					break;
				}
			}
			return path;
		}
	}
}

