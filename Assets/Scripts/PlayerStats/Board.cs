using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    /**
     * Everything that is classified as HiddenStat gets it's default value set by DefaultPlayerStats.
     * This means all HiddenStats are offsets from that default value. (Except AnimationCurves)
     */

    public Enums.Board board;

    /// <summary>
    /// Contains Movement variables that are not level dependent
    /// </summary>
    [System.Serializable]
    public class MovementVars
    {
        /***
         * All Stats in this category are based of the gear stats breakdown in the SRDX Datasheets
         */
        /// <summary>
        /// Determines the players usual cruising speed. This value is additive with the Character Based TopSpeed Value.
        /// Also stacks additively with TopSpeed Bonus from LevelUp.
        /// </summary>
        [Header("SpeedStats")]
        public int TopSpeed;
        /// <summary>
        /// SRDX
        /// The Speed to which the player boosts. MaxSpeed is set to boost speed while boosting.
        /// Stat changes with Level. Stacks additively with <see cref="Character.CharacterStatsMovement.BoostSpeed"/> from <see cref="Character"/>
        /// </summary>
        public int BoostSpeedLvl1;
        public int BoostSpeedLvl2;
        public int BoostSpeedLvl3;
        /// <summary>
        /// SRDX
        /// The percentage Modifier used when performing a BoostChain.
        /// Value Additively stacks with the character Based BoostChainModifier.
        /// </summary>
        public float BoostChainModifier;
        /// <summary>
        /// SRDX
        /// Speed player gains from a DriftDash. This amount stacks ontop of the current speed (as long as it's below the DriftCap).
        /// Stat is affected by Level and additively stacks with the Drift stat from character.
        /// </summary>
        public int DriftDashSpeedLvl1;
        public int DriftDashSpeedLvl2;
        public int DriftDashSpeedLvl3;
        /// <summary>
        /// SRDX
        /// Player can not you DriftDash to Boost past this speed, but also can not lose Speed by boosting while above cap.
        /// Additively stacks with the Drift Value from character and the values from LevelUps. (+10 per level)
        /// </summary>
        public int DriftCap;
        /// <summary>
        /// SRDX
        /// Minimum duration the player needs to be drifting for to gain a DriftDash upon releasing the drift. Measured in seconds.
        /// Stat is originally called DriftDashFrames and is orginally measured in Frames.
        /// </summary>
        public int DriftDashChargeDuration;

        /**
         * Other necesary stats that are board based, but not listed in the SRDX Datasheets
         */
        
        [Header("Acceleration")]
        /// <summary>
        /// HiddenStat
        /// Threshold for FastAccel
        /// </summary>
        public int MinSpeed;
        /// <summary>
        /// HiddenStat
        /// The rate at which the player accelerates while below their MinSpeed value
        /// </summary>
        public int FastAcceleration;

        [Header("Turning")]
        /// <summary>
        /// HiddenStat
        /// This is proabaly in the og game but kinda cryptic.
        /// Base Number affecting how fast the Board turns.
        /// </summary>
        public float Turnrate;
        /// <summary>
        /// HiddenStat
        /// Curve determening the speed lost while turning, based of the current speed
        /// </summary>
        public AnimationCurve TurnSpeedLossCurve;
        /// <summary>
        /// HiddenStat
        /// Determines the ratio of Turning relative to current speed.
        /// This stat is not based of the og game afaik
        /// </summary>
        public AnimationCurve TurnrateCurve;

        /// <summary>
        /// HiddenStat
        /// Rate at which the player passively turns towards a direction while drifting
        /// </summary>
        [Header("Drift")]
        public float DriftTurnratePassive;
        /// <summary>
        /// HiddenStat
        /// Minimum amount the player is able to turn while drifting
        /// </summary>
        public float DriftTurnrateMin;
        /// <summary>
        /// HiddenStat
        /// The turnrate used while drifting (if the player inputs a direction)
        /// </summary>
        public float DriftTurnrate;

        /// <summary>
        /// HiddenStat
        /// Rate of deceleration while breakeing
        /// </summary>
        [Header("Breake")]
        public float BreakeDeceleration;


        /// <summary>
        /// HiddenStat
        /// The maximum amount of speed the player hits while jumping
        /// </summary>
        [Header("Jump")]
        public float JumpSpeedMax;          // Controls jump speed relative to time
        /// <summary>
        /// HiddenStat
        /// Controls the acceleration behaviour while jumping
        /// </summary>
        public AnimationCurve JumpAccel;    // Acceleration for a jump
        /// <summary>
        /// HiddenStat
        /// The value the player the decelerates towards while charging a jump
        /// </summary>
        public float JumpChargeMinSpeed;
        /// <summary>
        /// HiddenStat
        /// Rate of deceleration while charging jump
        /// </summary>
        public float JumpChargeDeceleration;

        /// <summary>
        /// HiddenStat
        /// Acceleration whith which the player falls while affected by gravity
        /// </summary>
        [Header("Gravity")]
        public float GravityMultiplier;

        /// <summary>
        /// HiddenStat
        /// Duration for which the backwards movement while bumping of a wall lasts
        /// </summary>
        [Header("WallBump")]
        public float wallBumpTimer;
        /// <summary>
        /// HiddenStat
        /// Speedmultiplier for bumping of walls
        /// </summary>
        public float wallBumpSpeed;

        [Header("Air")]
        public float AirGainTrick;
        public float AirGainShortcut;
        public float AirGainAutorotate;
        public float JumpAirLoss;
    }

    /// <summary>
    /// Contains Level dependent stats
    /// </summary>
    [System.Serializable]
    public class ExtremeGearLevelStats
    {
        [Header("Air")]
        public int MaxAir;
        public int PassiveAirDrain;
        public int DriftAirCost;
        public int BoostCost;
        public int TornadoCost;
        [Header("Speed&Boost")]
        public float SpeedGainedFromDriftDash;
        public float BoostSpeed;
    }

    public MovementVars movementVars = new MovementVars();
    public ExtremeGearLevelStats GearStatsLevel1 = new ExtremeGearLevelStats();
    public ExtremeGearLevelStats GearStatsLevel2 = new ExtremeGearLevelStats();
    public ExtremeGearLevelStats GearStatsLevel3 = new ExtremeGearLevelStats();
}
