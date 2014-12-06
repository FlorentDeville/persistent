using UnityEngine;
using System.Collections;

public class StartScreen_GUI_Behavior : MonoBehaviour 
{

	void OnGUI()
    {
        float top = 30;
        if (GUI.Button(new Rect(30, top, 150, 30), "Start Game"))
        {
            Application.LoadLevel("Sequence_01");
        }

        string path = System.IO.Path.Combine(Application.dataPath, "Data");
        path = System.IO.Path.Combine(path, "Scene");

        string[] files = System.IO.Directory.GetFiles(path, "*.unity");

        const float VerticalOffset = 32f;
        top += VerticalOffset;
        foreach(string scene in files)
        {
            string filename = System.IO.Path.GetFileNameWithoutExtension(scene);
            top += VerticalOffset;
            if (GUI.Button(new Rect(30, top, 150, 30), filename))
            {
                Application.LoadLevel(filename);
            }
        }
    }
}
