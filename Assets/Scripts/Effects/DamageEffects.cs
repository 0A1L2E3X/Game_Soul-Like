using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ALEX
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Damage")]
    public class DamageEffects : InstanceEffects
    {
        [Header("CHARACTER CAUSING DAMAGE")]
        public CharacterManager currentCharacter;

        [Header("DAMAGE")]
        public float physicalDamage = 0;
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        [Header("POISE")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

        [Header("ANIMATIONS")]
        public bool playDamageAnim = true;
        public bool manualSelectAnim = false;
        public string damageAnim;

        [Header("FINAL DAMAGE")]
        private float finalDamage = 0;

        [Header("SOUND FX")]
        public bool playDamageSFX = true;
        public AudioClip elementalSFX;

        [Header("DAMAGE DIRECTION")]
        public float angleOfHit;
        public Vector3 contactPoint;

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            //  NO DAMAGE for dead characters
            if (character.isDead.Value)
                return;

            CalculateDamage(character);
        }

        private void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (currentCharacter != null)
            {

            }

            finalDamage = Mathf.RoundToInt(physicalDamage + fireDamage + lightningDamage + holyDamage);

            if (finalDamage <= 0)
            {
                finalDamage = 1;
            }

            character.characterNetworkManager.currentHealth.Value -= finalDamage;
        }
    }
}
