using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ALEX
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;

        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float moveAmount;

        [Header("==== MOVEMENT SETTING ====")]
        private Vector3 moveDirection;
        private Vector3 targetRotateDir;
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float rotationSpeed = 15;
        [SerializeField] float sprintSpeed = 6.5f;

        [SerializeField] int sprintingStaminaCost = 5;

        [Header("==== DODGE ====")]
        private Vector3 rollDir;
        [SerializeField] int dodgeStaminaCost = 25;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (player.IsOwner) 
            { 
                player.characterNetworkManager.verticalMovement.Value = verticalMovement;
                player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
                player.characterNetworkManager.moveAmount.Value = moveAmount;
            }
            else
            {
                verticalMovement = player.characterNetworkManager.verticalMovement.Value;
                horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
                moveAmount = player.characterNetworkManager.moveAmount.Value;

                player.playerAnim.UpdateAnimMoveParams(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
            }
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
        }

        private void GetMovementVals()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            moveAmount = PlayerInputManager.instance.moveAmount;
        }

        private void HandleGroundedMovement()
        {
            if (!player.canMove) { return; }

            GetMovementVals();

            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.characterController.Move(moveDirection * sprintSpeed * Time.deltaTime);
            }
            else
            {
                if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
                }
            }
        }

        private void HandleRotation()
        {
            if (!player.canRotate) { return; }

            targetRotateDir = Vector3.zero;
            targetRotateDir = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotateDir += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotateDir.Normalize();
            targetRotateDir.y = 0;

            if (targetRotateDir == Vector3.zero)
            {
                targetRotateDir = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotateDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }

        public void AttemptToPerformDodge()
        {
            if (player.isPerformingAction) { return; }

            if (player.playerNetworkManager.currentStamina.Value < dodgeStaminaCost) { return; }

            if (PlayerInputManager.instance.moveAmount > 0)
            {
                rollDir = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
                rollDir += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
                rollDir.Normalize();
                rollDir.y = 0;

                Quaternion playerRotation = Quaternion.LookRotation(rollDir);
                player.transform.rotation = playerRotation;

                player.playerAnim.PlayTargetActionAnim("Roll_Forward_01", true, true);
            }

            else
            {
                player.playerAnim.PlayTargetActionAnim("Step_Back_01", true, true);
            }

            player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
        }

        public void HandleSprinting()
        {
            if (player.isPerformingAction)
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

            if (player.playerNetworkManager.currentStamina.Value <= 0)
            {
                player.playerNetworkManager.isSprinting.Value = false;
                return;
            }

            if (moveAmount >= 0.5)
            {
                player.playerNetworkManager.isSprinting.Value = true;
            }

            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            }
        }
    }
}
