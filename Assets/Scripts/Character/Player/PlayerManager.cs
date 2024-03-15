using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ALEX
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimator playerAnim;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatManager playerStatManager;

        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnim = GetComponent<PlayerAnimator>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatManager = GetComponent<PlayerStatManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (!IsOwner) { return; }

            playerLocomotionManager.HandleAllMovement();

            playerStatManager.RegenerateStamina();
        }

        protected override void LateUpdate()
        {
            if (!IsOwner) { return; }
            base.LateUpdate();

            PlayerCamera.instance.HandleAllCameraActions();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsOwner)
            {
                PlayerCamera.instance.player = this;
                PlayerInputManager.instance.player = this;

                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaVal;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatManager.ResetStaminaRegenerationTimer;

                playerNetworkManager.maxStamina.Value = playerStatManager.CalcuStaminaBasedOnLV(playerNetworkManager.endurance.Value);
                playerNetworkManager.currentStamina.Value = playerStatManager.CalcuStaminaBasedOnLV(playerNetworkManager.endurance.Value);
                PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaVal(playerNetworkManager.maxStamina.Value);
            }
        }
    }
}
