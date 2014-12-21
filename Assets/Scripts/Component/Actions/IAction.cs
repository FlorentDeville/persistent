using UnityEngine;

namespace Assets.Scripts.Component.Actions
{
    public enum ActionType
    {
        CloseAction,
        NoAction
    }

    [System.Serializable]
    public class IAction
    {
        public enum Result
        {
            Over,       //The action is over
            Continue    //The action is not over and has to continue
        }

        public GameObject m_Pawn { get; set; }

        public GameObject m_Target { get; set; }

        public virtual void Prepare() { }

        public virtual Result Execute() { return Result.Over; }

        public void SetTarget(GameObject _pawnTarget)
        {
            m_Target = _pawnTarget;
        }
    }
}
