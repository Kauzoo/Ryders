using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Enums;

namespace Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        /**
         * Required Refrences
         */
        // Controlls to which player this Behaviour script belongs
        public Enums.Player playerPort;    // Needs to set before startup
        private InputHandler inputHandler;
        public InputHandler.PlayerInput inputVars;
        private PlayerStatLoader playerStatLoader;

        public Enums.Character character;                 // Needs to set before startup
        public Enums.Board board;                         // Needs to set before startup

        public Transform playerTransform;
        public Rigidbody playerRigidbody;

        public Transform visualPlayerTransform;

        /**
         * DEBUG
         */
        public TMPro.TextMeshProUGUI debugDump;
        public TMPro.TextMeshProUGUI rigidbodyDebugDump;
        public TMPro.TextMeshProUGUI movementDebugDump;
        public Transform testObject;
        public Transform respanwn;

        /**
         * Utils
         */
        public bool constantStatReload;

        /// <summary>
        /// Contains all movement related vars
        /// Values are loaded in from a combination of player and board
        /// Can be updated to refelect level changes
        /// </summary>
        [System.Serializable]
        public class MovementVars
        {
            [Header("Speed")]
            public int minSpeed;            // Speed threshold for fast accel
            public int fastAcceleration;
            public int acceleration;        // LVL_AFFECTED
            public int cruisingSpeed;       // LVL_AFFECTED
            public float deceleration;
            public float corneringDeceleration;
            [Header("Boost")]
            public float boostSpeed;        // LVL_AFFECTED | Speed to which the player is set while boosting
            public float boostDuration;     // How long the boost lasts
            public float boostLockTime;     // Duration for which a player can't boost after boosting (total: driftDuration + boostLockTime)
            [Header("Breake")]
            public float breakeDeceleration;
            [Header("Drift")]
            public float driftDuration;     // Minimum amount of seconds for a drift boost
            public float driftBoostSpeed;       // LVL_AFFECTED
            public float driftTurnratePassive;  // Amount that the player passively turns towards a direction, while drifting
            public float driftTurnrateMin;      // Minimum amount the player turn while drifting
            public float driftTurnrate;         // maximum rate at which the player can turn while drifting
            [Header("Cornering")]
            public float lowSpeedTurnMultiplier;    // currently unused
            public float highSpeedTurnMultiplier;   // currently unused
            public float turnrate;                  // LVL_AFFECTED
            public AnimationCurve turnrateCurve;    // Determines how your ability to turn is affected by speed
            public AnimationCurve turnSpeedLossCurve;
            [Header("Jump&Gravity")]
            public float jumpSpeedMax;          // Controls jump speed relative to time
            public AnimationCurve jumpAccel;    // Acceleration for a jump
            public float jumpChargeMinSpeed;
            public float jumpChargeDeceleration;
            public float gravityMultiplier;
            [Header("WallBump")]
            public float wallBumpTimer;
            public float wallBumpSpeed;
        }

        /// <summary>
        /// Contains all air related vars
        /// </summary>
        [System.Serializable]
        public class AirVars
        {
            public int maxAir;                  // LVL_AFFECTED
            public int passiveAirDrain;         // LVL_AFFECTED
            public int driftAirCost;            // LVL_AFFECTED
            public int boostCost;               // LVL_AFFECTED
            public int tornadoCost;             // LVL_AFFECTED
            public float AirGainTrick;
            public float AirGainShortcut;
            public float AirGainAutorotate;
            public float JumpAirLoss;
        }

        /// <summary>
        /// Container for stats that are affected by level
        /// </summary>
        [System.Serializable]
        public class LevelStats
        {
            [Header("Speed")]
            public int acceleration;        // LVL_AFFECTED
            public int cruisingSpeed;       // LVL_AFFECTED
            [Header("Boost")]
            public float boostSpeed;
            [Header("Drift")]
            public float driftBoostSpeed;
            [Header("Cornering")]
            public float turnrate;
            [Header("Air")]
            public int maxAir;                  // LVL_AFFECTED
            public int passiveAirDrain;         // LVL_AFFECTED
            public int driftAirCost;            // LVL_AFFECTED
            public int boostCost;               // LVL_AFFECTED
            public int tornadoCost;             // LVL_AFFECTED
        }

        [System.Serializable]
        public class Movement
        {
            public enum TranslationStates
            {
                Stationary, LowSpeed, HighSpeed
            }
            public enum CorneringStates
            {
                None, Turning, DriftingL, DriftingR, Braking
            }
            public enum JumpStates
            {
                None, JumpCharging, Jumping, JumpLanding
            }
            public enum BoostStates
            {
                None, Boost, Boosting, BoostLock
            }
            public float speed = 0;
            public float cruisingSpeed = 0;
            public float boostSpeed = 0;
            [Header("Turning")]
            public float rotation = 0;
            public float turningRaw = 0;    // Raw value for turning without speed multipliers
            public float turning = 0;   // Degress per Frame by which the playerTransform is rotated arround it's y-Axis
            public Vector3 savedRotation;
            [Header("Drift")]
            public float driftTurning = 0;
            public float driftTimer = 0;
            public int level = 0;
            [Header("Jump")]
            public float jumpChargeDuration = 0;
            public float jumpSpeed = 0;
            public float jumpAccel = 0;
            [Header("Gravity")]
            public float gravity = 0;
            [Header("Boost")]
            public float boostTimer = 0;    // Duration in seconds that has been boosted for
            public float boostLockTimer = 0;
            [Header("MovementStates")]
            public bool grounded = false;
            public bool overrideControl = false;        // temp | needs to be reworked
            public TranslationStates translationState = TranslationStates.Stationary;
            public CorneringStates corneringStates = CorneringStates.None;
            public JumpStates miscState = JumpStates.None;
            public BoostStates boostState = BoostStates.None;
        }

        [System.Serializable]
        public class Air
        {
            public int air;
        }

        [System.Serializable]
        public class GroundInfo
        {
            public GameObject currentGround;
            public GameObject previousGround;
        }

        [System.Serializable]
        public class Visuals
        {
            public float xOffset;
            public float xOffsetDamping;
            public float xRotationDamping;
            public float xRotationMaxAngle;
            public float yRotationDamping;
            public float yRoationMaxAngle;
            public float zRotationDamping;
            public float zRotationMaxAngle;
        }

        [System.Serializable]
        public class GroundVars
        {
            public float maxDistance;
            public int layerMask;
        }

        // Variable storage for calcs
        public MovementVars movementVars = new MovementVars();
        public AirVars airVars = new AirVars();
        public LevelStats statsLevel1 = new LevelStats();
        public LevelStats statsLevel2 = new LevelStats();
        public LevelStats statsLevel3 = new LevelStats();
        
        // Activly changing
        public Movement movement = new Movement();
        public Visuals visuals = new Visuals();
        public GroundVars grounded = new GroundVars();
        public GroundInfo groundInfo = new GroundInfo();

        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = 120;

            // Go through inital setup
            inputHandler = FindObjectOfType<InputHandler>();
            if (inputHandler == null)
            {
                Debug.LogError("ERROR: No InputHandler found");
            }
            switch (playerPort)
            {
                case Enums.Player.Player1:
                    inputVars = inputHandler.player1Input;
                    break;
                case Enums.Player.Player2:
                    inputVars = inputHandler.player2Input;
                    break;
                default:
                    Debug.LogWarning($"PlayerBehaviour {this.gameObject.name} does not have a Player assigned");
                    break;
            }

            playerStatLoader = gameObject.GetComponent<PlayerStatLoader>();
            playerStatLoader.LoadStats(this);

            UpdateLevelStats();
        }

        // Update is called once per frame
        void Update()
        {
            EnableConstantStatReload();
            if (!movement.overrideControl)
            {
                UpdateLevelStats();
                Grounded();
                HandleLevelSwitching();
                Move();
                DebugRays();
                FillDebugDump();
            }
        }

        private void FixedUpdate()
        {

        }

        #region Movement
        void Move()
        {
            /***
             * State Determination
             */
            // Set the TranslationState (based on current moveSpeed)
            SetTranslationState();
            // Set cornering state (based on current input)
            SetCorneringState();
            // Set misc state
            SetMiscState();
            SetBoostState();
            SetLevelStates();


            /***
             * Handle the results of the determined states, relating to speed/accel, rotation and jump
             */
            Accelerate();
            Turn();
            Jump();



            // Calculate gravity value
            if (movement.miscState != Movement.JumpStates.Jumping)
            {
                movement.gravity = Gravity() * (movement.gravity + movementVars.gravityMultiplier);
            }

            /***
             * Handle Translation and Gravity
             */
            Vector3 forwardVector = playerTransform.forward * movement.speed * Time.fixedDeltaTime;
            //forwardVector = forwardVector + movementVars.turnVectorMultiplier * playerTransform.right * movement.turning * Time.fixedDeltaTime;
            Vector3 gravityVector = Vector3.down * movement.gravity * Time.fixedDeltaTime;
            Vector3 jumpVector = playerTransform.up * movement.jumpSpeed * Time.fixedDeltaTime;
            playerRigidbody.velocity = forwardVector + gravityVector + jumpVector;

            //playerTransform.rotation *= Quaternion.FromToRotation(playerTransform.forward, forwardVector);
            //playerRigidbody.MoveRotation(playerTransform.rotation);

            /***
             * Handle purely visual component of Movement
             */
            ManipulatePlayerVisuals();
        }

        #region StateSetters
        private void SetTranslationState()
        {
            if (movement.speed < movementVars.minSpeed)
            {
                if (movement.speed == 0)
                {
                    movement.translationState = Movement.TranslationStates.Stationary;
                }
                else
                {
                    movement.translationState = Movement.TranslationStates.LowSpeed;
                }
            }
            else
            {
                movement.translationState = Movement.TranslationStates.HighSpeed;
            }
        }

        // This needs to be expanded to make sure only legal state changes are possible (with regards to drifting/braking and so on)
        private void SetCorneringState()
        {
            // Check if a drift has been released
            if ((movement.corneringStates == Movement.CorneringStates.DriftingL || movement.corneringStates == Movement.CorneringStates.DriftingR) && !inputVars.driftInput)
            {
                movement.driftTimer = 0;
                movement.speed = movementVars.driftBoostSpeed;
                movement.corneringStates = Movement.CorneringStates.None;
                return;
            }
            // If drift or breake state have already been entered, only the drift button needs to be checked
            if (inputVars.driftInput)
            {
                // If already drifting, continue drift if the drift button is held down
                if (movement.corneringStates == Movement.CorneringStates.DriftingL)
                {
                    movement.corneringStates = Movement.CorneringStates.DriftingL;
                    movement.driftTimer = movement.driftTimer + Time.deltaTime;
                    return;
                }
                if (movement.corneringStates == Movement.CorneringStates.DriftingR)
                {
                    movement.corneringStates = Movement.CorneringStates.DriftingR;
                    movement.driftTimer = movement.driftTimer + Time.deltaTime;
                    return;
                }
                // If already braking, continue braking if the brake button is held down
                if (movement.corneringStates == Movement.CorneringStates.Braking)
                {
                    movement.corneringStates = Movement.CorneringStates.Braking;
                    return;
                }
            }
            if (inputVars.horizontalAxis == 0)
            {
                if (inputVars.driftInput)
                {
                    movement.corneringStates = Movement.CorneringStates.Braking;
                }
                else
                {
                    movement.corneringStates = Movement.CorneringStates.None;
                }
            }
            else
            {
                // Enter Drift state
                if (movement.translationState != Movement.TranslationStates.Stationary && inputVars.driftInput)
                {
                    if (inputVars.horizontalAxis > 0)
                    {
                        movement.corneringStates = Movement.CorneringStates.DriftingR;
                    }
                    else
                    {
                        movement.corneringStates = Movement.CorneringStates.DriftingL;
                    }
                }
                else if (inputVars.horizontalAxis != 0)
                {
                    movement.corneringStates = Movement.CorneringStates.Turning;
                }
            }
        }

        private void SetMiscState()
        {
            if (inputVars.jumpInput && movement.grounded)
            {
                movement.miscState = Movement.JumpStates.JumpCharging;
                movement.jumpChargeDuration = movement.jumpChargeDuration + Time.deltaTime;
                return;
            }
            if (movement.grounded && movement.miscState == Movement.JumpStates.JumpCharging && !inputVars.jumpInput)
            {
                movement.miscState = Movement.JumpStates.Jumping;
                movement.jumpSpeed = movementVars.jumpSpeedMax;
                return;
            }
            if (movement.miscState == Movement.JumpStates.JumpLanding && movement.grounded)
            {
                movement.miscState = Movement.JumpStates.None;
                return;
            }
            if (movement.miscState != Movement.JumpStates.Jumping)
            {
                movement.miscState = Movement.JumpStates.None;
                movement.jumpChargeDuration = 0;
            }
        }

        private void SetBoostState()
        {
            if (inputVars.boostInput && movement.boostState == Movement.BoostStates.None)
            {
                movement.boostState = Movement.BoostStates.Boost;
                movement.boostTimer = 0;
                return;
            }
            if (movement.boostState == Movement.BoostStates.Boosting)
            {
                movement.boostTimer = movement.boostTimer + Time.deltaTime;
            }
            if (movement.boostState == Movement.BoostStates.Boosting && movement.boostTimer > movementVars.boostDuration)
            {
                movement.boostState = Movement.BoostStates.BoostLock;
            }
            if (movement.boostState == Movement.BoostStates.BoostLock)
            {
                movement.boostLockTimer = movement.boostLockTimer + Time.deltaTime;
            }
            if (movement.boostState == Movement.BoostStates.BoostLock && movement.boostLockTimer > movementVars.boostLockTime)
            {
                movement.boostState = Movement.BoostStates.None;
                movement.boostTimer = 0;
                movement.boostLockTimer = 0;
            }

        }

        private void SetLevelStates()
        {
            switch (movement.level)
            {
                case 1:
                    //movement.cruisingSpeed = movementVars.level1SpeedCap;
                    movement.boostSpeed = movementVars.boostSpeed;
                    break;
                case 2:
                    //movement.cruisingSpeed = movementVars.level2SpeedCap;
                    movement.boostSpeed = movementVars.boostSpeed;
                    break;
                case 3:
                    //movement.cruisingSpeed = movementVars.level3SpeedCap;
                    movement.boostSpeed = movementVars.boostSpeed;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region BasicTranslation
        /// <summary>
        /// Handles everything affecting speed
        /// </summary>
        private void Accelerate()
        {
            // Case 0: Handle any sort of boost
            if (movement.boostState == Movement.BoostStates.Boost)
            {
                movement.speed = movementVars.boostSpeed;
                movement.boostState = Movement.BoostStates.Boosting;
            }
            // Unless boosting, decel should always be applied
            if (movement.boostState != Movement.BoostStates.Boosting)
            {
                if (movement.speed > movementVars.cruisingSpeed)
                {
                    movement.speed = movement.speed - movementVars.deceleration;
                }
            }

            // Case 1: LowSpeed
            /*
             * LowSpeed Accel only takes into account if the player is braking or not (turning is ignored)
             */
            if (movement.translationState == Movement.TranslationStates.LowSpeed)
            {
                // Breaking
                if (movement.corneringStates == Movement.CorneringStates.Braking)
                {
                    movement.speed = movement.speed - movementVars.breakeDeceleration;
                    // Negative speed should not be a thing
                    if (movement.speed < 0)
                    {
                        movement.speed = 0;
                    }
                }
                else
                {
                    // Jump
                    if (movement.miscState == Movement.JumpStates.JumpCharging)
                    {
                        if (movement.speed < movementVars.jumpChargeMinSpeed)
                        {
                            movement.speed = movementVars.fastAcceleration + movement.speed;
                            return;
                        }
                        else
                        {
                            movement.speed = movement.speed - movementVars.jumpChargeDeceleration;
                            //movement.jumpAccel = movementVars.jumpChargeDeceleration;
                        }
                    }
                    movement.speed = movementVars.fastAcceleration + movement.speed;
                    return;
                }
            }

            // Case 2: HighSpeed
            if (movement.translationState == Movement.TranslationStates.HighSpeed)
            {
                // Jump
                if (movement.miscState == Movement.JumpStates.JumpCharging)
                {
                    movement.speed = movement.speed - movementVars.jumpChargeDeceleration;
                    //movement.jumpAccel = movementVars.jumpChargeDeceleration;
                }
                // Breaking
                if (movement.corneringStates == Movement.CorneringStates.Braking)
                {
                    movement.speed = movement.speed - movementVars.breakeDeceleration;
                    //movement.brakeAccel = movementVars.brakeDeceleration;
                    // Negative speed should not be a thing
                    if (movement.speed < 0)
                    {
                        movement.speed = 0;
                    }
                }
                // Cornering | Calculate TurnAccel for the frame
                if (movement.corneringStates == Movement.CorneringStates.Turning)
                {
                    movement.speed = movement.speed - (movementVars.turnSpeedLossCurve.Evaluate(movement.speed / 10) * Mathf.Abs(inputVars.horizontalAxis) * movementVars.corneringDeceleration);
                    //movement.turnAccel = (movementVars.test.Evaluate(movement.speed / 10) * Mathf.Abs(standardInputVars.sidewaysAxis) * movementVars.corneringDeceleration) * (-1);
                }
                // Regular Accel
                if (movement.corneringStates == Movement.CorneringStates.None)
                {
                    // Acceleration Behaviour while boosting
                    if (movement.boostState == Movement.BoostStates.Boosting)
                    {
                        if (movement.speed < movement.boostSpeed)
                        {
                            movement.speed = movement.speed + movementVars.acceleration;
                            //movement.baseAccel = movementVars.acceleration;
                        }
                        if (movement.speed > movement.boostSpeed)
                        {
                            movement.speed = movement.boostSpeed;
                        }
                    }
                    // Acceleration Behaviour while cruising
                    else
                    {
                        if (movement.speed < movement.cruisingSpeed)
                        {
                            movement.speed = movement.speed + movementVars.acceleration;
                            //movement.baseAccel = movementVars.acceleration;
                        }
                    }
                }
            }

            // Case 3: Stationary
            if (movement.translationState == Movement.TranslationStates.Stationary)
            {
                Debug.LogWarning("State not coded yet");
                movement.speed = 100;
                return;
            }
        }

        /// <summary>
        /// Handles all things turning
        /// </summary>
        private void Turn()
        {
            // Always reset to zero first
            movement.turningRaw = 0;
            movement.turning = 0;

            if (movement.corneringStates == Movement.CorneringStates.Turning)
            {
                movement.turningRaw = inputVars.horizontalAxis * movementVars.turnrate;
                // Factor in SpeedTier
                /*
                if (movement.speed <= movementVars.turnSpeedThreshhold)
                {
                    //movement.turning = movement.turningRaw * movementVars.lowSpeedTurnMultiplier;
                }
                else
                {
                    //movement.turning = movement.turningRaw * movementVars.highSpeedTurnMultiplier;
                }
                */
                movement.turning = movement.turningRaw * movementVars.turnrateCurve.Evaluate(movement.speed / 1000);

                // Make frame indipendent
                movement.turning = movement.turning * Time.deltaTime;
                playerTransform.Rotate(0, movement.turning, 0, Space.Self);
                playerRigidbody.MoveRotation(playerTransform.rotation);
            }
            else
            {
                if (movement.corneringStates == Movement.CorneringStates.DriftingL)
                {
                    // For left turn, values have to negative
                    movement.driftTurning = movementVars.driftTurnratePassive * (-1) + movementVars.driftTurnrate * inputVars.horizontalAxis;
                    if (movement.driftTurning > movementVars.driftTurnrateMin)
                    {
                        movement.driftTurning = movementVars.driftTurnrateMin * (-1);
                    }
                }
                if (movement.corneringStates == Movement.CorneringStates.DriftingR)
                {
                    // For right turn, values have to positive
                    movement.driftTurning = movementVars.driftTurnratePassive + movementVars.driftTurnrate * inputVars.horizontalAxis;
                    if (movement.driftTurning < movementVars.driftTurnrateMin)
                    {
                        movement.driftTurning = movementVars.driftTurnrateMin;
                    }
                }
                movement.driftTurning = movement.driftTurning * Time.deltaTime;
                playerTransform.Rotate(0, movement.driftTurning, 0, Space.Self);
                playerRigidbody.MoveRotation(playerTransform.rotation);
            }
        }
        #endregion

        #region Jump
        private void Jump()
        {
            if (movement.miscState == Movement.JumpStates.Jumping)
            {

                if (movement.jumpSpeed > 0)
                {
                    movement.jumpSpeed = movement.jumpSpeed - movementVars.jumpAccel.Evaluate(movement.jumpSpeed / 100);
                }
                else
                {
                    movement.jumpSpeed = 0;
                    movement.miscState = Movement.JumpStates.JumpLanding;
                }
            }
        }
        #endregion

        #region Drift
        private void Drift()
        {

        }
        #endregion

        #region Boost
        private void Boost()
        {
            if (movement.boostState == Movement.BoostStates.Boost)
            {
                movement.speed = movementVars.boostSpeed;
                movement.boostState = Movement.BoostStates.Boosting;
            }
        }
        #endregion

        #region WallCollision
        private void OnCollisionEnter(Collision collision)
        {
            //playerRigidbody.freezeRotation = true;
            if (collision.collider.gameObject.layer != grounded.layerMask)
            {
                StartCoroutine("BumpOfWall");
            }
            //playerRigidbody.freezeRotation = false;
        }

        private IEnumerator BumpOfWall()
        {
            movement.speed = movementVars.wallBumpSpeed * (-1);
            yield return new WaitForSeconds(movementVars.wallBumpTimer);
            movement.speed = 0;
        }
        #endregion

        #region StateSwitching & Air
        private void HandleLevelSwitching()
        {
            if (inputVars.cycleLevelInput)
            {
                if (movement.level < 3)
                {
                    movement.level++;
                }
                else if (movement.level >= 3 || movement.level <= 0)
                {
                    movement.level = 1;
                }
            }
        }

        private void HandleLevelChange()
        {
            
        }

        private void UpdateLevelStats()
        {
            switch(movement.level)
            {
                case 1:
                    movementVars.acceleration = statsLevel1.acceleration;
                    movementVars.boostSpeed = statsLevel1.boostSpeed;
                    movementVars.cruisingSpeed = statsLevel1.cruisingSpeed;
                    movementVars.turnrate = statsLevel1.turnrate;
                    airVars.boostCost = statsLevel1.boostCost;
                    airVars.driftAirCost = statsLevel1.driftAirCost;
                    airVars.maxAir = statsLevel1.maxAir;
                    airVars.tornadoCost = statsLevel1.tornadoCost;
                    airVars.passiveAirDrain = statsLevel1.passiveAirDrain;
                    break;
                case 2:
                    movementVars.acceleration = statsLevel2.acceleration;
                    movementVars.boostSpeed = statsLevel2.boostSpeed;
                    movementVars.cruisingSpeed = statsLevel2.cruisingSpeed;
                    movementVars.turnrate = statsLevel2.turnrate;
                    airVars.boostCost = statsLevel2.boostCost;
                    airVars.driftAirCost = statsLevel2.driftAirCost;
                    airVars.maxAir = statsLevel2.maxAir;
                    airVars.tornadoCost = statsLevel2.tornadoCost;
                    airVars.passiveAirDrain = statsLevel2.passiveAirDrain;
                    break;
                case 3:
                    movementVars.acceleration = statsLevel3.acceleration;
                    movementVars.boostSpeed = statsLevel3.boostSpeed;
                    movementVars.cruisingSpeed = statsLevel3.cruisingSpeed;
                    movementVars.turnrate = statsLevel3.turnrate;
                    airVars.boostCost = statsLevel3.boostCost;
                    airVars.driftAirCost = statsLevel3.driftAirCost;
                    airVars.maxAir = statsLevel3.maxAir;
                    airVars.tornadoCost = statsLevel3.tornadoCost;
                    airVars.passiveAirDrain = statsLevel3.passiveAirDrain;
                    break;
                default:
                    Debug.LogWarning("No valid Level");
                    break;
            }
        }

        private void PassiveAirDrain()
        {

        }
        #endregion

        #region Visuals
        private void ManipulatePlayerVisuals()
        {
            float xOffset = inputVars.horizontalAxis * visuals.xOffset;
            Vector3 visualPosition = new Vector3(0 - xOffset, 0, 0);
            visualPlayerTransform.localPosition = Vector3.Lerp(visualPlayerTransform.localPosition, visualPosition, visuals.xOffsetDamping);

            float zTilt = inputVars.horizontalAxis * visuals.zRotationMaxAngle * (-1);
            Quaternion visualZTilt = Quaternion.Euler(0, 0, zTilt);
            visualPlayerTransform.localRotation = Quaternion.Slerp(visualPlayerTransform.localRotation, visualZTilt, visuals.zRotationDamping);

            float yTilt = inputVars.horizontalAxis * visuals.yRoationMaxAngle;
            Quaternion visualYTilt = Quaternion.Euler(0, yTilt, 0);
            visualPlayerTransform.localRotation = Quaternion.Slerp(visualPlayerTransform.localRotation, visualYTilt, visuals.yRotationDamping);
        }

        private void ComputeAxisFactor()
        {

        }
        #endregion

        #region Ramps
        private void OnRampEnter()
        {

        }

        private void OnRampApex()
        {

        }
        #endregion

        #region Gravity & Grounded
        private void Grounded()
        {
            int layerMask = 1 << grounded.layerMask;
            if (Physics.Raycast(playerTransform.position, playerTransform.up * (-1), out RaycastHit hit, grounded.maxDistance, layerMask))
            {
                if (HasGroundChanged(hit.transform.gameObject))
                {
                    HandleGroundChanged(hit.transform.gameObject);
                }
                CorrectGroundDistance(hit.distance);
                movement.grounded = true;
            }
            else if (Physics.Raycast(playerTransform.position, Vector3.down, out RaycastHit hitAlt, grounded.maxDistance, layerMask))
            {
                {
                    if (HasGroundChanged(hitAlt.transform.gameObject))
                    {
                        HandleGroundChanged(hitAlt.transform.gameObject);
                    }
                    CorrectGroundDistance(hit.distance);
                    movement.grounded = true;
                }
            }
            else
            {
                movement.grounded = false;
            }
        }

        private void CorrectGroundDistance(float distance)
        {
            if (distance < grounded.maxDistance)
            {
                Debug.Log("Code is reached");
                //playerRigidbody.isKinematic = true;
                playerTransform.Translate(playerTransform.up * (grounded.maxDistance - distance));
                //playerRigidbody.MovePosition(playerTransform.up * (grounded.maxDistance - distance));
                //playerRigidbody.isKinematic = false;
            }
        }

        private int Gravity()
        {
            if (movement.grounded)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        private bool HasGroundChanged(GameObject newGround)
        {
            if (groundInfo.currentGround == null)
            {
                groundInfo.currentGround = playerTransform.gameObject;
            }
            if (!groundInfo.currentGround.Equals(newGround))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Handles Ground allignment
        /// WIP: Make it work with ground that is not world axis alligned
        /// </summary>
        /// <param name="newGround"></param>
        private void HandleGroundChanged(GameObject newGround)
        {
            Debug.Log("HandleGroundChanged is being executed");
            groundInfo.previousGround = groundInfo.currentGround;
            groundInfo.currentGround = newGround;

            float currentRotationAngle = playerTransform.rotation.eulerAngles.y; //+ groundInfo.previousGround.transform.rotation.eulerAngles.y;
            Vector3 currentRotationVector = new Vector3(0, currentRotationAngle, 0);

            // Alligns player to the new ground
            Quaternion targetRotation = Quaternion.LookRotation(groundInfo.currentGround.transform.forward, groundInfo.currentGround.transform.up);
            playerTransform.rotation = targetRotation;

            //playerTransform.rotation = Quaternion.LookRotation(groundInfo.previousGround.transform.forward, groundInfo.currentGround.transform.up);
            playerTransform.Rotate(currentRotationVector, Space.Self);
        }

        private bool TryGetGroundObject(out GameObject groundObject)
        {
            int layerMask = 1 << grounded.layerMask;
            if (Physics.Raycast(visualPlayerTransform.position, playerTransform.up * (-1), out RaycastHit hit, grounded.maxDistance, layerMask))
            {
                groundObject = hit.transform.gameObject;
                return true;
            }
            groundObject = null;
            return false;
        }
        #endregion


        #endregion

        #region Utils
        private void ReloadStats()
        {
            playerStatLoader.LoadStats(this);
        }

        private void EnableConstantStatReload()
        {
            if(constantStatReload)
            {
                ReloadStats();
            }
        }
        #endregion

        #region Debug
        private void DebugCircleMovement()
        {
            //playerTransform.gameObject.
        }

        private void DebugRays()
        {
            Debug.DrawRay(playerTransform.position, playerTransform.forward, Color.green, 10, false);
            Debug.DrawRay(playerTransform.position, playerTransform.up * (-1) * grounded.maxDistance, Color.yellow, 10, false);
        }

        private void FillDebugDump()
        {
            int layerMask = 1 << grounded.layerMask;
            float quaternionAngle = 0;
            float eulerAngle = 0;
            if (Physics.Raycast(playerTransform.position, playerTransform.up * (-1), out RaycastHit hit, grounded.maxDistance, layerMask))
            {
                quaternionAngle = Quaternion.Angle(hit.transform.rotation, playerTransform.rotation);
                eulerAngle = playerTransform.rotation.eulerAngles.y + hit.transform.rotation.eulerAngles.y;
            }
            float fps = 1 / Time.unscaledDeltaTime;
            fps = Mathf.Round(fps);
            string text = $"Speed: { movement.speed }" +
                    $"{ Environment.NewLine }Rotation: { movement.rotation}{ Environment.NewLine }BoostLock: " +
                    $"{ Environment.NewLine }Grounded: { movement.grounded}{ Environment.NewLine }Gravity: { movement.gravity }" +
                    $"{ Environment.NewLine }QuaternionAngle: { quaternionAngle }{ Environment.NewLine }EulerAngle: { eulerAngle }" +
                    $"{ Environment.NewLine }Level: { movement.level }";
            string rigidbodyText = $"Velocity-X: { playerRigidbody.velocity.x }{ Environment.NewLine }Velocity-Y: { playerRigidbody.velocity.y}" +
                    $"{ Environment.NewLine }Velocity-Z: { playerRigidbody.velocity.z }{ Environment.NewLine }FPS: { fps }";
            debugDump.text = text;
            rigidbodyDebugDump.text = rigidbodyText;
        }

        private void FillMovementDebugDump()
        {

        }

        void OnDrawGizmos()
        {

        }
        #endregion

    }
}
