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
            /// <summary>
            /// Affected by Level (<see cref="ExtremeGear.ExtremeGearData"/>).
            /// BoostSpeed = CharacterData.BoostSpeed + ExtremeGearData.BoostSpeedLvl
            /// </summary>
            [Header("Boost")]
            public float BoostSpeed;
            public float BoostChainModifier;
            [Header("Drift")]
            public float DriftDashSpeed;
            public float DriftCap;
            public float DriftDashFrames;
        }
        public SpeedStats speedStats = new SpeedStats();

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

        // HIDDEN STATS
        public float MinSpeed;
        public float FastAcceleration;
        public float Turnrate;

        /**
         * CHANGABLE VARS
         */
        public int Level;

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
            LoadStats(Level);
        }

        void FixedUpdate()
        {

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
        /// <param name="level"></param>
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
        #endregion

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

    }
}
