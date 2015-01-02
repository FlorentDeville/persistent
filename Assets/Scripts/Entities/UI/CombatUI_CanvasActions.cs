using Assets.Scripts.Assets;
using Assets.Scripts.Component.Actions;
using Assets.Scripts.Entities.Combat;
using Assets.Scripts.Entities.World;
using Assets.Scripts.UI;
using Assets.Scripts.Manager;

using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 649

namespace Assets.Scripts.Entities.UI
{
    public class CombatUI_CanvasActions : MonoBehaviour
    {
        public CustomButton m_AttackButton;
        public CustomButton m_MagicButton;
        public CustomButton m_ItemButton;

        [SerializeField]
        private CombatUI_EnemyList m_CanvasEnemyList;

        [SerializeField]
        private CombatUI_MagicList m_CanvasMagicList;

        private Vector3 m_EnemyListPositionSave;

        void Awake()
        {
            m_EnemyListPositionSave = m_CanvasEnemyList.transform.position;    
        }

        public void Show()
        {
            gameObject.SetActive(true);
            for(int i = 0; i < transform.childCount; ++i)
            {
                GameObject obj = transform.GetChild(i).gameObject;
                obj.SetActive(true);
            }

            m_AttackButton.Send(WidgetEvent.Select);
            m_MagicButton.Send(WidgetEvent.Unselect);
            m_ItemButton.Send(WidgetEvent.Unselect);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnAttackClicked()
        {
            WidgetManager.GetInstance().Show(m_CanvasEnemyList.gameObject, false, false);
            m_CanvasEnemyList.OnEnemySelected = OnAttackEnemySelected;
            m_CanvasEnemyList.Show();
            m_CanvasEnemyList.SetColumn(0);
        }

        public void OnMagicClicked()
        {
            WidgetManager.GetInstance().Show(m_CanvasMagicList.gameObject, false, false);
            int characterId = GameTurnManager.GetInstance().GetCurrentPawn().GetComponent<PawnBehavior>().m_CharacterId;
            MagicId[] availableMagic = GameStateManager.GetInstance().GetCharacter(characterId).m_AvailableMagic;
            m_CanvasMagicList.InitializeMagicList(availableMagic);

            m_CanvasMagicList.OnClicked = OnMagicMagicSelected;
        }

        private void OnAttackEnemySelected(GameObject _enemy)
        {
            ActionRunner attackAction = GameTurnManager.GetInstance().GetCurrentPawn().GetComponent<PawnActions>().m_DefaultAttack;
            attackAction.m_Target = _enemy;
            attackAction.ActionDescription = GameTurnManager.GetInstance().GetCurrentPawn().GetComponent<PawnBehavior>().m_AttackDescription;
            
            GameMaster.GetInstance().SetSelectedAction(attackAction);
            GameMaster.GetInstance().ActionReady();
        }

        private void OnMagicMagicSelected(MagicId _id)
        {
            //get the action
            MagicDescription desc = MagicManager.GetInstance().GetDescription(_id);
            ActionRunner magicAction = GameTurnManager.GetInstance().GetCurrentPawn().GetComponent<PawnActions>().GetAction(_id);
            magicAction.ActionDescription = desc;

            //set the action in the game master
            GameMaster.GetInstance().SetSelectedAction(magicAction);

            //show enemy selection
            WidgetManager.GetInstance().Show(m_CanvasEnemyList.gameObject, false, false);
            m_CanvasEnemyList.OnEnemySelected = OnMagicEnemySelected;
            m_CanvasEnemyList.OnCanvasClosed = OnMagicEnemyClosed;
            m_CanvasEnemyList.Show();
            m_CanvasEnemyList.SetColumn(1);
        }

        private void OnMagicEnemySelected(GameObject _enemy)
        {
            //Set the enemy in the action
            ActionRunner act = GameMaster.GetInstance().GetSelectedAction();
            act.m_Target = _enemy;

            GameMaster.GetInstance().ActionReady();
        }

        private void OnMagicEnemyClosed()
        {
            m_CanvasMagicList.ShowMagicList();
        }
    }
}
