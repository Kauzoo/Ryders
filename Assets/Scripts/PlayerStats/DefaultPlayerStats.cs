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
        public int MinSpeed;
        /// <summary>
        /// Acceleration value while the player is in FastAcceleration state. This is a total value.
        /// </summary>
        public int FastAcceleration;
        /// <summary>
        /// Will probably be handled as a Function.
        /// The rate at which the player decelerates back to his current MaxSpeed (when above that value). This value scales with CurrentSpeed as well as current MaxSpeed.
        /// For more see the SRDX Datasheet. 
        /// </summary>
        public float Deceleration;
        /// <summary>
        /// Base Value for speed loss while cornering.
        /// </summary>
        public float corneringDeceleration;
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
        /// 
        /// </summary>
        [Header("Drift")]
        public float DriftDashChargeDuration;
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
}
