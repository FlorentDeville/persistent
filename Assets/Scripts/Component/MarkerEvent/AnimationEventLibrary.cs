using Assets.Scripts.Assets.SpecificAction;
using Assets.Scripts.Component.Actions;

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

        public void StartMarkerFromActionDescription()
        {
            ActionRunner runner = GameMaster.GetInstance().GetSelectedAction();

            Transform prefab = runner.ActionDescription.m_EventMarker;
            if (prefab == null)
                return;

            if(!m_Markers.ContainsKey(prefab.name))
            {
                if (!LoadMarker(prefab.name))
                    return;
            }

            IAnimationEvent marker = m_Markers[prefab.name];
            marker.StartEvent();
        }

        public void StopMarkerFromActionDescription()
        {
            ActionRunner runner = GameMaster.GetInstance().GetSelectedAction();

            Transform prefab = runner.ActionDescription.m_EventMarker;
            if (prefab == null)
                return;

            m_Markers[prefab.name].StopEvent();
        }

        private bool LoadMarker(string _prefabName)
        {
            string resourcePath = string.Format("Prefabs/Marker/{0}", _prefabName);
            GameObject prefab = Resources.Load<GameObject>(resourcePath);
            if (prefab == null)
            {
                Debug.LogError(string.Format("Can't load marker name {0}", _prefabName));
                return false;
            }

            GameObject obj = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);

            IAnimationEvent marker = obj.GetComponent<IAnimationEvent>();
            if (marker == null)
            {
                Debug.LogError(string.Format("The prefab {0} has no IMarker component", _prefabName));
                return false;
            }

            m_Markers.Add(_prefabName, marker); //NOT MULTITHREADING SAFE!!!!!
            return true;
        }
    }
}
