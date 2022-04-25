using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerStats
{
    /// <summary>
    /// Contains Default Values for all stats that are not usually affected by Character or Board.
    /// </summary>
    public class DefaultPlayerStats : MonoBehaviour
    {

        /// <summary>
        /// Speed Threshold below which FastAcceleration is triggered. This is a total value.
        /// </summary>
        [Header("Acceleration")]
        public int MinSpeedDefault;
        /// <summary>
        /// Acceleration value while the player is in FastAcceleration state. This is a total value.
        /// </summary>
        public int FastAccelerationDefault;
        /// <summary>
        /// Will probably be handled as a Function.
        /// The rate at which the player decelerates back to his current MaxSpeed (when above that value). This value scales with CurrentSpeed as well as current MaxSpeed.
        /// For more see the SRDX Datasheet. 
        /// </summary>
        public float Deceleration;
        /// <summary>
        /// Base Value for speed loss while cornering.
        /// </summary>
        public float CorneringDeceleration;
        /// <summary>
        /// Default formula to calculate Deceleration.
        /// First value is current Speed, second value is current MaxSpeed. Returns a value for Deceleration >= 0.
        /// For more info on the formula see SRDX Datasheets.
        /// </summary>
        public Func<float, float, float> DecelerationFormula = (Speed, MaxSpeed) => {
            float OvermaxSpeed = Speed - MaxSpeed;
            float Deceleration = 0;
            if(OvermaxSpeed > 0)
            {
                if(MaxSpeed > 200)
                {
                    Deceleration = (Mathf.Pow((OvermaxSpeed / 60), 2.0f) + 0.2f) / 1000;
                }
                if(MaxSpeed < 200)
                {
                    Deceleration = (Mathf.Pow((OvermaxSpeed / (260 - MaxSpeed)), 2.0f) + 0.2f) / 1000;
                }
            }
            return Deceleration;
    };

        [Header("Turning")]
        /// <summary>
        /// HiddenStat
        /// This is proabaly in the og game but kinda cryptic.
        /// Base Number affecting how fast the Board turns.
        /// </summary>
        public float TurnrateDefault;
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
        /// Rate at which the player passively turns towards a direction while drifting
        /// </summary>
        [Header("Drift")]
        public float DriftTurnratePassiveDefault;
        /// <summary>
        /// HiddenStat
        /// Minimum amount the player is able to turn while drifting
        /// </summary>
        public float DriftTurnrateMinDefault;
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
