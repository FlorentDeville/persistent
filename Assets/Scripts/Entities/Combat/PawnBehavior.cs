using Assets.Scripts.Component.Actions;
using UnityEngine;

namespace Assets.Scripts.Entities.Combat
{
    public class PawnBehavior : MonoBehaviour
    {
        public PawnState m_State;

        public void Awake()
        {
            m_State = PawnState.Alive;
        }
    }

    public enum PawnState
    {
        Alive,
        Dead
    }
}
