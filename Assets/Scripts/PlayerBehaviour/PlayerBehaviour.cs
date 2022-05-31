using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ryders.Core.Player.Character;
using Ryders.Core.Player.ExtremeGear.Movement;

namespace Ryders.Core.Player.ExtremeGear
{
    /// <summary>
    /// This class represents the default case for an ExtremeGear.
    /// All movement is calculated here. The character only contributes stats, not custom code.
    /// Stats for the board are retrieved from the corresponding ExtemeGear Object
    /// </summary>
    public class PlayerBehaviour : MonoBehaviour
    {
        // Class that handles all thing input
        public GameObject InputModule;
        // Contains basic character data
        public CharacterData CharacterData;
        // Contains basic player stats
        public DefaultPlayerStats DefaultPlayerStats;
        // Contains basic extreme gear data
        public ExtremeGearData ExtremeGearData;

        public Transform playerTransform;
        public Rigidbody playerRigidbody;


        #region StaticVars
        /**
         * STATIC VARS
         * These values only change on level change
         */
        // SPEED STATS
        [System.Serializable]
        public class SpeedStats
        {
            [Header("Speed")]
            public float TopSpeed;
            [Header("Accelleration")]
            public float MinSpeed;
            public float Acceleration;
            public float FastAccelleration;
            /// <summary>
            /// Affected by Level (<see cref="ExtremeGear.ExtremeGearData"/>).
            /// BoostSpeed = CharacterData.BoostSpeed + ExtremeGearData.BoostSpeedLvl
            /// From SRDX Datasheets
            /// </summary>
            [Header("Boost")]
            public float BoostSpeed;
            /// <summary>
            /// BoostChainModifier = CharacterData.BoostChainModifier + ExtremeGearData.BoostChainModifier
            /// From SRDX Datasheets
            /// </summary>
            public float BoostChainModifier;
            [Header("Breake")]
            public float BreakeDecelleration;
            [Header("Drift")]
            public float DriftDashSpeed;
            public float DriftCap;
            public float DriftDashFrames;
            [Header("Turning")]
            public AnimationCurve TurnSpeedLoss;
            [Header("Jump")]
            public float JumpChargeMinSpeed;
            public float JumpChargeDecelleration;
        }
        public SpeedStats speedStats = new();

        public IAccelerationPack<PlayerBehaviour> accelPack = new TestAccel<PlayerBehaviour>();

        // TURN STATS
        /// <summary>
        /// All the stats that affect the calculations for rotation (arround local y-axis)
        /// Contains stats for regular Turning as well as Drifting
        /// </summary>
        [System.Serializable]
        public class TurnStats
        {
            [Header("Turning")]
            public float Turnrate;
            public AnimationCurve TurnSpeedLossCurve;
            public AnimationCurve TurnrateCurve;
            [Header("Drift")]
            public float DriftTurnratePassive;
            public float DriftTurnrateMin;
            public float DriftTurnrate;
        }
        public TurnStats turnStats = new();

        [System.Serializable]
        public class JumpStats
        {
            public float JumpSpeedMax;
            public AnimationCurve JumpAccelleration;
        }
        public JumpStats jumpStats = new();

        // FUEL STATS
        [System.Serializable]
        public class FuelStats
        {
            [Header("Fuel Stats")]
            public FuelType FuelType;
            public float JumpChargeMultiplier;
            public float TrickFuelGain;
            public float TypeFuelGain;
            public float QTEFuelGain;
            public float PassiveDrain;
            public float TankSize;
            public float BoostCost;
            public float DriftCost;
            public float TorandoCost;
        }
        public FuelStats fuelStats = new();
        #endregion

        #region ChangableVars
        /**
         * CHANGABLE VARS
         */

        [System.Serializable]
        public class Movement
        {
            [Header("Speed")]
            public float Speed;
            public float MaxSpeed;

            [Header("Acceleration")] 
            public float Acceleration;  // The sum of all acceleration effects
            public float Deceleration;  // The sum of all decelerations

            [Header("Boost")]
            public float BoostTimer;
            public float BoostLockTimer;

            [Header("Turning")]
            public float Rotation;
            public float TurningRaw;
            public float Turning;

            [Header("Drift")]
            public float DriftTurning;
            public float DriftTimer;

            [Header("Jump")]
            public float JumpChargeDuration;
            public float JumpCharge;
            public float JumpSpeed;
            public float JumpAccelleration;

            [Header("Gravity")]
            public float Gravity;

            [Header("MovementStates")]
            public bool Grounded;
            public TranslationStates TranslationState = TranslationStates.Stationary;
            public DriftStates DriftState = DriftStates.None;
            public JumpStates JumpState = JumpStates.None;
            public CorneringStates CorneringState = CorneringStates.None;
            public SpeedStates SpeedState = SpeedStates.LowSpeed;
            public GroundedStates GroundedState = GroundedStates.None;
        }
        public Movement movement = new();

        [System.Serializable]
        public class Fuel
        {
            public float CurrentFuel;
            public int Level;
            public int Rings;
        }
        [Tooltip("Contains info about fuel")] public Fuel fuel = new();
        #endregion

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        // Write everything that is supposed to be in Update in here
        public virtual void UpdateBase()
        {
            LoadStats(fuel.Level);
        }

        void FixedUpdate()
        {

        }

        public virtual void FixedUpdateBase()
        {
            Vector3 fowardVector = new(0, 0, 1);
            Vector3 gravityVector = new(0, 0, 0);
            Vector3 jumpVector = new(0, 0, 0);
            Quaternion orientation = playerTransform.rotation;
            MasterMove(orientation, fowardVector, gravityVector, jumpVector); 
        }
        
        #region StateDetermination
        public virtual void DetermineStates()
        {

        }
        #endregion

        #region Movement
        /// <summary>
        /// MasterMethod for Movement. Orientation and Position are updated here.
        /// </summary>
        public virtual void MasterMove(Quaternion orientation, Vector3 forwardVector, Vector3 gravityVector, Vector3 jumpVector)
        {
            playerRigidbody.rotation = orientation;
            playerRigidbody.velocity = forwardVector + gravityVector + jumpVector;
        }

        public virtual void MasterAcceleration()
        {
            
        }

        /// <summary>
        /// Calculate the speed value that is multiplied with the playerTransform forwardVector and then applied to tge playerRigidbody velocity
        /// This method takes Accel and Decel into account
        /// </summary>
        public virtual void CalculateSpeed()
        {
            
        }

        /// <summary>
        /// Determine the current value for MaxSpeed, based of the TranslationState
        /// </summary>
        public virtual float DetermineMaxSpeed()
        {
            return movement.TranslationState == TranslationStates.Boosting ? speedStats.BoostSpeed : speedStats.TopSpeed;
        }

        /// <summary>
        /// Calculate the rotation Quaternion that is applied to the playerRigidbody
        /// </summary>
        public virtual void CalculateOrientation()
        {
            
        }

        public virtual void CalculateJumpVector()
        {

        }

        public virtual void CalculateGravityVector()
        {

        }

        public virtual void CalculateFuel()
        {

        }

        public virtual void Drift()
        {

        }

        public virtual void Turn()
        {

        }

        public virtual void Move()
        {

        }

        public virtual void Accelerate()
        {

        }

        public virtual void Breake()
        {

        }

        public virtual void Jump()
        {

        }
        #endregion

        #region Level
        public virtual void LevelChange(int Level)
        {

        }

        public virtual void LevelUp()
        {

        }

        public virtual void LevelDown()
        {

        }
        #endregion

    }
}

namespace Ryders.Core.Player.ExtremeGear.Movement
{
    public enum TranslationStates
    {
        Stationary, Cruising, Boosting
    }
    public enum SpeedStates
    {
        LowSpeed, HighSpeed
    }
    public enum DriftStates
    {
        None, DriftingL, DriftingR
    }
    public enum JumpStates
    {
        None, JumpCharging, Jumping, JumpLanding
    }
    public enum CorneringStates
    {
        None, TurningL, TurningR
    }
    public enum GroundedStates
    {
        None, Grounded
    }
}