using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace ALEX
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager instance;

        [Header("MENUS")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;

        [Header("BUTTON")]
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadButton;
        [SerializeField] Button mainMenuNewGameButton;
        [SerializeField] Button deleteCharacterConfirmButton;

        [Header("POP UP")]
        [SerializeField] GameObject noFreeSlotPopUp;
        [SerializeField] Button btn_noFreeSlot;
        
        [SerializeField] GameObject deleteCharacterSlotPopUp;

        [Header("SAVE SLOT")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

        private void Awake()
        {
            if (instance == null) { instance = this; }
            else { Destroy(gameObject); }
        }

        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            WorldSavedGameManager.instance.AttemptToCreateNewGame();
        }

        public void OpenLoadGameMenu()
        {
            titleScreenMainMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();
        }

        public void CloseLoadGameMenu()
        {
            titleScreenLoadMenu.SetActive(false);
            titleScreenMainMenu.SetActive(true);

            mainMenuLoadButton.Select();
        }

        public void DisplayNoFreeCharacter()
        {
            noFreeSlotPopUp.SetActive(true);
            btn_noFreeSlot.Select();
        }

        public void CloseNoFreeSlotPopUp()
        {
            noFreeSlotPopUp.SetActive(false);
            mainMenuNewGameButton.Select();
        }

        public void SelectCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedSlot = characterSlot;
        }

        public void SelectNoSlot()
        {
            currentSelectedSlot = CharacterSlot.NO_SLOT;
        }

        public void AttemptToDeleteSlot()
        {
            if (currentSelectedSlot != CharacterSlot.NO_SLOT) 
            {
                deleteCharacterSlotPopUp.SetActive(true);
                deleteCharacterConfirmButton.Select();
            }
        }

        public void DeleteCharacter()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            WorldSavedGameManager.instance.DeleteGame(currentSelectedSlot);

            titleScreenLoadMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);
            
            loadMenuReturnButton.Select();
        }

        public void CloseDeleteCharacterPopUp()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            loadMenuReturnButton.Select();
        }
    }
}
