using UnityEngine;
using System.Collections;

using Assets.Scripts.Manager;

public class Level_01_Combat_01_GUI_Behavior : MonoBehaviour 
{
    void OnGUI()
    {
        float top = 30;
        if (GUI.Button(new Rect(30, top, 150, 30), "Go Back to level"))
        {
            GameSceneManager.GetInstance().Pop(true);
        }
    }
}
