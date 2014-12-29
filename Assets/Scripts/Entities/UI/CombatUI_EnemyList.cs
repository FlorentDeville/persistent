﻿using Assets.Scripts.Entities.Combat;
using Assets.Scripts.UI;
using Assets.Scripts.Manager;

using System;

using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 649

namespace Assets.Scripts.Entities.UI
{
    public class CombatUI_EnemyList : MonoBehaviour
    {
        [SerializeField]
        private CustomButton[] m_BtnEnemies;

        [SerializeField]
        private GameObject m_Cursor;

        private Action<GameObject> m_OnEnemySelected;
        public Action<GameObject> OnEnemySelected
        {
            get { return m_OnEnemySelected; }
            set { m_OnEnemySelected = value; }
        }

        private Action m_OnCanvasClosed;
        public Action OnCanvasClosed
        {
            get { return m_OnCanvasClosed; }
            set { m_OnCanvasClosed = value; }
        }

        void OnEnable()
        {
            Debug.Log("on enable");
            GameTurnManager turnMng = GameTurnManager.GetInstance();
            int enemyCount = turnMng.m_EnemiesPawns.Count;
            int buttonUsed = 0;
            for (int i = 0; i < enemyCount; ++i)
            {
                GameObject objPawn = turnMng.m_EnemiesPawns[i];
                PawnBehavior bhv = objPawn.GetComponent<PawnBehavior>();
                if (bhv.m_State == PawnState.Dead)
                    continue;

                CustomButton btn = m_BtnEnemies[buttonUsed];
                btn.gameObject.SetActive(true);
                Text txt = btn.transform.GetComponentInChildren<Text>();
                txt.text = objPawn.GetComponent<PawnStatistics>().m_PawnName;

                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                    {
                        if (OnEnemySelected != null)
                            OnEnemySelected(objPawn);
                    });

                btn.onCancel.RemoveAllListeners();
                btn.onCancel.AddListener(() =>
                    {
                        WidgetManager.GetInstance().Hide();
                        if (OnCanvasClosed != null)
                            OnCanvasClosed();
                    });

                btn.onSelect.RemoveAllListeners();
                btn.onSelect.AddListener(() =>
                    {
                        Vector3 camPosition = Camera.main.WorldToScreenPoint(objPawn.transform.position);
                        m_Cursor.transform.position = camPosition;
                    });

                btn.Send(WidgetEvent.Unselect);

                ++buttonUsed;
            }

            for (int i = buttonUsed; i < m_BtnEnemies.Length; ++i)
            {
                CustomButton btn = m_BtnEnemies[i];
                btn.gameObject.SetActive(false);
            }

            m_BtnEnemies[0].Send(WidgetEvent.Select);
            m_Cursor.SetActive(true);
        }

        public void Show()
        {
           // WidgetManager.GetInstance().Show(gameObject, false, false);
        }

        public void SetCursorPosition(ref GameObject _obj)
        {
            Vector3 camPosition = Camera.main.WorldToScreenPoint(_obj.transform.position);
            m_Cursor.transform.position = camPosition;
        }


    }
}
