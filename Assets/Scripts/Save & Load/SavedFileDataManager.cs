using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ALEX
{
    public class SavedFileDataManager
    {
        public string saveDataPath = "";
        public string saveFileName = "";

        public bool CheckFileExist()
        {
            if (File.Exists(Path.Combine(saveDataPath, saveFileName))) { return true; }
            else { return false; }
        }

        public void DeleteSavedFile()
        {
            File.Delete(Path.Combine(saveDataPath, saveFileName));
        }

        public void CreateNewCharacterFile(CharacterSavedData data)
        {
            string filePath = Path.Combine(saveDataPath, saveFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                Debug.Log("CREATING SAVE FILE, AT: " + filePath);

                string dataToStore = JsonUtility.ToJson(data, true);

                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    using (StreamWriter fileWriter = new(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }
            }

            catch (Exception e)
            {
                Debug.Log("ERROR WHILE SAVING DATA, CANNOT SAVE GAME DATA" + saveDataPath + "\n" + e);
            }
        }

        public CharacterSavedData LoadSavedFile()
        {
            CharacterSavedData data = null;

            string loadPath = Path.Combine(saveDataPath, saveFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";

                    using (FileStream stream = new(loadPath, FileMode.Open))
                    {
                        using (StreamReader reader = new(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    data = JsonUtility.FromJson<CharacterSavedData>(dataToLoad);
                }

                catch (Exception e) { Debug.Log(e); }
            }

            return data;
        }
    }
}
