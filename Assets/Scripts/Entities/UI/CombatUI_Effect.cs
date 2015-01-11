﻿using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 649

namespace Assets.Scripts.Entities.UI
{
    public class CombatUI_Effect : MonoBehaviour
    {
        [SerializeField]
        [Header("Display Attack Name")]
        private GameObject m_ShowAttackControl;

        [SerializeField]
        private string m_TriggerShowAttack;

        [SerializeField]
        [Header("Victory Effect")]
        private GameObject m_Victory;

        void Awake()
        {
            m_ShowAttackControl.SetActive(false);
            HideVictoryEffect();
        }

        public void StartAnimationShowAttackName(string _attackName)
        {
            m_ShowAttackControl.SetActive(true);
            m_ShowAttackControl.GetComponentInChildren<Text>().text = _attackName;

            Animator anim = m_ShowAttackControl.GetComponent<Animator>();
            anim.SetTrigger(m_TriggerShowAttack);

        }

        public void ShowVictoryEffect()
        {
            m_Victory.SetActive(true);
        }

        public void HideVictoryEffect()
        {
            m_Victory.SetActive(false);
        }
    }
}
