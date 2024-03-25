using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq.Expressions;

namespace ALEX
{
    [System.Serializable]
    public class CharacterSavedData
    {
        [Header("SCENE INDEX")]
        public int sceneIndex = 1;

        [Header("CHARACTER NAME")]
        public string characterName = "Character";

        [Header("TIME PLAYED")]
        public float secondsPlayed;

        [Header("WORLD COORDINATES")]
        public float xPos;
        public float yPos;
        public float zPos;

        [Header("RESOURCES")]
        public float currentHealth;
        public float currentStamina;

        [Header("STATS")]
        public int vitality;
        public int endurance;
    }
}
