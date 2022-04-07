using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [System.Serializable]
    public class CharacterStatsMovement
    {
        public float SpeedMultiplier;

        [Header("TopSpeed")]
        public float TopSpeedLvl1;
        public float TopSpeedLvl2;
        public float TopSpeedLvl3;


        [Header("Acceleration")]
        public float AccelerationLvl1;
        public float AccelerationLvl2;
        public float AccelerationLvl3;

        [Header("Cornering")]
        public float CorneringLvl1;
        public float CorneringLvl2;
        public float CorneringLvl3;


        [Header("Offroad")]
        public float OffroadLvl1;
        public float OffroadLvl2;
        public float OffroadLvl3;
    }

    [System.Serializable]
    public class CharacterStatsVanity
    {
        public float Height;
        public bool Gender;
    }
}
