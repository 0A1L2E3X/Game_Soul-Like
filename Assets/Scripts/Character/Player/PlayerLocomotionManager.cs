using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
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

        [Header("==== JUMP ====")]
        [SerializeField] float jumpHeight = 1;
        [SerializeField] int jumpStaminaCost = 15;
        [SerializeField] float jumpForwardSpeed = 5;
        [SerializeField] float freeFallSpeed = 2;
        private Vector3 jumpDir;

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
            HandleJumpingMovement();
            HandleFreeFallMovement();
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

        private void HandleJumpingMovement()
        {
            if (player.isJumping)
            {
                player.characterController.Move(jumpDir * jumpForwardSpeed * Time.deltaTime);
            }
        }

        private void HandleFreeFallMovement()
        {
            if (!player.isGrounded)
            {
                Vector3 freeFallDir;

                freeFallDir = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
                freeFallDir += PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
                freeFallDir.y = 0;

                player.characterController.Move(freeFallDir * freeFallSpeed * Time.deltaTime);
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

        public void AttemptToPerformJump()
        {
            if (player.isPerformingAction) { return; }

            if (player.playerNetworkManager.currentStamina.Value < jumpStaminaCost) { return; }

            if (player.isJumping) { return; }

            if (!player.isGrounded) { return; }

            player.playerAnim.PlayTargetActionAnim("Main_Jump_01", false);
            player.isJumping = true;

            player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;

            jumpDir = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            jumpDir += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;

            jumpDir.y = 0;


            if (jumpDir != Vector3.zero)
            {
                if (player.playerNetworkManager.isSprinting.Value)
                {
                    jumpDir *= 1;
                }
                else if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    jumpDir *= 0.5f;
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    jumpDir *= 0.25f;
                }
            }
            
        }

        public void ApplyJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
    }
}
