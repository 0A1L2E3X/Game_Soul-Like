using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ALEX
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [SerializeField] UI_StatBar staminaBar;

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
