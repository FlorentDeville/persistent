using Assets.Scripts.Component.Actions;
using Assets.Scripts.Entities.Combat;
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

        //public GameObject m_Cursor;
        [SerializeField]
        private CombatUI_EnemyList m_CanvasEnemyList;

        void Awake()
        {
            
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
        }

        private void OnAttackEnemySelected(GameObject _enemy)
        {
            IAction attackAction = GameTurnManager.GetInstance().GetCurrentPawn().GetComponent<PawnActions>().GetAttackAction();
            attackAction.SetTarget(_enemy);
            GameMaster.GetInstance().SetSelectedAction(attackAction);
        }
    }
}
