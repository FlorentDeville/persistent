using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 649

namespace Assets.Scripts.Entities.UI
{
    public class CombatUI_Effect : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_ShowAttackControl;

        [SerializeField]
        private string m_TriggerShowAttack;

        void Awake()
        {
            m_ShowAttackControl.SetActive(false);
        }

        public void StartAnimationShowAttackName(string _attackName)
        {
            m_ShowAttackControl.SetActive(true);
            m_ShowAttackControl.GetComponentInChildren<Text>().text = _attackName;

            Animator anim = m_ShowAttackControl.GetComponent<Animator>();
            anim.SetTrigger(m_TriggerShowAttack);

        }
    }
}
