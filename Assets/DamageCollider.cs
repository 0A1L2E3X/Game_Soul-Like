using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ALEX
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("DAMAGE")]
        public float physicalDamage = 0;
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        [Header("CONTACT POINT")]
        protected Vector3 contactPoint;

        [Header("CHARACTER DAMAGED")]
        protected List<CharacterManager> characterDamaged = new();

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                CharacterManager damageTarget = other.GetComponent<CharacterManager>();

                if (damageTarget != null )
                {
                    contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                }

                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            if (characterDamaged.Contains(damageTarget))
                return;

            characterDamaged.Add(damageTarget);

            DamageEffects damageEffects = Instantiate(WorldCharacterEffectsManager.instance.damageEffect);
            damageEffects.physicalDamage = physicalDamage;
            damageEffects.magicDamage = magicDamage;    
            damageEffects.fireDamage = fireDamage;
            damageEffects.holyDamage = holyDamage;

            damageEffects.contactPoint = contactPoint;

            damageTarget.characterEffectsManager.ProcessInstanceEffects(damageEffects);
        }
    }
}
