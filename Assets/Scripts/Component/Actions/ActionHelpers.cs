using UnityEngine;

namespace Assets.Scripts.Component.Actions
{
    public enum ReactionType
    {
        None,
        NormalHit,
        BigHit,
        Slow
    }

    public enum Result
    {
        Over,       //The action is over
        Continue    //The action is not over and has to continue
    }

    public class ResolveResult
    {
        public int m_Damage;

        public ResolveResult()
        {
            m_Damage = 0;
        }
    }
}
