using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

/// <summary>
/// This script saves all values that are affected by a character. The type of stats is based of a mix of Sonic Riders Vanilla and SRDX.
/// For more info see the Sonic Riders Competetive Datasheets as well as Sewer56.SonicRiders.
/// SRDX Datasheets: https://docs.google.com/spreadsheets/d/10zc6cBGf1-RNXLro5z_gw3DSK366zD3L2bSJ4JhSJ60/edit#gid=1406646950
/// </summary>
[CreateAssetMenu(fileName = "CharacterBase", menuName = "ScriptableObjects/CharacterBase", order = 1)]
public class CharacterBase : ScriptableObject
{
    public Enums.Character character;
    public GameObject model;

    [System.Serializable]
    public class CharacterStatsMovement
    {
        public CharacterType ShortCutType;
        public CharacterType StatsType;
        public CharacterClass CharacterClass;

        /// <summary>
        /// SRDX
        /// Character Based Boost Duration. Scales with Level and additively affects the Players BoostDuration.
        /// Value is orginally measured in frames. Due to variable framerate support value is measured in seconds here.
        /// This additively stacks with the Flat additional boost frames from LevelUps (LevelUp Boost frames are seprrate from these value);
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

    public virtual
}
