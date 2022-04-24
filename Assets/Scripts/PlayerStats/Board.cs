using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
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
        /// </summary>
        [Header("SpeedStats")]
        public int TopSpeed;
        /// <summary>
        /// 
        /// </summary>
        public int BoostSpeedLvl1;
        public int BoostSpeedLvl2;
        public int BoostSpeedLvl3;
        public float BoostChainModifier;
        public int DriftDashSpeedLvl1;
        public int DriftDashSpeedLvl2;
        public int DriftDashSpeedLvl3;
        public int DriftCap;
        public int DriftDashChargeDuration;

        [Header("Turning")]
        /// <summary>
        /// Currently Unused
        /// </summary>
        public float TurnAcceleration;
        /// <summary>
        /// Base Number affecting how fast the Board turns
        /// </summary>
        public float Turnrate;
        /// <summary>
        /// Affects the amount of speed lost while turning. Currently unused.
        /// </summary>
        public float TurnSpeedLoss;
        /// <summary>
        /// Currently unused
        /// </summary>
        public float SpeedHandlingMultiplier;
        /// <summary>
        /// Currently unused
        /// </summary>
        public float TurnLowSpeedHandlingMultiplier;
        /// <summary>
        /// Curve determening the speed lost while turning, based of the current speed
        /// </summary>
        public AnimationCurve TurnSpeedLossCurve;
        public AnimationCurve TurnrateCurve;

        /// <summary>
        /// SRDX
        /// Minimum duration the player needs to be drifting for to gain a DriftDash upon releasing the drift. Measured in seconds.
        /// Stat is originally called DriftDashFrames and is orginally measured in Frames.
        /// </summary>
        [Header("Drift")]
        public float DriftDashChargeDuration;
        // public float DriftBoostSpeed;    LVL_AFFECTED
        public float DriftTurnratePassive;      // Stat usually filled by GlobalStats
        public float DriftTurnrateMin;
        public float DriftTurnrate;

        [Header("Boost")]
        public float BoostDuration;
        public float BoostLockTime;

        [Header("Breake")]
        public float BreakeDeceleration;

        [Header("Jump")]
        public float jumpSpeedMax;          // Controls jump speed relative to time
        public AnimationCurve jumpAccel;    // Acceleration for a jump
        public float jumpChargeMinSpeed;
        public float jumpChargeDeceleration;

        [Header("Gravity")]
        public float gravityMultiplier;

        [Header("WallBump")]
        public float wallBumpTimer;
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
