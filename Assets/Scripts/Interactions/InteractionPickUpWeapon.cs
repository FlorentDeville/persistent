using AssemblyCSharp;
using Assets.Scripts.Assets;
using Assets.Scripts.Manager;

using UnityEngine;

namespace Assets.Scripts.Interactions
{
    public class InteractionPickUpWeapon : Interaction
    {
        public Weapon[] m_WeaponsToPickUp;
        public bool m_MakeDisappear;

        private bool m_ShowUI;
        private int m_CurrentWeaponId;

        public override void ExecuteStart()
        {
            m_CurrentWeaponId = 0;
            m_ShowUI = true;
        }

        public override void ExecuteUpdate()
        {
            if (m_ShowUI)
            {
                string buttonName = InputHelper.GetButtonName(InputButton.INPUT_BUTTON_A);
                if (Input.GetButtonDown(buttonName))
                {
                    if (m_CurrentWeaponId == m_WeaponsToPickUp.Length)
                        m_ShowUI = false;

                    GameStateManager.GetInstance().AddToInventory(m_WeaponsToPickUp[m_CurrentWeaponId]);
                    ++m_CurrentWeaponId;
                }
            }

            if (m_CurrentWeaponId == m_WeaponsToPickUp.Length && m_MakeDisappear)
                gameObject.SetActive(false);
        }

        public override void ExecuteOnGUI()
        {
            if (m_ShowUI)
            {
                string text = string.Format("You found one {0}", m_WeaponsToPickUp[m_CurrentWeaponId].m_WeaponName);
                RendererDialog.Render(null, null, text, InputButton.INPUT_BUTTON_A);
            }
        }
    }
}
