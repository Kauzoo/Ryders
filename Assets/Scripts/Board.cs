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
        [Header("LinearMovement")]
        public int MinSpeed;      // Threshold to trigger fast accel
        public int FastAcceleration;      // Value for fast accel
        // public float Acceleration;   value can be found on character
        // public float CruisingSpeed;  value can be found on character
        // public float BoostSpeed;     value can be found in LevlStats
        public float deceleration;
        public float corneringDeceleration;
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
        [Header("Drift")]
        public float DriftDurationMinimum;
        // public float DriftBoostSpeed;    LVL_AFFECTED
        public float DriftTurnratePassive;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
