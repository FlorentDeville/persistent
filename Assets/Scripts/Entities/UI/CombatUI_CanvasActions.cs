using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Entities.UI
{
    public class CombatUI_CanvasActions : MonoBehaviour
    {
        public GameObject m_AttackButton;
        public GameObject m_MagicButton;
        public GameObject m_ItemButton;

        public GameObject m_Cursor;

        public void ActivateMenu()
        {
            gameObject.SetActive(true);
            for(int i = 0; i < transform.childCount; ++i)
            {
                GameObject obj = transform.GetChild(i).gameObject;
                obj.SetActive(true);
            }

            m_Cursor.SetActive(false);
            m_AttackButton.GetComponent<Button>().Select();
        }

        public void DeactivateMenu()
        {
            gameObject.SetActive(false);
        }
    }
}
