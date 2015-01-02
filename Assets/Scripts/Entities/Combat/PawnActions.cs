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
        //[SerializeField]
        //private List<ActionEntry> m_ActionEntries;

        //private Dictionary<ActionId, IAction> m_ActionsMap;

        //public IAction GetAttackAction()
        //{
        //    return m_ActionsMap[ActionId.Attack];
        //}

        public ActionRunner m_DefaultAttack;
        public ActionRunner m_DefaultMagic;
        public ActionRunner m_DefaultItem;

        [SerializeField]
        private List<NewMagicActionOverrideEntry> m_OverrideMagic;
        private Dictionary<MagicId, ActionRunner> m_OverrideMagicMap;

        public void Awake()
        {
            //m_ActionsMap = new Dictionary<ActionId, IAction>();
            //foreach (ActionEntry entry in m_ActionEntries)
            //{
            //    IAction act = entry.GetAction();
            //    act.m_Pawn = gameObject;
            //    m_ActionsMap.Add(entry.m_Action, act);
            //}

            m_DefaultAttack.m_Pawn = gameObject;
            m_DefaultMagic.m_Pawn = gameObject;
            m_DefaultItem.m_Pawn = gameObject;

            m_OverrideMagicMap = new Dictionary<MagicId, ActionRunner>();
            foreach(NewMagicActionOverrideEntry entry in m_OverrideMagic)
            {
                ActionRunner action = entry.m_Action;
                action.m_Pawn = gameObject;
                m_OverrideMagicMap.Add(entry.m_Magic, action);
            }
        }

        //public IAction GetAction(ActionId _id)
        //{
        //    return m_ActionsMap[_id];
        //}

        public ActionRunner GetAction(MagicId _id)
        {
            if (!m_OverrideMagicMap.ContainsKey(_id))
                return m_DefaultMagic;

            return m_OverrideMagicMap[_id];
        }
    }
}
