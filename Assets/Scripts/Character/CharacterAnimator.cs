using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace ALEX
{
    public class CharacterAnimator : MonoBehaviour
    {
        CharacterManager character;

        int vertical;
        int horizontal;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimMoveParams(float horizontalVal, float verticalVal, bool isSprinting)
        {
            float horizontalAmount = horizontalVal;
            float verticalAmount = verticalVal;

            if (isSprinting)
            {
                verticalAmount = 2;
            }

            character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
        }

        public virtual void PlayTargetActionAnim(string targetAnim, 
            bool isPerformingAction, bool applyRootMotion = true, bool _canRotate = false, bool _canMove = false)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnim, 0.2f);

            character.isPerformingAction = isPerformingAction;
            character.canMove = _canMove;
            character.canRotate = _canRotate;

            character.characterNetworkManager.NotifyServerOfActionAnimServerRpc(NetworkManager.Singleton.LocalClientId, targetAnim, applyRootMotion);
        }
    }
}
