using UnityEngine;

namespace Assets.Scripts.Helper
{
    public class Cooldown
    {
        private float m_LastTime;
        private float m_CooldownDuration;

        public Cooldown(float _cooldownDuration)
        {
            m_LastTime = 0f;
            m_CooldownDuration = _cooldownDuration;
        }

        public bool IsCooldownElapsed()
        {
            if (m_LastTime + m_CooldownDuration < Time.fixedTime)
                return true;

            return false;
        }

        public void StartCooldown()
        {
            m_LastTime = Time.fixedTime;
        }
    }
}
