using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

namespace Player
{
    /// <summary>
    /// This class is the Basis for Characters.
    /// To implement a characters 
    /// This script saves all values that are affected by a character. The type of stats is based of a mix of Sonic Riders Vanilla and SRDX.
    /// For more info see the Sonic Riders Competetive Datasheets as well as Sewer56.SonicRiders.
    /// SRDX Datasheets: https://docs.google.com/spreadsheets/d/10zc6cBGf1-RNXLro5z_gw3DSK366zD3L2bSJ4JhSJ60/edit#gid=1406646950
    /// </summary>
    
    public interface CharacterBase
    {
        public CharacterType ShortCutType { get; set; }
        public CharacterType StatsType { get; set; }
        public CharacterClass CharacterClass { get; set; }

        /// <summary>
        /// SRDX
        /// Character Based Boost Duration. Scales with Level and additively affects the Players BoostDuration.
        /// Value is orginally measured in frames. Due to variable framerate support value is measured in seconds here.
        /// This additively stacks with the Flat additional boost frames from LevelUps (LevelUp Boost frames are seprrate from these value);
        /// </summary>
        public float BoostDurationLvl1 { get; set; }
        public float BoostDurationLvl2 { get; set; }
        public float BoostDurationLvl3 { get; set; }

        /// <summary>
        /// SRDX
        /// Character Based BoostStat. Additively affects the players max boost speed.
        /// </summary>
        public float BoostSpeed { get; set; }

        /// <summary>
        /// SRDX
        /// Character based BoostChainModifier. Value is additive and a percentage.
        /// Only relevant for BoostChains. Boost Chains occur when you Boost while drifting.
        /// </summary>
        public float BoostChainModifier { get; set; }

        /// <summary>
        /// SRDX
        /// Character based Drift stat. Additively affects DriftDashSpeed and DriftCap.
        /// DriftDashSpeed is the actual amount of speed you gain from a DriftDash, while DriftCap is your DriftDash speed Limit (i.e you can not boost past this value=
        /// For more info see the SRDX Term Glossary.
        /// </summary>
        public float Drift { get; set; }

        /// <summary>
        /// SRDX
        /// Character Based modifier for TopSpeed. Value is additive.
        /// </summary>
        public float TopSpeed { get; set; }

        #region Getters&Setters
        #endregion
    }
}
