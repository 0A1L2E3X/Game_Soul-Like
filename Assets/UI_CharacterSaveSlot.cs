using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ALEX
{
    public class UI_CharacterSaveSlot : MonoBehaviour
    {
        SavedFileDataManager savedFileDataManager;

        [Header("GAME SLOT")]
        public CharacterSlot characterSlot;

        [Header("CHARACTER INFO")]
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timedPlayed;

        private void OnEnable()
        {
            LoadSaveSlots();
        }

        private void LoadSaveSlots()
        {
            savedFileDataManager = new SavedFileDataManager
            {
                savedDataPath = Application.persistentDataPath
            };

            if (characterSlot == CharacterSlot.CharacterSlot_01)
            {
                savedFileDataManager.savedFileName = WorldSavedGameManager.instance.DetermineFileNameBasedCurrentSlotUsing(characterSlot);

                if (savedFileDataManager.CheckFileExist())
                {
                    characterName.text = WorldSavedGameManager.instance.characterSlot01.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }

            else if (characterSlot == CharacterSlot.CharacterSlot_02)
            {
                savedFileDataManager.savedFileName = WorldSavedGameManager.instance.DetermineFileNameBasedCurrentSlotUsing(characterSlot);

                if (savedFileDataManager.CheckFileExist())
                {
                    characterName.text = WorldSavedGameManager.instance.characterSlot02.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }

            else if (characterSlot == CharacterSlot.CharacterSlot_03)
            {
                savedFileDataManager.savedFileName = WorldSavedGameManager.instance.DetermineFileNameBasedCurrentSlotUsing(characterSlot);

                if (savedFileDataManager.CheckFileExist())
                {
                    characterName.text = WorldSavedGameManager.instance.characterSlot03.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }

            else if (characterSlot == CharacterSlot.CharacterSlot_04)
            {
                savedFileDataManager.savedFileName = WorldSavedGameManager.instance.DetermineFileNameBasedCurrentSlotUsing(characterSlot);

                if (savedFileDataManager.CheckFileExist())
                {
                    characterName.text = WorldSavedGameManager.instance.characterSlot04.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }

            else if (characterSlot == CharacterSlot.CharacterSlot_05)
            {
                savedFileDataManager.savedFileName = WorldSavedGameManager.instance.DetermineFileNameBasedCurrentSlotUsing(characterSlot);

                if (savedFileDataManager.CheckFileExist())
                {
                    characterName.text = WorldSavedGameManager.instance.characterSlot05.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }

            else if (characterSlot == CharacterSlot.CharacterSlot_06)
            {
                savedFileDataManager.savedFileName = WorldSavedGameManager.instance.DetermineFileNameBasedCurrentSlotUsing(characterSlot);

                if (savedFileDataManager.CheckFileExist())
                {
                    characterName.text = WorldSavedGameManager.instance.characterSlot06.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }

            else if (characterSlot == CharacterSlot.CharacterSlot_07)
            {
                savedFileDataManager.savedFileName = WorldSavedGameManager.instance.DetermineFileNameBasedCurrentSlotUsing(characterSlot);

                if (savedFileDataManager.CheckFileExist())
                {
                    characterName.text = WorldSavedGameManager.instance.characterSlot07.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }

            else if (characterSlot == CharacterSlot.CharacterSlot_08)
            {
                savedFileDataManager.savedFileName = WorldSavedGameManager.instance.DetermineFileNameBasedCurrentSlotUsing(characterSlot);

                if (savedFileDataManager.CheckFileExist())
                {
                    characterName.text = WorldSavedGameManager.instance.characterSlot08.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }

            else if (characterSlot == CharacterSlot.CharacterSlot_09)
            {
                savedFileDataManager.savedFileName = WorldSavedGameManager.instance.DetermineFileNameBasedCurrentSlotUsing(characterSlot);

                if (savedFileDataManager.CheckFileExist())
                {
                    characterName.text = WorldSavedGameManager.instance.characterSlot09.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }

            else if (characterSlot == CharacterSlot.CharacterSlot_10)
            {
                savedFileDataManager.savedFileName = WorldSavedGameManager.instance.DetermineFileNameBasedCurrentSlotUsing(characterSlot);

                if (savedFileDataManager.CheckFileExist())
                {
                    characterName.text = WorldSavedGameManager.instance.characterSlot10.characterName;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }

        public void LoadGameFromCharacterSlot()
        {
            WorldSavedGameManager.instance.currentSlotUsing = characterSlot;
            WorldSavedGameManager.instance.LoadGame();
        }
    }
}
