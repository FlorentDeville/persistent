using AssemblyCSharp;
using UnityEngine;

namespace Assets.Scripts.Interactions.Trigger
{
    public class Trigger_EnterCollider : MonoBehaviour
    {
        public Collider m_collider;

        public Interaction[] m_interactions;

        public bool m_AutoReset;

        void Awake()
        {
            foreach (Interaction inter in m_interactions)
                inter.enabled = false;
        }

        void OnTriggerEnter(Collider _col)
        {
            if (_col.gameObject.name != GameObjectHelper.getPlayerName())
                return;

            foreach (Interaction inter in m_interactions)
                inter.enabled = true;

            if (!m_AutoReset)
                enabled = false;
        }
    }
}
