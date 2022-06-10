using System;
using System.Collections;
using System.Collections.Generic;
using Ryders.Core.InputManagement;
using UnityEngine;
using Ryders.Core.Player.Character;
using Ryders.Core.Player.ExtremeGear.Movement;
using Ryders.Core.Player.ExtremeGear;
using Ryders.Core.Player.DefaultBehaviour.Components;
using Ryders.Core.Player.DefaultBehaviour;

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
    // TODO Update RequireComponents
    public abstract class PlayerBehaviour : MonoBehaviour
    {
        public Transform playerTransform;
        public Rigidbody playerRigidbody;

        [Header("Input")] public PlayerSignifier playerSignifier;
        [SerializeReference] public MasterInput masterInput;
        public InputPlayer inputPlayer;
        

        [Header("ScriptableObjects")]
        // Contains basic character data
        public CharacterData characterData;

        // Contains basic player stats
        public DefaultPlayerStats defaultPlayerStats;

        // Contains basic extreme gear data
        public ExtremeGearData extremeGearData;

        [Header("Stats")] [Tooltip("Contains all stats that are used for Translation at runtime")]
        public SpeedStats speedStats;
        public TurnStats turnStats;
        public JumpStats jumpStats;
        public FuelStats fuelStats;

        [Header("Packs")] public AccelerationPack accelerationPack;
        public StatLoaderPack statLoaderPack;
        public GravityPack gravityPack;
        public StateDeterminationPack stateDeterminationPack;
        public JumpPack jumpPack;
        public WallCollisionPack wallCollisionPack;
        public FuelPack fuelPack;
        public DriftPack driftPack;
        public BoostPack boostPack;
        
        [Header("RuntimeVarContainers")] public Movement movement;
        [Tooltip("Contains info about fuel")] public Fuel fuel;

        /// <summary>
        /// Assigns all RequiredComponents
        /// </summary>
        protected virtual void Setup()
        {
            // RIGIDBODY
            playerRigidbody = GetComponent<Rigidbody>();
            playerRigidbody.useGravity = false;
            
            // STAT CONTAINERS
            if(TryGetComponent<SpeedStats>(out var speedStatsOut))
                speedStats = speedStatsOut;
            else
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find SpeedStats");
            if(TryGetComponent<TurnStats>(out var turnStatsOut))
                turnStats = turnStatsOut;
            else
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find TurnStats");
            if(TryGetComponent<JumpStats>(out var jumpStatsOut))
                jumpStats = jumpStatsOut;
            else
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find JumpStats");
            if(TryGetComponent<FuelStats>(out var fuelStatsOut))
                fuelStats = fuelStatsOut;
            else
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find FuelStats");
            
            // PACKS
            if(TryGetComponent<AccelerationPack>(out var accelerationPackOut))
                accelerationPack = accelerationPackOut;
            else
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find AccelerationPack");
            if(TryGetComponent<StatLoaderPack>(out var statLoaderPackPackOut))
                statLoaderPack = statLoaderPackPackOut;
            else
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find StatLoaderPack");
            if(TryGetComponent<GravityPack>(out var gravityPackOut))
                gravityPack = gravityPackOut;
            else
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find GravityPack");
            if(TryGetComponent<StateDeterminationPack>(out var stateDeterminationPackOut))
                stateDeterminationPack = stateDeterminationPackOut;
            else
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find StateDeterminationPack");
            if(TryGetComponent<JumpPack>(out var jumpPackOut))
                jumpPack = jumpPackOut;
            else
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find JumpPack");
            if(TryGetComponent<WallCollisionPack>(out var wallCollisionPackOut))
                wallCollisionPack = wallCollisionPackOut;
            else
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find WallCollisionPack");
            if(TryGetComponent<FuelPack>(out var fuelPackOut))
                fuelPack = fuelPackOut;
            else
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find FuelPack");
            if (TryGetComponent<DriftPack>(out var driftPackOut))
                driftPack = driftPackOut;
            else
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find DriftPack");
            if (TryGetComponent<BoostPack>(out var boostPackOut))
                boostPack = boostPackOut;
            else
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find BoostPack");
            
            // RUNTIME
            if(TryGetComponent<Movement>(out var movementOut))
                movement = movementOut;
            else
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find Movement");
            if(TryGetComponent<Fuel>(out var fuelOut))
                fuel = fuelOut;
            else
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find Fuel");

            masterInput = FindObjectOfType<MasterInput>();
            Debug.Log("hello");
            if (!masterInput.Equals(null))
            {
                if (masterInput.players.Equals(null))
                {
                    Debug.Log("Wtf");
                }
                if (masterInput.players.TryGetValue(playerSignifier, out var inputPlayerOut))
                {
                    inputPlayer = inputPlayerOut;
                    Debug.Log("Was geht");
                }
                else
                {
                    Debug.LogError($"@{this.ToString()}.Setup(): Failed to retrieve InputPlayer");
                }
            }
            else
            {
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find MastInput Object in Scene");
            }
        }
        
        public virtual void TestAcceleration()
        {
            movement.Speed += accelerationPack.StandardAcceleration() + accelerationPack.StandardDeceleration();
        }

        public virtual void TestBoost()
        {
            boostPack.DetermineBoostState();
            boostPack.Boost();
        }

        public virtual void TestMove()
        {
            if (movement.BoostTimer == 0)
            {
                movement.MaxSpeed = speedStats.TopSpeed;
            }
            TestAcceleration();
            TestBoost();
        }

        public virtual void MasterMoveTest()
        {
            playerRigidbody.velocity = new Vector3(0, 0, movement.Speed);
        }

        public virtual void FixedUpdateTest()
        {
            statLoaderPack.LoadStatsMaster();
            TestMove();
            MasterMoveTest();
            inputPlayer.GetInput();
            if (inputPlayer.GetInputContainer().Boost)
            {
                Debug.Log("Boost");
            }
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
            public float BoostDuration;

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
            [Header("Turning")] public float Turnrate;
            public AnimationCurve TurnSpeedLossCurve;
            public AnimationCurve TurnrateCurve;
            [Header("Drift")] public float DriftTurnratePassive;
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
            [Header("Fuel Stats")] public FuelType FuelType;
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
            [Header("Speed")] public float Speed;
            public float MaxSpeed;

            [Header("Acceleration")] public float Acceleration; // The sum of all acceleration effects
            public float Deceleration; // The sum of all decelerations

            [Header("Boost")] public float BoostTimer;
            public float BoostLockTimer;

            [Header("Turning")] public float Rotation;
            public float TurningRaw;
            public float Turning;

            [Header("Drift")] public float DriftTurning;
            public float DriftTimer;

            [Header("Jump")] public float JumpChargeDuration;
            public float JumpCharge;
            public float JumpSpeed;
            public float JumpAccelleration;

            [Header("Gravity")] public float Gravity;

            [Header("MovementStates")] public bool Grounded;
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
            Stationary,
            Cruising,
            Boosting,
            EnteredBoost
        }

        public enum SpeedStates
        {
            LowSpeed,
            MediumSpeed,
            HighSpeed
        }

        public enum DriftStates
        {
            None,
            DriftingL,
            DriftingR,
            Break
        }

        public enum JumpStates
        {
            None,
            JumpCharging,
            Jumping,
            JumpLanding
        }

        public enum CorneringStates
        {
            None,
            TurningL,
            TurningR
        }

        public enum GroundedStates
        {
            None,
            Grounded
        }
    }
