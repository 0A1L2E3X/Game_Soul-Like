using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ALEX
{
    public class WorldSavedGameManager : MonoBehaviour
    {
        public static WorldSavedGameManager instance;

        public PlayerManager player;

        [Header("SAVE/LOAD")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        [Header("WORLD SCENE INDEX")]
        [SerializeField] int worldSceneIndex = 1;

        [Header("SAVED DATA WRITER")]
        private SavedFileDataManager savedFileDataManager;

        [Header("CURRENT CHARACTER DATA")]
        public CharacterSlot currentSlotUsing;
        public CharacterSavedData currentCharacterData;
        private string saveFileName;

        [Header("CHARACTER SLOTS")]
        public CharacterSavedData characterSlot01;
        public CharacterSavedData characterSlot02;
        public CharacterSavedData characterSlot03;
        public CharacterSavedData characterSlot04;
        public CharacterSavedData characterSlot05;
        public CharacterSavedData characterSlot06;
        public CharacterSavedData characterSlot07;
        public CharacterSavedData characterSlot08;
        public CharacterSavedData characterSlot09;
        public CharacterSavedData characterSlot10;

        private void Awake()
        {
            if (instance == null) { instance = this; }
            else { Destroy(gameObject); }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            PreloadAllProfiles();
        }

        private void Update()
        {
            if (saveGame) 
            { 
                saveGame = false;
                SaveGame();
            }

            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        public string DetermineFileNameBasedCurrentSlotUsing(CharacterSlot characterSlot)
        {
            string fileName = "";

            switch (characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    fileName = "characterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    fileName = "characterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    fileName = "characterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    fileName = "characterSlot_04";
                    break;
                case CharacterSlot.CharacterSlot_05:
                    fileName = "characterSlot_05";
                    break;
                case CharacterSlot.CharacterSlot_06:
                    fileName = "characterSlot_06";
                    break;
                case CharacterSlot.CharacterSlot_07:
                    fileName = "characterSlot_07";
                    break;
                case CharacterSlot.CharacterSlot_08:
                    fileName = "characterSlot_08";
                    break;
                case CharacterSlot.CharacterSlot_09:
                    fileName = "characterSlot_09";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    fileName = "characterSlot_10";
                    break;
                default:
                    break;
            }

            return fileName;
        }

        public void AttemptToCreateNewGame()
        {
            savedFileDataManager = new SavedFileDataManager();
            savedFileDataManager.saveDataPath = Application.persistentDataPath;

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_01);

            if (!savedFileDataManager.CheckFileExist())
            {
                currentSlotUsing = CharacterSlot.CharacterSlot_01;
                currentCharacterData = new CharacterSavedData();
                NewGame();
                return;
            }

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_02);

            if (!savedFileDataManager.CheckFileExist())
            {
                currentSlotUsing = CharacterSlot.CharacterSlot_02;
                currentCharacterData = new CharacterSavedData();
                NewGame();
                return;
            }

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_03);

            if (!savedFileDataManager.CheckFileExist())
            {
                currentSlotUsing = CharacterSlot.CharacterSlot_03;
                currentCharacterData = new CharacterSavedData();
                NewGame();
                return;
            }

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_04);

            if (!savedFileDataManager.CheckFileExist())
            {
                currentSlotUsing = CharacterSlot.CharacterSlot_04;
                currentCharacterData = new CharacterSavedData();
                NewGame();
                return;
            }

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_05);

            if (!savedFileDataManager.CheckFileExist())
            {
                currentSlotUsing = CharacterSlot.CharacterSlot_05;
                currentCharacterData = new CharacterSavedData();
                NewGame();
                return;
            }

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_06);

            if (!savedFileDataManager.CheckFileExist())
            {
                currentSlotUsing = CharacterSlot.CharacterSlot_06;
                currentCharacterData = new CharacterSavedData();
                NewGame();
                return;
            }

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_07);

            if (!savedFileDataManager.CheckFileExist())
            {
                currentSlotUsing = CharacterSlot.CharacterSlot_07;
                currentCharacterData = new CharacterSavedData();
                NewGame();
                return;
            }

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_08);

            if (!savedFileDataManager.CheckFileExist())
            {
                currentSlotUsing = CharacterSlot.CharacterSlot_08;
                currentCharacterData = new CharacterSavedData();
                NewGame();
                return;
            }

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_09);

            if (!savedFileDataManager.CheckFileExist())
            {
                currentSlotUsing = CharacterSlot.CharacterSlot_09;
                currentCharacterData = new CharacterSavedData();
                NewGame();
                return;
            }

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_10);

            if (!savedFileDataManager.CheckFileExist())
            {
                currentSlotUsing = CharacterSlot.CharacterSlot_10;
                currentCharacterData = new CharacterSavedData();
                NewGame();
                return;
            }

            TitleScreenManager.instance.DisplayNoFreeCharacter();
        }

        private void NewGame()
        {
            player.playerNetworkManager.vitality.Value = 5;
            player.playerNetworkManager.endurance.Value = 10;

            SaveGame();
            StartCoroutine(LoadWorldScene());
        }

        public void LoadGame()
        {
            saveFileName = DetermineFileNameBasedCurrentSlotUsing(currentSlotUsing);

            savedFileDataManager = new SavedFileDataManager
            {
                saveDataPath = Application.persistentDataPath,
                saveFileName = saveFileName
            };
            currentCharacterData = savedFileDataManager.LoadSavedFile();

            StartCoroutine(LoadWorldScene());
        }

        public void SaveGame()
        {
            saveFileName = DetermineFileNameBasedCurrentSlotUsing(currentSlotUsing);

            savedFileDataManager = new SavedFileDataManager
            {
                saveDataPath = Application.persistentDataPath,
                saveFileName = saveFileName
            };

            player.SaveGameDataToCurrentCharacter(ref currentCharacterData);

            savedFileDataManager.CreateNewCharacterFile(currentCharacterData);
        }

        public void DeleteGame(CharacterSlot characterSlot)
        {
            saveFileName = DetermineFileNameBasedCurrentSlotUsing(characterSlot);

            savedFileDataManager = new SavedFileDataManager
            {
                saveDataPath = Application.persistentDataPath,
                saveFileName = saveFileName
            };
            savedFileDataManager.DeleteSavedFile();
        }

        private void PreloadAllProfiles()
        {
            savedFileDataManager = new SavedFileDataManager
            {
                saveDataPath = Application.persistentDataPath
            };

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_01);
            characterSlot01 = savedFileDataManager.LoadSavedFile();

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_02);
            characterSlot02 = savedFileDataManager.LoadSavedFile();

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_03);
            characterSlot03 = savedFileDataManager.LoadSavedFile();

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_04);
            characterSlot04 = savedFileDataManager.LoadSavedFile();

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_05);
            characterSlot05 = savedFileDataManager.LoadSavedFile();

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_06);
            characterSlot06 = savedFileDataManager.LoadSavedFile();

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_07);
            characterSlot07 = savedFileDataManager.LoadSavedFile();

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_08);
            characterSlot08 = savedFileDataManager.LoadSavedFile();

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_09);
            characterSlot09 = savedFileDataManager.LoadSavedFile();

            savedFileDataManager.saveFileName = DetermineFileNameBasedCurrentSlotUsing(CharacterSlot.CharacterSlot_10);
            characterSlot09 = savedFileDataManager.LoadSavedFile();
        }

        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
            //AsyncOperation loadOperation = SceneManager.LoadSceneAsync(currentCharacterData.sceneIndex);

            player.LoadGameDataFromCurrentCharacter(ref currentCharacterData);

            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}
