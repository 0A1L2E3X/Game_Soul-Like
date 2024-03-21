using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace ALEX
{
    public class CharacterManager : NetworkBehaviour
    {
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;

        [HideInInspector] public CharacterNetworkManager characterNetworkManager;

        [Header("==== FLAGS ====")]
        public bool isPerformingAction = false;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;
        public bool isJumping = false;
        public bool isGrounded = true;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();

            animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            animator.SetBool("IsGrounded", isGrounded);

            if (IsOwner)
            {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            else
            {
                transform.position = Vector3.SmoothDamp(
                    transform.position, 
                    characterNetworkManager.networkPosition.Value, 
                    ref characterNetworkManager.networkPositionVelocity,
                    characterNetworkManager.networkPositionSmoothTime);

                transform.rotation = Quaternion.Slerp(transform.rotation,
                    characterNetworkManager.networkRotation.Value, 
                    characterNetworkManager.networkPositionSmoothTime);
            }
        }

        protected virtual void LateUpdate()
        {

        }
    }
}
