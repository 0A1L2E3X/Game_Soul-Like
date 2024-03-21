using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ALEX
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;
        public PlayerManager player;
        PlayerControls playerControls;

        [Header("==== PLAYER MOVEMENT INPUT ====")]
        [SerializeField] Vector2 movementInput;
        public float horizontalInput;
        public float verticalInput;
        public float moveAmount;

        [Header("==== CAMERA MOVEMENT INPUT ====")]
        [SerializeField] Vector2 cameraInput;
        public float cameraHorizontalInput;
        public float cameraVerticalInput;

        [Header("==== PLAYER ACTIONS INPUT ====")]
        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool jumpInput = false;
        
        private void Awake()
        {
            if (instance == null) { instance = this; }
            else { Destroy(gameObject); }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += OnSceneChanged;

            instance.enabled = false;
        }

        private void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSavedGameManager.instance.GetWorldSceneIndex()) { instance.enabled = true; }
            else { instance.enabled = false; }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();

                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;

                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus) { playerControls.Enable(); }
                else { playerControls.Disable(); }
            }
        }

        private void Update()
        {
            HandleAllInputs();
        }

        private void HandleAllInputs()
        {
            // BASIC MOVEMENT INPUT FUNCTION
            HandleMovementInput();

            // CAMERA INPUTS FUNCTION
            HandleCameraMovementInput();

            // ACTION DODGE INPUT FUNCTION
            HandleDodgeInput();

            // ACTION SPRINTING FUNCTION
            HandleSprintInput();

            // ACTION JUMP FUNCTION
            HandleJumpInput();
        }

        // ==== MOVEMENT FUNCTION ====

        private void HandleMovementInput()
        {
            horizontalInput = movementInput.x;
            verticalInput = movementInput.y;

            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            if (moveAmount <= 0.5f && moveAmount > 0) { moveAmount = 0.5f; }
            else if (moveAmount > 0.5f && moveAmount <= 1) { moveAmount = 1; }

            if (player == null) { return; }
            player.playerAnim.UpdateAnimMoveParams(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
        }

        private void HandleCameraMovementInput()
        {
            cameraHorizontalInput = cameraInput.x;
            cameraVerticalInput = cameraInput.y;
        }

        // ==== ACTIONS FUNCTION ====

        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;

                player.playerLocomotionManager.AttemptToPerformDodge();
            }
        }

        private void HandleSprintInput()
        {
            if (sprintInput)
            {
                player.playerLocomotionManager.HandleSprinting();
            }

            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }

        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;

                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }
    }
}
