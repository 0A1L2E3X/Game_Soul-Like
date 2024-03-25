using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ALEX
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Stamina Damage")]
    public class StaminaDamageEffects : InstanceEffects
    {
        public float staminaDamage;

        public override void ProcessEffect(CharacterManager character)
        {
            CalcuStaminaDamage(character);
        }

        private void CalcuStaminaDamage(CharacterManager character)
        {
            if (character.IsOwner)
            {
                character.characterNetworkManager.currentStamina.Value -= staminaDamage;
            }
        }
    }
}
