using Assets.Scripts.Entities.World;

namespace Assets.Scripts.Component.Actions
{
    [System.Serializable]
    public class ActionEntry
    {
        public ActionId m_Action;

        public ActionType m_TypeOfAction;

        public ActionCloseAttack m_CloseAction;

        public IAction GetAction()
        {
            switch(m_TypeOfAction)
            {
                case ActionType.CloseAction:
                    return m_CloseAction;
                   
                case ActionType.NoAction:
                    return null;
            }

            return null;
        }
    }
}
