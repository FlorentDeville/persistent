using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.Component.MarkerEvent
{
    public class AnimationEventLibrary : MonoBehaviour
    {
        private static Dictionary<string, IAnimationEvent> m_Markers = new Dictionary<string, IAnimationEvent>();

        public static void InitEventLibrary()
        {
            m_Markers.Clear();
        }

        public void StartMarker(string _prefabName)
        {
            if(!m_Markers.ContainsKey(_prefabName))
            {
                if (!LoadMarker(_prefabName))
                    return;
            }

            IAnimationEvent marker = m_Markers[_prefabName];
            marker.StartEvent();
        }

        public void StopMarker(string _prefabMarker)
        {
            if (!m_Markers.ContainsKey(_prefabMarker))
                return;

            m_Markers[_prefabMarker].StopEvent();
        }

        private bool LoadMarker(string _prefabName)
        {
            GameObject prefab = Resources.Load<GameObject>(_prefabName);
            if(prefab == null)
            {
                Debug.LogError(string.Format("Can't load marker name {0}", _prefabName));
                return false;
            }

            GameObject obj = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);

            IAnimationEvent marker = obj.GetComponent<IAnimationEvent>();
            if(marker == null)
            {
                Debug.LogError(string.Format("The prefab {0} has no IMarker component", _prefabName));
                return false;
            }

            m_Markers.Add(_prefabName, marker); //NOT MULTITHREADING SAFE!!!!!
            return true;
        }
    }
}
