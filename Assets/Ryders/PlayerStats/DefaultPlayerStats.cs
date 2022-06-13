using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryders.Core.Player
{
    /// <summary>
    /// Contains Default Values for all stats that are not usually affected by Character or Board.
    /// </summary>
    [CreateAssetMenu(fileName = "DefaultPlayerStats", menuName = "ScriptableObjects/DefaultPlayerStats", order = 1)]
    public class DefaultPlayerStats : ScriptableObject
    {

        /// <summary>
        /// Acceleration at low speeds. This is a total value.
        /// </summary>
        [Header("Acceleration")]
        public float AccelerationLowLvl1;
        /// <summary>
        /// Acceleration at low speeds. This is a total value.
        /// </summary>
        public float AccelerationLowLvl2;
        /// <summary>
        /// Acceleration at low speeds. This is a total value.
        /// </summary>
        public float AccelerationLowLvl3;
        /// <summary>
        /// Acceleration at medium speeds. This is a total value.
        /// </summary>
        public float AccelerationMediumLvl1;
        /// <summary>
        /// Acceleration at medium speeds. This is a total value.
        /// </summary>
        public float AccelerationMediumLvl2;
        /// <summary>
        /// Acceleration at medium speeds. This is a total value.
        /// </summary>
        public float AccelerationMediumLvl3;
        /// <summary>
        /// Acceleration at high speeds. This is a total value.
        /// </summary>
        public float AccelerationHighLvl1;
        /// <summary>
        /// Acceleration at high speeds. This is a total value.
        /// </summary>
        public float AccelerationHighLvl2;
        /// <summary>
        /// Acceleration at high speeds. This is a total value.
        /// </summary>
        public float AccelerationHighLvl3;
        /// <summary>
        /// Threshold below which AccelerationLow is used
        /// </summary>
        public float AccelerationLowThresholdLvl1;
        /// <summary>
        /// Threshold below which AccelerationLow is used
        /// </summary>
        public float AccelerationLowThresholdLvl2;
        /// <summary>
        /// Threshold below which AccelerationLow is used
        /// </summary>
        public float AccelerationLowThresholdLvl3;
        /// <summary>
        /// Threshold below which AccelerationMedium is used
        /// </summary>
        public float AccelerationMediumThresholdLvl1;
        /// <summary>
        /// Threshold below which AccelerationMedium is used
        /// </summary>
        public float AccelerationMediumThresholdLvl2;
        /// <summary>
        /// Threshold below which AccelerationMedium is used
        /// </summary>
        public float AccelerationMediumThresholdLvl3;
        /// <summary>
        /// Threshold for OffRoad
        /// </summary>
        public float AccelerationOffRoadThresholdLvl1;
        /// <summary>
        /// Threshold for OffRoad
        /// </summary>
        public float AccelerationOffRoadThresholdLvl2;
        /// <summary>
        /// Threshold for OffRoad
        /// </summary>
        public float AccelerationOffRoadThresholdLvl3;
        /// <summary>
        /// Base Value for speed loss while cornering.
        /// </summary>
        public float CorneringDeceleration;

        /// <summary>
        /// HiddenStat
        /// This is proabaly in the og game but kinda cryptic.
        /// Base Number affecting how fast the Board turns.
        /// </summary>
        [Header("Turning")]
        public float TurnrateDefault;
        /// <summary>
        /// Maximum TurnRate that can be reached
        /// Based of TurnMaxRadius in Sewer56.SonicRiders
        /// </summary>
        public float TurnRateMax;
        /// <summary>
        /// HiddenStat
        /// Curve determening the speed lost while turning, based of the current speed
        /// </summary>
        public AnimationCurve TurnSpeedLossCurveDefault;
        /// <summary>
        /// HiddenStat
        /// Determines the ratio of Turning relative to current speed.
        /// This stat is not based of the og game afaik
        /// </summary>
        public AnimationCurve TurnrateCurveDefault;
        
        /// <summary>
        /// HiddenStat
        /// Minimum amount the player is able to turn while drifting
        /// </summary>
        [Header("Drift")]
        public float DriftTurnrateMinDefault;
        /// <summary>
        /// Maximum amount the player is able to turn while drifting
        /// </summary>
        public float DriftTurnRateMaxDefault;
        /// <summary>
        /// HiddenStat
        /// The turnrate used while drifting (if the player inputs a direction)
        /// </summary>
        public float DriftTurnrateDefault;

        /// <summary>
        /// HiddenStat
        /// Rate of deceleration while breakeing
        /// </summary>
        [Header("Breake")]
        public float BreakeDecelerationDefault;


        /// <summary>
        /// HiddenStat
        /// The maximum amount of speed the player hits while jumping
        /// </summary>
        [Header("Jump")]
        public float JumpSpeedMaxDefault;          // Controls jump speed relative to time
        /// <summary>
        /// HiddenStat
        /// Controls the acceleration behaviour while jumping
        /// </summary>
        public AnimationCurve JumpAccelDefault;    // Acceleration for a jump
        /// <summary>
        /// HiddenStat
        /// The value the player the decelerates towards while charging a jump
        /// </summary>
        public float JumpChargeMinSpeedDefault;
        /// <summary>
        /// HiddenStat
        /// Rate of deceleration while charging jump
        /// </summary>
        public float JumpChargeDecelerationDefault;

        /// <summary>
        /// HiddenStat
        /// Acceleration whith which the player falls while affected by gravity
        /// </summary>
        [Header("Gravity")]
        public float GravityMultiplierDefault;

        /// <summary>
        /// HiddenStat
        /// Duration for which the backwards movement while bumping of a wall lasts
        /// </summary>
        [Header("WallBump")]
        public float WallBumpTimerDefault;
        /// <summary>
        /// HiddenStat
        /// Speedmultiplier for bumping of walls
        /// </summary>
        public float WallBumpSpeedDefault;


        /// <summary>
        /// DefaultStat
        /// Default change to Top speed on LevelChange for each player regardless of Gear or Character.
        /// Change is applied additively
        /// </summary>
        [Header("LevelUpChanges")]
        public int TopSpeedLevelUp;
        /// <summary>
        /// DefaultStat
        /// Default change to DriftCap on LevelChange for each player regardless of Gear or Character
        /// Change is applied additively
        /// </summary>
        public int DriftCapLevelUp;
        /// <summary>
        /// DefaultStat
        /// Default change to BoostDuration on LevelChange for each player regardless of Gear or Character
        /// Change is applied additively
        /// </summary>
        public int BoostDuration;
        /// <summary>
        /// DefaultStat
        /// Default change to RunSpeed on LevelChange for each plaer regardless of Gear or Character
        /// Change is applied additively
        /// </summary>
        public int RunSpeed;
    }
}
