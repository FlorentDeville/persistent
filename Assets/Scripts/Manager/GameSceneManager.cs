using Assets.Scripts.Manager.Parameter;

using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class GameSceneManager
    {
        private static GameSceneManager m_Instance = null;

        private Stack<GameObject> m_RootStack;

        private ISceneParameter m_SceneParameter;

        private GameSceneManager()
        {
            m_RootStack = new Stack<GameObject>(); 
        }
        
        public static GameSceneManager GetInstance()
        {
            if(m_Instance == null)
            {
                m_Instance = new GameSceneManager();
            }

            return m_Instance;
        }

        public void Close()
        {
            m_Instance = null;
        }

        public void LoadCombatScene(string _sceneName)
        {
            m_RootStack.Peek().SetActive(false);
            Application.LoadLevelAdditive(_sceneName);
        }

        public void PushCurrentRoot()
        {
            GameObject any = GameObject.FindObjectOfType<GameObject>();
            m_RootStack.Push(any.transform.root.gameObject);
        }

        public GameObject Pop(bool _destroy)
        {
            GameObject top = m_RootStack.Pop();
            top.SetActive(false);
            m_RootStack.Peek().SetActive(true);

            if(_destroy)
            {
                GameObject.Destroy(top);
                return null;
            }

            return top;
            
        }

    }
}
