using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace ALEX
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        CharacterManager character;

        [Header("POSITION")]
        public NetworkVariable<Vector3> networkPosition =
            new(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Quaternion> networkRotation =
            new(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public Vector3 networkPositionVelocity;

        public float networkPositionSmoothTime = 0.1f;
        public float networkRotationSmoothTime = 0.1f;

        [Header("ANIMATOR")]
        public NetworkVariable<float> horizontalMovement = 
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> verticalMovement = 
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> moveAmount =
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("FLAGS")]
        public NetworkVariable<bool> isSprinting =
            new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("STATS")]
        public NetworkVariable<int> endurance =
            new(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> vitality =
            new(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("RESOURCES")]
        public NetworkVariable<float> currentStamina =
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxStamina =
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public NetworkVariable<float> currentHealth =
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxHealth =
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        [ServerRpc]
        public void NotifyServerOfActionAnimServerRpc(ulong clientID, string animationID, bool _applyRootMotion)
        {
            if (IsServer)
            {
                PlayActionAnimForAllClientClientRpc(clientID, animationID, _applyRootMotion);
            }
        }

        [ClientRpc]
        public void PlayActionAnimForAllClientClientRpc(ulong clientID, string animationID, bool _applyRootMotion)
        {
            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                PerformActionAnimFromServer(animationID, _applyRootMotion);
            }
        }

        private void PerformActionAnimFromServer(string animationID, bool _applyRootMotion)
        {
            character.applyRootMotion = _applyRootMotion;
            character.animator.CrossFade(animationID, 0.2f);
        }
    }
}
