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

                playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;

                playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewHealthVal;
                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaVal;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatManager.ResetStaminaRegenerationTimer;
            }
        }

        public void SaveGameDataToCurrentCharacter(ref CharacterSavedData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;

            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.xPos = transform.position.x;
            currentCharacterData.yPos = transform.position.y;
            currentCharacterData.zPos = transform.position.z;

            currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
            currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;

            currentCharacterData.vitality = playerNetworkManager.vitality.Value;
            currentCharacterData.endurance = playerNetworkManager.endurance.Value;
        }

        public void LoadGameDataFromCurrentCharacter(ref CharacterSavedData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            Vector3 currentPos = new(currentCharacterData.xPos, currentCharacterData.yPos, currentCharacterData.zPos);
            transform.position = currentPos;

            playerNetworkManager.vitality.Value = currentCharacterData.vitality;
            playerNetworkManager.endurance.Value = currentCharacterData.endurance;

            playerNetworkManager.maxHealth.Value = playerStatManager.CalcuHealthBasedOnLV(playerNetworkManager.vitality.Value);
            playerNetworkManager.maxStamina.Value = playerStatManager.CalcuStaminaBasedOnLV(playerNetworkManager.endurance.Value);

            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaVal(playerNetworkManager.maxStamina.Value);
        }
    }
}
