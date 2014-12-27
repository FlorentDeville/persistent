using Assets.Scripts.Entities.World;
using Persistent.WorldEntity;

namespace CinemaDirector
{
    [CutsceneItemAttribute("Level Master", "Start Battle")]
    public class StartBattleGlobal : CinemaGlobalEvent
    {
        public BaseEnemy_CombatSettings m_Settings;

        //public override void Initialize()
        //{
        //    if (target != null)
        //    {
        //        cachedState = target.activeInHierarchy;
        //    }
        //}

        public override void Trigger()
        {
            LevelMaster.GetInstance().StartBattleTransition(m_Settings);
        }

        //public override void Reverse()
        //{
        //    if (target != null)
        //    {
        //        target.SetActive(previousState);
        //    }
        //}

        //public override void Stop()
        //{
        //    target.SetActive(cachedState);
        //}
    }
}
