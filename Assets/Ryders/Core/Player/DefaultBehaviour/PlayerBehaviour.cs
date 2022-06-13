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
using Ryders.Core.Player.DefaultBehaviour.Telemetry;

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
        /**
         * DEBUG 
         */
        [Header("Debug")]
        public TMPro.TextMeshProUGUI playerTransformTelemetry;
        public TMPro.TextMeshProUGUI playerRigidbodyTelemetry;
        public TMPro.TextMeshProUGUI playerMovementTelemetry; 
        
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
        public CorneringPack corneringPack;
        
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
            playerRigidbody.isKinematic = true;
            
            // TRANSFORM
            playerTransform = GetComponent<Transform>();

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
            if (TryGetComponent<CorneringPack>(out var corneringPackOut))
                corneringPack = corneringPackOut;
            else
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find CorneringPack");
            
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
            accelerationPack.MasterAcceleration();
        }

        public virtual void TestBoost()
        {
            boostPack.DetermineBoostState();
            boostPack.Boost();
        }

        public virtual void TestDrift()
        {
            driftPack.MasterDrift();
            driftPack.MasterDriftTurn();
        }

        public virtual void TestCornering()
        {
            corneringPack.MasterCornering();
        }

        public virtual void TestMove()
        {
            TestDrift();
            TestCornering();
            TestBoost();
            TestAcceleration();
        }

        public virtual void MasterTurnTest()
        {
            if (movement.CorneringState == CorneringStates.Cornering)
            {
                playerTransform.Rotate(0, movement.Turning, 0, Space.Self);
            }
            if (movement.DriftState is DriftStates.DriftingL or DriftStates.DriftingR)
            {
                playerTransform.Rotate(0, movement.DriftTurning, 0, Space.Self);
            }
            //playerRigidbody.MoveRotation(playerTransform.rotation);
        }
            
        public virtual void MasterMoveTest()
        {
            playerTransform.position += Formula.SpeedToRidersSpeed(movement.Speed) * playerTransform.forward;
            //var forwardVector = movement.Speed * Time.fixedDeltaTime * 3f * playerTransform.forward;
            //playerRigidbody.velocity = forwardVector;
        }

        public virtual void FixedUpdateTest()
        {
            inputPlayer.GetInput();
            statLoaderPack.LoadStatsMaster();
            TestMove();
            MasterTurnTest();
            MasterMoveTest();
        }

        public virtual void UpdateTest()
        {
            PrintTelemetry();
        }

        #region Debug

        public virtual void PrintTelemetry()
        {
            // TODO Improve this
            playerMovementTelemetry.text = "Movement" + Environment.NewLine + movement.GetTelemetry();
            playerRigidbodyTelemetry.text = "Rigidbody" + Environment.NewLine + GetRigidbodyTelemetry();
            playerTransformTelemetry.text = "Transform" + Environment.NewLine + GetTransformTelemetry();
        }
            
        public virtual string GetTransformTelemetry()
        {
            // TODO Expand this
            string str = $"Position: {TelemetryHelper.GetVector3Formated(playerTransform.position)}"
                         + $"{Environment.NewLine}Rotation: {TelemetryHelper.GetVector3Formated(playerTransform.rotation.eulerAngles)}";
            return str;
        }

        public virtual string GetRigidbodyTelemetry()
        {
            // TODO Expand this
            string str = $"Velocity: {TelemetryHelper.GetVector3Formated(playerRigidbody.velocity)}";
            return str;
        }
        

        #endregion
    }
}

namespace Ryders.Core.Player.DefaultBehaviour.Components
    {
        public abstract class SpeedStats : MonoBehaviour
        {
            [Header("Speed")] public float TopSpeed;
            [Header("Acceleration")] public float AccelerationLow;
            public float AccelerationMedium;
            public float AccelerationHigh;
            public float AccelerationLowThreshold;
            public float AccelerationMediumThreshold;
            public float AccelerationOffRoadThreshold;

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

            [Header("Break")] public float BreakeDecelleration;
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
            public float DriftTurnrateMax;
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
            
            [Header("Boost")] public float BoostTimer;

            [Header("Turning")]
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
            public MaxSpeedState MaxSpeedState = MaxSpeedState.Cruising;
            public TranslationStates TranslationState = TranslationStates.Stationary;
            public DriftStates DriftState = DriftStates.None;
            public JumpStates JumpState = JumpStates.None;
            public CorneringStates CorneringState = CorneringStates.None;
            public SpeedStates SpeedState = SpeedStates.LowSpeed;
            public GroundedStates GroundedState = GroundedStates.None;

            // TODO Add more stuff to Telemetry
            public virtual string GetTelemetry()
            {
                var str = $"Speed: {Speed}" + $"{Environment.NewLine}MaxSpeed: {MaxSpeed}"
                                             + $"{Environment.NewLine}BoostTimer: {BoostTimer}"
                                             + $"{Environment.NewLine}DriftTimer: {DriftTimer}"
                                             + $"{Environment.NewLine}DriftState: {DriftState}"
                                             + $"{Environment.NewLine}CorneringState: {CorneringState}";
                return str;
            }
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
            Cornering
        }

        public enum GroundedStates
        {
            None,
            Grounded
        }

        public enum MaxSpeedState
        {
            Cruising, Boosting
        }
    }
