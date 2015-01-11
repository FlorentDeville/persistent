using Assets.Scripts.Assets;
using UnityEngine;
using System.Collections;

public class PawnStatistics : MonoBehaviour 
{
    public string m_PawnName;

    public float m_Atk;

    public float m_Def;

    public float m_MaxHP;

    public float m_HP;

    public float m_MaxMP;

    public float m_MP;

    public float m_MGAtk;

    public float m_MGDef;

    public float m_WepAtk { get; set; }

    public float m_WepAtkCritic { get; set; }

    public float m_WepDef { get; set; }

    public float m_WepDefCritic { get; set; }

    public float m_WepMGAtk { get; set; }

    public float m_WepMGAtkCritic { get; set; }

    public float m_WepMGDef { get; set; }

    public float m_WepMGDefCritic { get; set; }

    public float m_Priority;

    public float m_PriorityIncrease;

    public bool m_IsControlledByPlayer;

    public Weapon m_EquippedWeapon;

    public PawnStatistics(PawnStatistics _original)
    {
        m_PawnName = _original.m_PawnName;
        m_Atk = _original.m_Atk;
        m_Def = _original.m_Def;
        m_MaxHP = _original.m_MaxHP;
        m_HP = _original.m_HP;
        m_MaxMP = _original.m_MaxMP;
        m_MP = _original.m_MP;
        m_MGAtk = _original.m_MGAtk;
        m_MGDef = _original.m_MGDef;
        m_WepAtk = _original.m_WepAtk;
        m_WepAtkCritic = _original.m_WepAtkCritic;
        m_WepDef = _original.m_WepDef;
        m_WepDefCritic = _original.m_WepDefCritic;
        m_WepMGAtk = _original.m_WepMGAtk;
        m_WepMGAtkCritic = _original.m_WepMGAtkCritic;
        m_WepMGDef = _original.m_WepMGDef;
        m_WepMGDefCritic = _original.m_WepMGDefCritic;
        m_Priority = _original.m_Priority;
        m_PriorityIncrease = _original.m_PriorityIncrease;
        m_IsControlledByPlayer = _original.m_IsControlledByPlayer;
        m_EquippedWeapon = _original.m_EquippedWeapon;
    }
}
