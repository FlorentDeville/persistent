using Assets.Scripts.Entities.World;

namespace Assets.Scripts.Component.Actions
{
    [System.Serializable]
    public class NewMagicActionOverrideEntry
    {
        public MagicId m_Magic;
        public Actions.ActionRunner m_Action;
    }
}
