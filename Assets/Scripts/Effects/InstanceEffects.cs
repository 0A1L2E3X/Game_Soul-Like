using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ALEX
{
    public class InstanceEffects : ScriptableObject
    {
        [Header("EFFECT ID")]
        public int instanceEffectID;

        public virtual void ProcessEffect(CharacterManager character)
        {

        }
    }
}