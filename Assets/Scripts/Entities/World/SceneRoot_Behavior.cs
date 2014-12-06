using UnityEngine;
using System.Collections;

using Assets.Scripts.Manager;

public class SceneRoot_Behavior : MonoBehaviour 
{
    void Awake()
    {
        GameSceneManager.GetInstance().PushCurrentRoot();
    }
}
