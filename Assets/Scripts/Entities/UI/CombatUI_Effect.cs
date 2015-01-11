using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 649

namespace Assets.Scripts.Entities.UI
{
    public class CombatUI_Effect : MonoBehaviour
    {
        [SerializeField]
        [Header("Show Attack Name")]
        private GameObject m_ShowAttackControl;

        [SerializeField]
        private string m_TriggerShowAttack;

        [SerializeField]
        [Header("Victory")]
        private GameObject m_Victory;

        [SerializeField]
        [Header("Game Over")]
        private GameObject m_GameOver;

        [SerializeField]
        [Header("Damage")]
        private GameObject m_DamageWrapper;

        [SerializeField]
        private Text m_DamageText;

        [SerializeField]
        private string m_DamageStartTrigger;

        [SerializeField]
        private string m_DamageEndState;

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

        public void ShowGameOverEffect()
        {
            m_GameOver.SetActive(true);
        }

        public void HideGameOverEffect()
        {
            m_GameOver.SetActive(false);
        }

        public void StartDamageEffect(Vector3 _screenPosition, string _text)
        {
            m_DamageText.text = _text;
            m_DamageWrapper.transform.position = _screenPosition;
            m_DamageWrapper.SetActive(true);

            Animator anim = m_DamageWrapper.GetComponentInChildren<Animator>();
            anim.SetTrigger(m_DamageStartTrigger);
        }

        public void HideDamageEffect()
        {
            m_DamageWrapper.SetActive(false);
        }

        public bool IsDamageEffectOver()
        {
            Animator anim = m_DamageWrapper.GetComponentInChildren<Animator>();
            if (anim == null)
                return true;
            int endStateHash = Animator.StringToHash(m_DamageEndState);
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
            if (info.nameHash == endStateHash)
                return true;

            return false;
        }
    }
}
