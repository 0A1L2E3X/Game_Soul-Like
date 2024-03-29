using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ALEX
{
    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager instance;

        [Header("==== ACTION SOUND ====")]
        public AudioClip rollSFX;

        private void Awake()
        {
            if (instance == null) { instance = this; }
            else { Destroy(gameObject); }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
