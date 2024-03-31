using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ALEX
{
    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        public static WorldCharacterEffectsManager instance;

        [Header("DAMAGE")]
        public DamageEffects damageEffect;

        [SerializeField] List<InstanceEffects> instanceEffects;

        private void Awake()
        {
            if (instance == null) { instance = this; }
            else { Destroy(gameObject); }

            GenerateEffectIDs();
        }

        private void GenerateEffectIDs()
        {
            for (int i = 0; i < instanceEffects.Count; i++)
            {
                instanceEffects[i].instanceEffectID = i;
            }
        }
    }
}
