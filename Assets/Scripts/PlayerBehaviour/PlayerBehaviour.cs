using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ryders.Core.Player.Character;
using Ryders.Core.Player.ExtremeGear.Movement;
using Ryders.Core.Player.ExtremeGear;
using Ryders.Core.Player.DefaultBehaviour.Components;
using Object = UnityEngine.Object;

namespace Ryders.Core.Player.DefaultBehaviour
{
    /// <summary>
    /// This class represents the default case for an ExtremeGear.
    /// All movement is calculated here. The character only contributes stats, not custom code.
    /// Stats for the board are retrieved from the corresponding ExtemeGear Object
    /// </summary>
    [RequireComponent(typeof(SpeedStats))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(TurnStats))]
    [RequireComponent(typeof(JumpStats))]
    [RequireComponent(typeof(FuelStats))]
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(Fuel))]
    public abstract class PlayerBehaviour : MonoBehaviour
    {
        // Class that handles all thing input
        public GameObject InputModule;
        // Contains basic character data
        public CharacterData CharacterData;
        // Contains basic player stats
        public DefaultPlayerStats DefaultPlayerStats;
        // Contains basic extreme gear data
        public ExtremeGearData ExtremeGearData;

        public SpeedStats speedStats;
        
        public Transform playerTransform;
        public Rigidbody playerRigidbody;

        public AccelerationPack AccelerationPack;
        //public IStatLoader StatLoader;


        #region StaticVars
        /**
         * STATIC VARS
         * These values only change on level change
         */
        public TurnStats turnStats;
        public JumpStats jumpStats;
        public FuelStats fuelStats;
        #endregion

        #region ChangableVars
        /**
         * CHANGABLE VARS
         */
        public Movement movement;
        [Tooltip("Contains info about fuel")] public Fuel fuel;
        #endregion

        public virtual void BaseStart()
        {
            speedStats = GetComponent<SpeedStats>();
            playerRigidbody = GetComponent<Rigidbody>();
            AccelerationPack = GetComponent<AccelerationPack>();
            InherritanceTest();
        }

        public virtual void InherritanceTest()
        {
            Debug.Log("wtf");
            Debug.Log(this.ToString());
            Debug.Log("" + AccelerationPack.StandardAcceleration(this));
        }

        public override string ToString()
        {
            return "Tolles to string";
        }
    }
}

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    public abstract class SpeedStats : MonoBehaviour
    {
        [Header("Speed")] public float TopSpeed;
        [Header("Accelleration")] public float MinSpeed;
        public float Acceleration;
        public float FastAccelleration;

        /// <summary>
        /// Affected by Level (<see cref="ExtremeGear.ExtremeGearData"/>).
        /// BoostSpeed = CharacterData.BoostSpeed + ExtremeGearData.BoostSpeedLvl
        /// From SRDX Datasheets
        /// </summary>
        [Header("Boost")] public float BoostSpeed;

        /// <summary>
        /// BoostChainModifier = CharacterData.BoostChainModifier + ExtremeGearData.BoostChainModifier
        /// From SRDX Datasheets
        /// </summary>
        public float BoostChainModifier;

        [Header("Breake")] public float BreakeDecelleration;
        [Header("Drift")] public float DriftDashSpeed;
        public float DriftCap;
        public float DriftDashFrames;
        [Header("Turning")] public AnimationCurve TurnSpeedLoss;
        [Header("Jump")] public float JumpChargeMinSpeed;
        public float JumpChargeDecelleration;
    }
    
    // TURN STATS
    /// <summary>
    /// All the stats that affect the calculations for rotation (arround local y-axis)
    /// Contains stats for regular Turning as well as Drifting
    /// </summary>
    public abstract class TurnStats : MonoBehaviour
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
    
    public abstract class JumpStats : MonoBehaviour
    {
        public float JumpSpeedMax;
        public AnimationCurve JumpAccelleration;
    }
    
    public abstract class FuelStats : MonoBehaviour
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
    
    public abstract class Movement : MonoBehaviour
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
    
    public abstract class Fuel : MonoBehaviour
    {
        public float CurrentFuel;
        public int Level;
        public int Rings;
    }
    
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
