using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                WorldSavedGameManager.instance.player = this;

                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaVal;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatManager.ResetStaminaRegenerationTimer;

                playerNetworkManager.maxStamina.Value = playerStatManager.CalcuStaminaBasedOnLV(playerNetworkManager.endurance.Value);
                playerNetworkManager.currentStamina.Value = playerStatManager.CalcuStaminaBasedOnLV(playerNetworkManager.endurance.Value);
                PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaVal(playerNetworkManager.maxStamina.Value);
            }
        }

        public void SaveGameDataToCurrentCharacter(ref CharacterSavedData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;

            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.xPos = transform.position.x;
            currentCharacterData.yPos = transform.position.y;
            currentCharacterData.zPos = transform.position.z;
        }

        public void LoadGameDataFromCurrentCharacter(ref CharacterSavedData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            Vector3 currentPos = new(currentCharacterData.xPos, currentCharacterData.yPos, currentCharacterData.zPos);
            transform.position = currentPos;
        }
    }
}
