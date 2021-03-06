﻿using AssemblyCSharp;
using UnityEngine;
using Assets.Scripts.Component.Actions;
using Assets.Scripts.UI;

public partial class GameMaster : MonoBehaviour
{
    public partial class GameMaster_State_RunSingleTurn : IFSMState<GameMaster>
    {
        public class RunSingleTurn_State_PlayerTurn : IFSMState<GameMaster>
        {
            public override int State { get { return (int)RunSingleTurnState.PlayerTurn; } }

            private bool m_IsActionSelected;

            public override void OnEnter()
            {
                m_Behavior.ShowActionsMenu();
                m_IsActionSelected = false;
            }

            public override void OnExecute()
            {
                if (m_IsActionSelected)
                    m_Runner.SetCurrentState((int)RunSingleTurnState.RunAction, "action selected by player");
            }

            public override void OnExit()
            {
                WidgetManager.GetInstance().HideAll();
            }

            public void SetSelectedAction()
            {
                m_IsActionSelected = true;
            }
        }
    }
}
