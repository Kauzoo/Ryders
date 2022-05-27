using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ryders.Core.Player.Character;

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
        public SpeedStats speedStats = new SpeedStats();

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
        public TurnStats turnStats = new TurnStats();

        [System.Serializable]
        public class JumpStats
        {
            public float JumpSpeedMax;
            public AnimationCurve JumpAccelleration;
        }
        public JumpStats jumpStats = new JumpStats();

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
        public FuelStats fuelStats = new FuelStats();
        #endregion

        #region ChangableVars
        /**
         * CHANGABLE VARS
         */

        [System.Serializable]
        public class Movement
        {
            public enum TranslationStates
            {
                Stationary, LowSpeed, HighSpeed
            }
            public enum CorneringStates
            {
                None, Turning, DriftingL, DriftingR, Breaking
            }
            public enum JumpStates
            {
                None, JumpCharging, Jumping, JumpLanding
            }
            public enum BoostStates
            {
                None, Boost, Boosting, BoostLock
            }

            [Header("Speed")]
            public float Speed;
            public float MaxSpeed;

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
            public float JumpSpeed;
            public float JumpAccelleration;

            [Header("Gravity")]
            public float Gravity;

            [Header("MovementStates")]
            public bool Grounded;
            public TranslationStates TranslationState = TranslationStates.Stationary;
            public CorneringStates CorneringState = CorneringStates.None;
            public JumpStates JumpState = JumpStates.None;
            public BoostStates BoostState = BoostStates.None;
        }
        public Movement movement = new Movement();

        [System.Serializable]
        public class Fuel
        {
            public float CurrentFuel;
            public int Level;
            public int Rings;
        }
        public Fuel fuel = new Fuel();
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
            Vector3 fowardVector = new Vector3(0, 0, 1);
            Vector3 gravityVector = new Vector3(0, 0, 0);
            Vector3 jumpVector = new Vector3(0, 0, 0);
            Quaternion orientation = playerTransform.rotation;
            MasterMove(orientation, fowardVector, gravityVector, jumpVector); 
        }

        #region UtilityMethods
        public virtual void LoadStats(int level)
        {
            LoadTopSpeed(level);
            LoadBoostSpeed(level);
            LoadBoostChainModifier();
            LoadDriftDashSpeed(level);
            LoadDriftCap(level);
            LoadDrifDashFrames();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void LoadTopSpeed(int level)
        {
            speedStats.TopSpeed = (DefaultPlayerStats.TopSpeedLevelUp * level) + CharacterData.TopSpeed + ExtremeGearData.movementVars.TopSpeed;
        }

        public virtual void LoadMinSpeed()
        {
            speedStats.MinSpeed = DefaultPlayerStats.MinSpeedDefault;
        }

        public virtual void LoadFastAccelleration()
        {
            speedStats.FastAccelleration = DefaultPlayerStats.FastAccelerationDefault;
        }

        /// <summary>
        /// Affected by Level (ExtremeGear).
        /// BoostSpeed = CharacterData.BoostSpeed + ExtremeGearData.BoostSpeedLvl
        /// <see cref="PlayerBehaviour.BoostSpeed"/>
        /// </summary>
        /// <param name="level"></param>
        public virtual void LoadBoostSpeed(int level)
        {
            switch(level)
            {
                case 1:
                    speedStats.BoostSpeed = CharacterData.BoostSpeed + ExtremeGearData.movementVars.BoostSpeedLvl1;
                    break;
                case 2:
                    speedStats.BoostSpeed = CharacterData.BoostSpeed + ExtremeGearData.movementVars.BoostSpeedLvl2;
                    break;
                case 3:
                    speedStats.BoostSpeed = CharacterData.BoostSpeed + ExtremeGearData.movementVars.BoostSpeedLvl3;
                    break;
                default:
                    throw new System.NotImplementedException("Invalid Level");
            }
            
        }

        /// <summary>
        /// BoostChainModifier = CharacterData.BoostChainModifier + ExtremeGearData.BoostChainModifier
        /// </summary>
        public virtual void LoadBoostChainModifier()
        {
            speedStats.BoostChainModifier = CharacterData.BoostChainModifier + ExtremeGearData.movementVars.BoostChainModifier;
        }

        /// <summary>
        /// Affected by Level (ExtremeGear).
        /// DriftDashSpeed = CharacterData.Drift + ExtremeGearData.DriftDashSpeedLvl;
        /// </summary>
        /// <param name="level"></param>
        public virtual void LoadDriftDashSpeed(int level)
        {
            switch(level)
            {
                case 1:
                    speedStats.DriftDashSpeed = CharacterData.Drift + ExtremeGearData.movementVars.DriftDashSpeedLvl1;
                    break;
                case 2:
                    speedStats.DriftDashSpeed = CharacterData.Drift + ExtremeGearData.movementVars.DriftDashSpeedLvl2;
                    break;
                case 3:
                    speedStats.DriftDashSpeed = CharacterData.Drift + ExtremeGearData.movementVars.DriftDashSpeedLvl3;
                    break;
                default:
                    throw new System.NotImplementedException("Invalid Level");
            }
            
        }

        public virtual void LoadDriftCap(int level)
        {
            speedStats.DriftCap = (DefaultPlayerStats.DriftCapLevelUp * level) + CharacterData.Drift + ExtremeGearData.movementVars.DriftCap;
        }

        public virtual void LoadDrifDashFrames()
        {
            speedStats.DriftDashFrames = ExtremeGearData.movementVars.DriftDashChargeDuration;
        }

        public virtual void LoadTurnSpeedLoss()
        {
            speedStats.TurnSpeedLoss = DefaultPlayerStats.TurnSpeedLossCurveDefault;
        }

        public virtual void LoadJumpChargeMinSpeed()
        {
            speedStats.JumpChargeMinSpeed = DefaultPlayerStats.JumpChargeMinSpeedDefault;
        }

        public virtual void LoadJumpChargeDecelleration()
        {
            speedStats.JumpChargeDecelleration = DefaultPlayerStats.JumpChargeDecelerationDefault;
        }

        /**
         * TURNING
         */
        public virtual void LoadTurnrate()
        {
            turnStats.Turnrate = DefaultPlayerStats.TurnrateDefault;
        }

        public virtual void LoadTurnSpeedLossCurve()
        {
            turnStats.TurnSpeedLossCurve = DefaultPlayerStats.TurnSpeedLossCurveDefault;
        }

        public virtual void LoadTurnrateCurve()
        {
            turnStats.TurnrateCurve = DefaultPlayerStats.TurnrateCurveDefault;
        }

        public virtual void LoadDriftTurnratePassive()
        {
            turnStats.DriftTurnratePassive = DefaultPlayerStats.DriftTurnrateDefault;
        }

        public virtual void LoadDriftTurnrateMin()
        {
            turnStats.DriftTurnrateMin = DefaultPlayerStats.DriftTurnrateMinDefault;
        }

        public virtual void LoadDriftTurnrate()
        {
            turnStats.DriftTurnrate = DefaultPlayerStats.DriftTurnrateDefault;
        }
        #endregion

        #region 
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

        /// <summary>
        /// Calculate the speed value that is multiplied with the playerTransform forwardVector and then applied to tge playerRigidbody velocity
        /// </summary>
        public virtual void CalculateSpeed()
        {

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
