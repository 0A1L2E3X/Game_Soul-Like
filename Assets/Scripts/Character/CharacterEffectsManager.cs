using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ALEX
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstanceEffects(InstanceEffects effects)
        {
            effects.ProcessEffect(character);
        }
    }
}
