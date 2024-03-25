using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ALEX
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] UI_StatBar healthBar;
        [SerializeField] UI_StatBar staminaBar;

        public void RefreshHUD()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);
            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }

        public void SetNewHealthVal(float oldVal, float newVal)
        {
            healthBar.SetStats(Mathf.RoundToInt(newVal));
        }

        public void SetMaxHealthVal(int maxHealth)
        {
            healthBar.SetMaxStats(maxHealth);
        }

        public void SetNewStaminaVal(float oldVal, float newVal)
        {
            staminaBar.SetStats(Mathf.RoundToInt(newVal));
        }

        public void SetMaxStaminaVal(int maxStamina)
        {
            staminaBar.SetMaxStats(maxStamina);
        }
    }
}
