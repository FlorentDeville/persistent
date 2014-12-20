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

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
