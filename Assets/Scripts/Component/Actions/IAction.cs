using UnityEngine;

namespace Assets.Scripts.Component.Actions
{
    public enum ActionType
    {
        CloseAction,
        NoAction
    }

    public enum ReactionType
    {
        None,
        NormalHit,
        BigHit,
        Slow
    }

    [System.Serializable]
    public class IAction
    {
        public string m_DisplayName;

        public ReactionType m_TypeOfReaction = ReactionType.None;

        public enum Result
        {
            Over,       //The action is over
            Continue    //The action is not over and has to continue
        }

        public GameObject m_Pawn { get; set; }

        public GameObject m_Target { get; set; }

        

        public virtual void Prepare() { }

        public virtual Result Execute() { return Result.Over; }

        public virtual void Resolve(ResolveResult _result) { }

        public void SetTarget(GameObject _pawnTarget)
        {
            m_Target = _pawnTarget;
        }
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
