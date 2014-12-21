using Assets.Scripts.Component.Actions;
using UnityEngine;

namespace Assets.Scripts.Entities.Combat
{
    public class PawnActions : MonoBehaviour
    {
        public ActionType m_AttackType;
        public ActionCloseAttack m_Attack;

        public IAction GetAttackAction()
        {
            return m_Attack;
        }

        public void Awake()
        {
            m_Attack.m_Pawn = gameObject;
        }
    }
}
