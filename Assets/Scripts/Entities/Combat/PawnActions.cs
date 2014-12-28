using Assets.Scripts.Component.Actions;
using Assets.Scripts.Entities.World;

using System.Collections.Generic;

using UnityEngine;
#pragma warning disable 649

namespace Assets.Scripts.Entities.Combat
{
    public class PawnActions : MonoBehaviour
    {
        //This list os only here to appear in the editor. The dictionary is used at runtime.
        [SerializeField]
        private List<ActionEntry> m_ActionEntries;

        private Dictionary<ActionId, IAction> m_ActionsMap;

        public IAction GetAttackAction()
        {
            return m_ActionsMap[ActionId.Attack];
        }

        public void Awake()
        {
            m_ActionsMap = new Dictionary<ActionId, IAction>();
            foreach (ActionEntry entry in m_ActionEntries)
            {
                IAction act = entry.GetAction();
                act.m_Pawn = gameObject;
                m_ActionsMap.Add(entry.m_Action, act);
            }
        }
    }
}
