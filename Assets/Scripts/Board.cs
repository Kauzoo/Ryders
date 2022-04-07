using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [System.Serializable]
    public class MovementVars
    {
        [Header("LinearMovement")]
        public float MinSpeed;
        public float FastAcceleration;
        public float Acceleration;
        public float CruisingSpeed;
        public float BoostSpeed;
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
        [Header("Drift")]
        public float DriftDurationMinimum;
        public float DriftBoostSpeed;
        public float DriftTurnratePassive;
        public float DriftTurnrateMin;
        public float DriftTurnrate;
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

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
