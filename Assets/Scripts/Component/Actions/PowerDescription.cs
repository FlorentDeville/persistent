using Assets.Scripts.Assets;
using Assets.Scripts.Entities.Combat;

using System;

using UnityEngine;

namespace Assets.Scripts.Component.Actions
{
    public enum PawnTag
    {
        Unknown,
        MP,
        HP,
        Priority,
        PriorityIncrease,
        None,

        Count
    }

    [Serializable]
    public class PowerDescription
    {
        public PawnTag m_DamageTarget;

        public float m_DamageFactor;

        public PawnTag m_CostTarget;

        public float m_Cost;

        public bool CanBeUsed(PawnStatistics _pawn)
        {
            switch(m_CostTarget)
            {
                case PawnTag.HP:
                    return _pawn.m_HP >= m_Cost;

                case PawnTag.MP:
                    return _pawn.m_MP >= m_Cost;

                case PawnTag.Priority:
                    return _pawn.m_Priority >= m_Cost;

                case PawnTag.PriorityIncrease:
                    return _pawn.m_PriorityIncrease >= m_Cost;

                case PawnTag.None:
                    return true;

                default:
                    Debug.LogError(string.Format("Unknown PawnTag {0}", m_CostTarget));
                    return false;
            }
        }

        public void ApplyCost(PawnStatistics _pawn)
        {
            switch(m_CostTarget)
            {
                case PawnTag.HP:
                    {
                        _pawn.m_HP -= m_Cost;
                        if (_pawn.m_HP < 0) _pawn.m_HP = 0;
                    }
                    break;

                case PawnTag.MP:
                    {
                        _pawn.m_MP -= m_Cost;
                        if (_pawn.m_MP < 0) _pawn.m_MP = 0;
                    }
                    break;

                case PawnTag.PriorityIncrease:
                    {
                        _pawn.m_PriorityIncrease -= m_Cost;
                        if (_pawn.m_PriorityIncrease < 0) _pawn.m_PriorityIncrease = 0;
                    }
                    break;

                case PawnTag.Priority:
                    {
                        _pawn.m_Priority -= m_Cost;
                        if (_pawn.m_Priority < 0) _pawn.m_Priority = 0;
                    }
                    break;

                default:
                    break;
            }
        }

        public void Resolve(PawnStatistics _src, PawnStatistics _target, ResolveResult _result)
        {
            switch(m_DamageTarget)
            {
                case PawnTag.HP:
                    ResolveHP(_src, _target, _result);
                    break;

                case PawnTag.PriorityIncrease:
                    ResolvePriorityIncrease(_src, _target, _result);
                    break;

                default:
                    Debug.LogError(string.Format("Can't resolve damage target {0}", m_DamageTarget));
                    break;
            }
        }

        private void ResolveHP(PawnStatistics _src, PawnStatistics _target, ResolveResult _result)
        {
            float weaponBonus = 0;
            if (_src.m_EquippedWeapon != null)
            {
                Weapon wpn = _src.m_EquippedWeapon;
                weaponBonus = wpn.m_Atk + UnityEngine.Random.Range(-wpn.m_AtkR, wpn.m_AtkR);
            }

            float dmg = (_src.m_Atk * m_DamageFactor) + weaponBonus - _target.m_Def;
            if (dmg < 0) dmg = 0;
            int realDamage = (int)dmg;

            _result.m_Damage = realDamage;

            _target.m_HP -= realDamage;

            if (_target.m_HP <= 0)
            {
                _target.m_HP = 0;
                PawnBehavior behavior = _target.GetComponent<PawnBehavior>();
                behavior.SetDeadState();
            }
        }

        private void ResolvePriorityIncrease(PawnStatistics _src, PawnStatistics _target, ResolveResult _result)
        {
            _target.m_PriorityIncrease = _target.m_PriorityIncrease * m_DamageFactor;
        }
    }
}
