using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace ALEX
{
    public class CharacterStatManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("==== STAMINA ====")]
        private float staminaRegenerationTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField] float staminaRegenerationDelay = 1;
        [SerializeField] int staminaRegenerationAmount = 1;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public int CalcuStaminaBasedOnLV(int endurance)
        {
            float stamina = 0;

            stamina = endurance * 10;

            return Mathf.RoundToInt(stamina);
        }

        public virtual void RegenerateStamina()
        {
            if (!character.IsOwner) { return; }

            if (character.characterNetworkManager.isSprinting.Value) { return; }

            if (character.isPerformingAction) { return; }

            staminaRegenerationTimer += Time.deltaTime;

            if (staminaRegenerationTimer >= staminaRegenerationDelay)
            {
                if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
                {
                    staminaTickTimer += Time.deltaTime;

                    if (staminaTickTimer >= 0.1f)
                    {
                        staminaTickTimer = 0;
                        character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                    }
                }
            }
        }

        public virtual void ResetStaminaRegenerationTimer(float oldVal, float newVal)
        {
            if (newVal < oldVal)
            {
                staminaRegenerationTimer = 0;
            }
        }
    }
}
