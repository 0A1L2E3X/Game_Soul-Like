using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ALEX
{
    public class WorldSavedGameManager : MonoBehaviour
    {
        public static WorldSavedGameManager instance;

        [SerializeField] PlayerManager player;

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
        private string fileName;

        [Header("CHARACTER SLOTS")]
        public CharacterSavedData characterSlot01;
        //public CharacterSavedData characterSlot02;
        //public CharacterSavedData characterSlot03;
        //public CharacterSavedData characterSlot04;
        //public CharacterSavedData characterSlot05;
        //public CharacterSavedData characterSlot06;
        //public CharacterSavedData characterSlot07;
        //public CharacterSavedData characterSlot08;
        //public CharacterSavedData characterSlot09;
        //public CharacterSavedData characterSlot10;

        private void Awake()
        {
            if (instance == null) { instance = this; }
            else { Destroy(gameObject); }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
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

        private void DetermineFileNameBasedCurrentSlotUsing()
        {
            switch (currentSlotUsing)
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
        }

        public void CreateNewGame()
        {
            DetermineFileNameBasedCurrentSlotUsing();

            currentCharacterData = new CharacterSavedData();
        }

        public void LoadGame()
        {
            DetermineFileNameBasedCurrentSlotUsing();

            savedFileDataManager = new SavedFileDataManager();
            savedFileDataManager.savedDataPath = Application.persistentDataPath;
            savedFileDataManager.savedFileName = fileName;
            currentCharacterData = savedFileDataManager.LoadSavedFile();

            StartCoroutine(LoadWorldScene());
        }

        public void SaveGame()
        {
            DetermineFileNameBasedCurrentSlotUsing();

            savedFileDataManager = new SavedFileDataManager();
            savedFileDataManager.savedDataPath = Application.persistentDataPath;
            savedFileDataManager.savedFileName = fileName;

            player.SaveGameDataToCurrentCharacter(ref currentCharacterData);

            savedFileDataManager.CreateNewCharacterFile(currentCharacterData);
        }

        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}
