using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryders.Core.Player.Character
{
    public enum CharacterType
    {
        Speed, Power, Fly
    }

    public enum CharacterClass
    {
        LateBoost, TopSpeed, Combat, Drift, AllRound, Omni
    }

    /// <summary>
    /// This class is the Basis for Characters.
    /// To implement a characters 
    /// This script saves all values that are affected by a character. The type of stats is based of a mix of Sonic Riders Vanilla and SRDX.
    /// For more info see the Sonic Riders Competetive Datasheets as well as Sewer56.SonicRiders.
    /// SRDX Datasheets: https://docs.google.com/spreadsheets/d/10zc6cBGf1-RNXLro5z_gw3DSK366zD3L2bSJ4JhSJ60/edit#gid=1406646950
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData", order = 1)]
    public class CharacterData : ScriptableObject
    {
        /// /// <summary>
        /// SRDX
        /// Character Based Boost Duration. Scales with Level and additively affects the Players BoostDuration.
        /// Value is orginally measured in frames. Due to variable framerate support value is measured in seconds here.
        /// This additively stacks with the Flat additional boost frames from LevelUps (LevelUp Boost frames are seprrate from these value);
        /// </summary>
        public float BoostDurationLvl1;
        public float BoostDurationLvl2;
        public float BoostDurationLvl3;

        public CharacterType ShortCutType;
        public CharacterType StatsType;
        public CharacterClass CharacterClass;

        /// <summary>
        /// SRDX
        /// Character Based BoostStat. Additively affects the players max boost speed.
        /// </summary>
        public float BoostSpeed;

        /// <summary>
        /// SRDX
        /// Character based BoostChainModifier. Value is additive and a percentage.
        /// Only relevant for BoostChains. Boost Chains occur when you Boost while drifting.
        /// </summary>
        public float BoostChainModifier;

        /// <summary>
        /// SRDX
        /// Character based Drift stat. Additively affects DriftDashSpeed and DriftCap.
        /// DriftDashSpeed is the actual amount of speed you gain from a DriftDash, while DriftCap is your DriftDash speed Limit (i.e you can not boost past this value=
        /// For more info see the SRDX Term Glossary.
        /// </summary>
        public float Drift;

        /// <summary>
        /// SRDX
        /// Character Based modifier for TopSpeed. Value is additive.
        /// </summary>
        public float TopSpeed;
    }
}
