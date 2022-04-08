using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Character : MonoBehaviour
{
    public Enums.Character character;
    public GameObject model;

    [System.Serializable]
    public class CharacterStatsMovement
    {
        public CharacterType ShortCutType;
        public CharacterType StatsType;

        public float SpeedMultiplier;

        [Header("TopSpeed")]
        public int TopSpeedLvl1;
        public int TopSpeedLvl2;
        public int TopSpeedLvl3;


        [Header("Acceleration")]
        public int AccelerationLvl1;
        public int AccelerationLvl2;
        public int AccelerationLvl3;

        [Header("Cornering")]
        public int CorneringLvl1;
        public int CorneringLvl2;
        public int CorneringLvl3;


        [Header("Offroad")]
        public int OffroadLvl1;
        public int OffroadLvl2;
        public int OffroadLvl3;
    }

    [System.Serializable]
    public class CharacterStatsVanity
    {
        public float Height;
        public bool Gender;
    }

    public CharacterStatsMovement characterStatsMovement = new CharacterStatsMovement();
    public CharacterStatsVanity characterStatsVanity = new CharacterStatsVanity();
}
