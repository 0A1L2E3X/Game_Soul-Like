using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ALEX
{
    public class PlayerStatManager : CharacterStatManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();

            CalcuHealthBasedOnLV(player.playerNetworkManager.vitality.Value);
            CalcuStaminaBasedOnLV(player.playerNetworkManager.endurance.Value);
        }
    }
}
