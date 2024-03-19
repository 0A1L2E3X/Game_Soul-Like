using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ALEX
{
    public class TitleScreenLoadInputManager : MonoBehaviour
    {
        PlayerControls playerControls;

        [Header("TITLE SCREEN INPUTS")]
        [SerializeField] bool deleteCharacterSlot = false;

        private void Update()
        {
            if (deleteCharacterSlot)
            {
                deleteCharacterSlot = false;
                TitleScreenManager.instance.AttemptToDeleteSlot();
            }
        }

        private void OnEnable()
        {
            if (playerControls ==  null)
            {
                playerControls = new PlayerControls();
                playerControls.UI.X.performed += i => deleteCharacterSlot = true;
            }

            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
    }
}
