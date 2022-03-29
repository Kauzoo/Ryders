using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [System.Serializable]
    public class CharacterStatsMovement
    {
        [Header("TopSpeed")]
        public float TopSpeedLvl1;

        [Header("Acceleration")]
        public float AccelerationLvl1;

        [Header("Cornering")]
        public float CorneringLvl1;

        [Header("Offroad")]
        public float OffroadLvl1;
    }

    public class CharacterStatsVanity
    {
        public float Height;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
