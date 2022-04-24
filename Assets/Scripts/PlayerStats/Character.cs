using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

/// <summary>
/// This script saves all values that are affected by a character. The type of stats is based of a mix of Sonic Riders Vanilla and SRDX.
/// For more info see the Sonic Riders Competetive Datasheets as well as Sewer56.SonicRiders.
/// SRDX Datasheets: https://docs.google.com/spreadsheets/d/10zc6cBGf1-RNXLro5z_gw3DSK366zD3L2bSJ4JhSJ60/edit#gid=1406646950
/// </summary>
public class Character : MonoBehaviour
{
    public Enums.Character character;
    public GameObject model;

    [System.Serializable]
    public class CharacterStatsMovement
    {
        public CharacterType ShortCutType;
        public CharacterType StatsType;
        public CharacterClass CharacterClass;

        public float SpeedMultiplier;

        /// <summary>
        /// CURRENTLY WIP
        /// Determines the Players default MaxSpeed value. (Usually this will equal the players cruising speed).
        /// Scales with Level and is additive.
        /// </summary>
        [Header("TopSpeed")]
        public int TopSpeedLvl1;
        public int TopSpeedLvl2;
        public int TopSpeedLvl3;

        /// <summary>
        /// CURRENTLY WIP
        /// Sewer56.SonicRiders
        /// Determines the players base accel.
        /// </summary>
        [Header("Acceleration")]
        public int AccelerationLvl1;
        public int AccelerationLvl2;
        public int AccelerationLvl3;

        /// <summary>
        /// CURRENTLY WIP
        /// Sewer56.SonicRiders
        /// Determines the players Cornering ability.
        /// </summary>
        [Header("Cornering")]
        public int CorneringLvl1;
        public int CorneringLvl2;
        public int CorneringLvl3;

        /// <summary>
        /// CURRENTLY WIP
        /// Sewer56.SonicRiders
        /// Determines how much the player is affected by going Offroad. Exact workings are kinda unclear and Offroad is currently unimplemented.
        /// </summary>
        [Header("Offroad")]
        public int OffroadLvl1;
        public int OffroadLvl2;
        public int OffroadLvl3;

        /// <summary>
        /// SRDX
        /// Character Based Boost Duration. Scales with Level and additively affects the Players BoostDuration.
        /// Value is orginally measured in frames. Due to variable framerate support value is measured in seconds here.
        /// </summary>
        [Header("BoostDuration")]
        public float BoostDurationLvl1;
        public float BoostDurationLvl2;
        public float BoostDurationLvl3;

        /// <summary>
        /// SRDX
        /// Character Based BoostStat. Additively affects the players max boost speed.
        /// </summary>
        [Header("BoostSpeed")]
        public float BoostSpeed;

        /// <summary>
        /// SRDX
        /// Character based BoostChainModifier. Value is additive and a percentage.
        /// Only relevant for BoostChains. Boost Chains occur when you Boost while drifting.
        /// </summary>
        [Header("BoostChainModifier")]
        public float BoostChainModifier;

        /// <summary>
        /// SRDX
        /// Character based Drift stat. Additively affects DriftDashSpeed and DriftCap.
        /// DriftDashSpeed is the actual amount of speed you gain from a DriftDash, while DriftCap is your DriftDash speed Limit (i.e you can not boost past this value=
        /// For more info see the SRDX Term Glossary.
        /// </summary>
        [Header("Drift")]
        public float Drift;

        /// <summary>
        /// SRDX
        /// Character Based modifier for TopSpeed. Value is additive.
        /// </summary>
        [Header("TopSpeed")]
        public float TopSpeed;
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
