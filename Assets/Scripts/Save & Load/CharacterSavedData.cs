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
        [Header("CHARACTER NAME")]
        public string characterName;

        [Header("TIME PLAYED")]
        public float secondsPlayed;

        [Header("WORLD COORDINATES")]
        public float xPos;
        public float yPos;
        public float zPos;
    }
}
