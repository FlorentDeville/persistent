using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Entities.UI
{
    public class CombatUI_PawnState : MonoBehaviour
    {
        public Image m_avatar;

        public Text m_PawnName;

        public Text m_HPText;

        public Slider m_HPSlider;

        public Text m_MPText;

        public Slider m_MPSlider;

        public void SetAvatar(Sprite _avatar)
        {
            m_avatar.sprite = _avatar;
        }

        public void SetHP(float _hp, float _hpMax)
        {
            m_HPSlider.minValue = 0;
            m_HPSlider.maxValue = _hpMax;
            m_HPSlider.value = _hp;

            m_HPText.text = string.Format("{0} / {1}", _hp, _hpMax);
        }

        public void SetMP(float _mp, float _mpMax)
        {
            m_MPSlider.minValue = 0;
            m_MPSlider.maxValue = _mpMax;
            m_MPSlider.value = _mp;

            m_MPText.text = string.Format("{0} / {1}", _mp, _mpMax);
        }

        public void SetName(string _name)
        {
            m_PawnName.text = _name;
        }

    }
}
