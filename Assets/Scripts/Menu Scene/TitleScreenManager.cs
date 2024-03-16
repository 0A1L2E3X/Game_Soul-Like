using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace ALEX
{
    public class TitleScreenManager : MonoBehaviour
    {
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void StartNewGame()
        {
            WorldSavedGameManager.instance.CreateNewGame();
            StartCoroutine(WorldSavedGameManager.instance.LoadWorldScene());
        }
    }
}
