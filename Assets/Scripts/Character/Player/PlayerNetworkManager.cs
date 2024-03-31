using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace ALEX
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        PlayerManager player;

        public NetworkVariable<FixedString64Bytes> characterName =
            new("Player", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void SetNewMaxHealthValue(int oldVal, int newVal)
        {
            maxHealth.Value = player.playerStatManager.CalcuHealthBasedOnLV(newVal);
            PlayerUIManager.instance.playerUIHudManager.SetMaxHealthVal(maxHealth.Value);
            currentHealth.Value = maxHealth.Value;
        }

        public void SetNewMaxStaminaValue(int oldVal, int newVal)
        {
            maxStamina.Value = player.playerStatManager.CalcuStaminaBasedOnLV(newVal);
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaVal(maxStamina.Value);
            currentStamina.Value = maxStamina.Value;
        }
    }
}
