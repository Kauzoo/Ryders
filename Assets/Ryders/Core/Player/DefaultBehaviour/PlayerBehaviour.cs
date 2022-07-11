using System;
using Ryders.Core.InputManagement;
using UnityEngine;
using Ryders.Core.Player.Character;
using Ryders.Core.Player.ExtremeGear.Movement;
using Ryders.Core.Player.ExtremeGear;
using Ryders.Core.Player.DefaultBehaviour.Components;
using Ryders.Core.Player.DefaultBehaviour.Components.DefaultComponents;
using Ryders.Core.Player.DefaultBehaviour.Telemetry;
using static System.Environment;
using Nyr.UnityDev.Component;


namespace Ryders.Core.Player.DefaultBehaviour
{
    /// <summary>
    /// This class represents the default case for an ExtremeGear.
    /// All movement is calculated here. The character only contributes stats, not custom code.
    /// Stats for the board are retrieved from the corresponding ExtemeGear Object
    /// </summary>
    [RequireComponent(typeof(SpeedStats))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(TurnStats))]
    [RequireComponent(typeof(JumpStats))]
    [RequireComponent(typeof(FuelStats))]
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(Fuel))]
    [RequireComponent(typeof(GravityPack))]
    [RequireComponent(typeof(AccelerationPack))]
    [RequireComponent(typeof(BoostPack))]
    [RequireComponent(typeof(CorneringPack))]
    [RequireComponent(typeof(DriftPack))]
    [RequireComponent(typeof(FuelPack))]
    [RequireComponent(typeof(StatLoaderPack))]
    [RequireComponent(typeof(EventPublisherPack))]
    // TODO Update RequireComponents
    public abstract partial class PlayerBehaviour : MonoBehaviour
    {
        /**
         * DEBUG 
         */
        [System.Serializable]
        public class PBDebug
        {
            [Header("Toggles")] public bool printTransformTelemetry;
            public bool printRigidbodyTelemetry;
            public bool printMovementTelemetry;
            [Header("TelemetryText")] public TMPro.TextMeshProUGUI playerTransformTelemetry;
            public TMPro.TextMeshProUGUI playerRigidbodyTelemetry;
            public TMPro.TextMeshProUGUI playerMovementTelemetry;
            [System.NonSerialized] public Canvas debugCanvas;
        }

        public PBDebug pbDebug = new();

        [HideInInspector] public Transform playerTransform;
        [HideInInspector] public Rigidbody playerRigidbody;
        public Transform rotationAnchor;

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

        // STATS
        [HideInInspector] public SpeedStats speedStats;
        [HideInInspector] public TurnStats turnStats;
        [HideInInspector] public JumpStats jumpStats;
        [HideInInspector] public FuelStats fuelStats;

        // PACKS
        [HideInInspector] public AccelerationPack accelerationPack;
        [HideInInspector] public StatLoaderPack statLoaderPack;
        [HideInInspector] public GravityPack gravityPack;
        [HideInInspector] public JumpPack jumpPack;
        [HideInInspector] public WallCollisionPack wallCollisionPack;
        [HideInInspector] public FuelPack fuelPack;
        [HideInInspector] public DriftPack driftPack;
        [HideInInspector] public BoostPack boostPack;
        [HideInInspector] public CorneringPack corneringPack;
        [HideInInspector] public EventPublisherPack eventPublisherPack;

        // RUNTIME VAR CONTAINERS
        [HideInInspector] public Movement movement;
        [HideInInspector] public Fuel fuel;

        /// <summary>
        /// Assigns all RequiredComponents
        /// </summary>
        [ContextMenu("Setup References")]
        protected virtual void Setup()
        {
            // RIGIDBODY
            playerRigidbody = GetComponent<Rigidbody>();
            playerRigidbody.useGravity = false;
            playerRigidbody.isKinematic = true;

            // TRANSFORM
            playerTransform = GetComponent<Transform>();

            // ROTATION ANCHOR
            rotationAnchor.SetParent(null);

            // STAT CONTAINERS
            this.SafeGetComponent(ref speedStats);
            this.SafeGetComponent(ref turnStats);
            this.SafeGetComponent(ref jumpStats);
            this.SafeGetComponent(ref fuelStats);

            // PACKS
            this.SafeGetComponent(ref accelerationPack);
            this.SafeGetComponent(ref statLoaderPack);
            this.SafeGetComponent(ref gravityPack);
            try
            {
                this.SafeGetComponent(ref jumpPack);
            }
            catch (MissingReferenceException e)
            {
                Debug.LogError(e);
            }

            try
            {
                this.SafeGetComponent(ref wallCollisionPack);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            this.SafeGetComponent(ref fuelPack);
            this.SafeGetComponent(ref driftPack);
            this.SafeGetComponent(ref boostPack);
            this.SafeGetComponent(ref corneringPack);
            this.SafeGetComponent(ref eventPublisherPack);

            // RUNTIME
            this.SafeGetComponent(ref movement);
            this.SafeGetComponent(ref fuel);

            // TODO Rework this along side the entire InputManagement
            masterInput = FindObjectOfType<MasterInput>();
            if (masterInput != null)
            {
                if (masterInput.players.TryGetValue(playerSignifier, out var inputPlayerOut))
                    inputPlayer = inputPlayerOut;
                else
                    Debug.LogError($"@{this.ToString()}.Setup(): Failed to retrieve InputPlayer");
            }
            else
                Debug.LogError($"@{this.ToString()}.Setup(): Failed to find MastInput Object in Scene");
        }
        
        public virtual void TestAcceleration()
        {
            accelerationPack.Master();
            accelerationPack.OnExitPack();
        }

        public virtual void TestBoost()
        {
            boostPack.Master();
        }

        public virtual void TestDrift()
        {
            driftPack.Master();
        }

        public virtual void TestCornering()
        {
            corneringPack.Master();
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
            if (movement.CorneringState is (CorneringStates.CorneringL or CorneringStates.CorneringR))
            {
                rotationAnchor.Rotate(0, movement.Turning, 0, Space.Self);
                return;
            }

            if (movement.DriftState is DriftStates.DriftingL or DriftStates.DriftingR)
            {
                rotationAnchor.Rotate(0, movement.DriftTurning, 0, Space.Self);
                return;
            }
        }

        public virtual void MasterMoveTest()
        {
            playerTransform.position += Formula.SpeedToRidersSpeed(movement.Speed) * playerTransform.forward;
            playerTransform.position += movement.Gravity * 0.5f * Vector3.down;
        }

        public virtual void FixedUpdateTest()
        {
            inputPlayer.GetInput();
            statLoaderPack.LoadStatsMaster();
            fuelPack.Master();
            gravityPack.Master();
            TestMove();
            MasterTurnTest();
            MasterMoveTest();
        }

        public virtual void UpdateTest()
        {
            UpdateDebug();
        }

        #region Utility

        public virtual string GetTransformTelemetry()
        {
            // TODO Expand this
            var str = $"Position: {TelemetryHelper.GetVector3Formated(playerTransform.position)}"
                      + $"{NewLine}Rotation: {TelemetryHelper.GetVector3Formated(playerTransform.rotation.eulerAngles)}";
            return str;
        }

        public virtual string GetRigidbodyTelemetry()
        {
            // TODO Expand this
            var str = $"Velocity: {TelemetryHelper.GetVector3Formated(playerRigidbody.velocity)}";
            return str;
        }

        #endregion

        #region Debug

        public virtual void UpdateDebug()
        {
            PrintTelemetry();
        }

        private void SetupDebug()
        {
            if (pbDebug.printMovementTelemetry && pbDebug.playerMovementTelemetry is null)
                Debug.LogWarning("@SetupDebug: Missing playerMovementTelemetry Object.");
            if (pbDebug.printRigidbodyTelemetry && pbDebug.playerRigidbodyTelemetry is null)
                Debug.LogWarning("@SetupDebug: Missing playerRigidbodyTelemetry Object.");
            if (pbDebug.printTransformTelemetry && pbDebug.playerTransformTelemetry is null)
                Debug.LogWarning("@SetupDebug: Missing playerMovementTelemetry Object.");
        }

        private void PrintTelemetry()
        {
            if (pbDebug.printMovementTelemetry)
                pbDebug.playerMovementTelemetry.text = "Movement" + NewLine + movement.GetTelemetry();
            if (pbDebug.printRigidbodyTelemetry)
                pbDebug.playerRigidbodyTelemetry.text = "Rigidbody" + NewLine + GetRigidbodyTelemetry();
            if (pbDebug.printTransformTelemetry)
                pbDebug.playerTransformTelemetry.text = "Transform" + NewLine + GetTransformTelemetry();
        }

        #endregion
    }
}

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    public abstract class SpeedStats : MonoBehaviour
    {
        [Header("Speed")] public float TopSpeed;
        public float SpeedHandlingMultiplier;
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
        [Header("Turning")] public float TurnSpeedLoss;
        [Header("Jump")] public float JumpChargeMinSpeed;
        public float JumpChargeDecelleration;
    }

    // TURN STATS
    /// <summary>
    /// All the stats that affect the calculations for rotation (around local y-axis)
    /// Contains stats for regular Turning as well as Drifting
    /// </summary>
    public abstract class TurnStats : MonoBehaviour
    {
        [Header("Turning")] public float Turnrate;
        public float TurnRateMax;
        public float TurnLowSpeedMultiplier;
        public AnimationCurve TurnSpeedLossCurve;
        public AnimationCurve TurnrateCurve;
        [Header("Drift")] public float DriftTurnrateMin;
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
        public int MinRings;
        public int MaxRings;
        public int[] LevelCaps;
    }

    public abstract class Movement : MonoBehaviour
    {
        [Header("Speed")] public float Speed;
        public float MaxSpeed;

        [Header("Boost")] public float BoostTimer;

        [Header("Turning")] public float Turning;

        [Header("Drift")] public float DriftTurning;
        public float DriftTimer;

        [Header("Jump")] public float JumpChargeDuration;
        public float JumpCharge;
        public float JumpSpeed;
        public float JumpAccelleration;

        [Header("Gravity")] public float Gravity;
        public float NormalForce;

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
            var str = $"Speed: {Speed}" + $"{NewLine}MaxSpeed: {MaxSpeed}"
                                        + $"{NewLine}BoostTimer: {BoostTimer}"
                                        + $"{NewLine}DriftTimer: {DriftTimer}"
                                        + $"{NewLine}DriftState: {DriftState}"
                                        + $"{NewLine}CorneringState: {CorneringState}"
                                        + $"{NewLine}TurnRate: {Turning}"
                                        + $"{NewLine}Grounded: {Grounded}"
                                        + $"{NewLine}Gravity: {Gravity}";
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
        CorneringL,
        CorneringR
    }

    public enum GroundedStates
    {
        None,
        Grounded
    }

    public enum MaxSpeedState
    {
        Cruising,
        Boosting
    }
}