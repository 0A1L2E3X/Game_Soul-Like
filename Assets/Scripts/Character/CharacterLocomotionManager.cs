using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ALEX
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("GROUND CHECK AND HANDLE JUMP")]
        [SerializeField] protected float gravityForce = -10f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRadius = 1;
        [SerializeField] protected Vector3 yVelocity;
        [SerializeField] protected float groundedYVelocity = -20;
        [SerializeField] protected float fallStartYVelocity = -5;
        protected bool fallVelocityIsSet = false;
        protected float inAirTimer = 0;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandleGroundCheck();

            if (character.isGrounded)
            {
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallVelocityIsSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                if (!character.isJumping && !fallVelocityIsSet)
                {
                    fallVelocityIsSet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                inAirTimer += Time.deltaTime;
                character.animator.SetFloat("InAirTimer", inAirTimer);
                yVelocity.y += gravityForce * Time.deltaTime;
            }
            character.characterController.Move(yVelocity * Time.deltaTime);
        }

        protected void HandleGroundCheck()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        }

        protected void OnDrawGizmosSelected()
        {
            if (character == null) { return; }

            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }
    }
}
