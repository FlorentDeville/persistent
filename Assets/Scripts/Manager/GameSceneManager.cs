using Assets.Scripts.Manager.Parameter;

using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class GameSceneManager
    {
        private static GameSceneManager m_Instance = null;

        private Stack<GameObject> m_RootStack;

        private Stack<ISceneParameter> m_ParameterStack;

        private GameSceneManager()
        {
            m_RootStack = new Stack<GameObject>();
            m_ParameterStack = new Stack<ISceneParameter>();
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

        public void LoadCombatScene(string _sceneName, ISceneParameter _parameter)
        {
            m_RootStack.Peek().SetActive(false);
            Application.LoadLevelAdditive(_sceneName);

            m_ParameterStack.Push(_parameter);
        }

        public void PushCurrentRoot()
        {
            GameObject any = GameObject.FindObjectOfType<GameObject>();
            m_RootStack.Push(any.transform.root.gameObject);
        }

        public GameObject Pop(bool _destroy)
        {
            m_ParameterStack.Pop();

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

        public T GetParameter<T>()
            where T : ISceneParameter
        {
            return (T)m_ParameterStack.Peek();
        }

    }
}
