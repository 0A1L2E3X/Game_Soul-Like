using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ALEX
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        [Header("DEBUG DELETE LATER")]
        [SerializeField] InstanceEffects effectToTest;
        [SerializeField] bool processEffects = false;

        private void Update()
        {
            if (processEffects)
            {
                processEffects = false;
                InstanceEffects effects = Instantiate(effectToTest);
                ProcessInstanceEffects(effects);
            }
        }
    }
}
