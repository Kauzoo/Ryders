using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Transform playerTransform;
    public Rigidbody playerRigidbody;


    public Transform visualPlayerTransform;

    public LookupTables LookupTables;

    private IEnumerator boostCoroutine;
    private IEnumerator driftBoostCoroutine;

    public TMPro.TextMeshProUGUI debugDump;
    public TMPro.TextMeshProUGUI rigidbodyDebugDump;

    public Transform testObject;
    public Transform respanwn;

    [System.Serializable]
    public class KeyBinds
    {
        public KeyCode fowardKey;
        public KeyCode backwardKey;
        public KeyCode leftKey;
        public KeyCode rightKey;
        public KeyCode jumpKey;
        public KeyCode boostKey;
        public KeyCode driftKey1;
        public KeyCode driftKey2;
        public KeyCode cycleLevelKey;
        public string verticalAxis;
        public string horizontalAxis;
    }

    [System.Serializable]
    public class InputVars
    {
        public bool forwardInput;
        public bool backwardInput;
        public bool leftInput;
        public bool rightInput;
        public bool jumpInput;
        public bool boostInput;
        public bool driftInput;
        public bool cycleLevelInput;
        public float forwardAxis;
        public float sidewaysAxis;
    }

    [System.Serializable]
    public class MovementVars
    {
        [Header("Speed")]
        public int minSpeed;
        public int fastAcceleration;
        public int acceleration;
        public float corneringDeceleration;
        [Header("Boost")]
        public float boostSpeed;
        public float boostLockTime;
        public float boostInterval;
        [Header("Brake")]
        public float brakeDeceleration;
        [Header("Drift")]
        public float driftDuration;     // Minimum amount of seconds for a drift boost
        public float driftBoostSpeed;   
        public float driftBoostInterval;
        public int driftBoostTicks;
        public int boostTicks;
        public float driftTurnratePassive;  // Amount that the player passively turns towards a direction, while drifting
        public float driftTurnrateMin;      // Minimum amount the player turn while drifting
        public float driftTurnrate;         // maximum rate at which the player can turn while drifting
        [Header("Turning")]
        public float turnSpeedThreshhold;
        public float lowSpeedTurnMultiplier;
        public float highSpeedTurnMultiplier;
        public float turnrate;
        public AnimationCurve turnrateCurve;
        [Header("Jump&Gravity")]
        public AnimationCurve jumpSpeed;    // Controls jump speed relative to time
        public float jumpChargeMinSpeed;
        public float jumpChargeDeceleration;
        public float gravityMultiplier;
        [Header("WallBump")]
        public float wallBumpTimer;
        public float wallBumpSpeed;
        [Header("SpeedLevels")]
        public int level1SpeedCap;
        public int level2SpeedCap;
        public int level3SpeedCap;
        [Header("Curves")]
        public AnimationCurve test;
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
        public enum MiscStates
        {
            None, Braking, JumpCharging
        }
        public float speed = 0;
        public float boostTranslation = 0;
        [Header("Turning")]
        public float rotation = 0;
        public float turningRaw = 0;    // Raw value for turning without speed multipliers
        public float turning = 0;   // Degress per Frame by which the playerTransform is rotated arround it's y-Axis
        public Vector3 savedRotation;
        [Header("Drift")]
        public float driftTurning = 0;
        public float jump = 0;
        public float driftTimeMarker = 0;
        public float driftTimer = 0;
        public float gravity = 0;
        public int level = 0;
        [Header("MovementStates")]
        public bool driftBoost = false;
        public bool drifting = false;
        public bool grounded = false;
        public bool boostLock = false;
        public bool jumping = false;
        public bool chargingJump = false;
        public bool brakeing = false;
        public bool cornering = false;
        public TranslationStates translationState = TranslationStates.Stationary;
        public CorneringStates corneringStates = CorneringStates.None;
        public MiscStates miscState = MiscStates.None;
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

    public KeyBinds keyboardKeyBinds = new KeyBinds();
    public KeyBinds controllerKeyBinds = new KeyBinds();
    public InputVars standardInputVars = new InputVars();
    public MovementVars movementVars = new MovementVars();
    public Movement movement = new Movement();
    public Visuals visuals = new Visuals();
    public GroundVars grounded = new GroundVars();
    public GroundInfo groundInfo = new GroundInfo();

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        visualPlayerTransform.SetParent(playerTransform);
    }

    // Update is called once per frame
    void Update()
    {
        Grounded();
        GetInput();
        HandleLevelSwitching();
        Move();
        DebugRays();
        FillDebugDump();
    }

    private void FixedUpdate()
    {

    }

    void SanityChecks()
    {
    }

    #region InputHandeling
    void GetInput()
    {
        // Reset
        standardInputVars.forwardInput = false;
        standardInputVars.backwardInput = false;
        standardInputVars.leftInput = false;
        standardInputVars.rightInput = false;
        standardInputVars.jumpInput = false;
        standardInputVars.boostInput = false;
        standardInputVars.driftInput = false;
        standardInputVars.cycleLevelInput = false;

        // Keys
        if (Input.GetKey(keyboardKeyBinds.fowardKey) || Input.GetKey(controllerKeyBinds.fowardKey))
        {
            standardInputVars.forwardInput = true;
        }
        if (Input.GetKey(keyboardKeyBinds.backwardKey) || Input.GetKey(controllerKeyBinds.backwardKey))
        {
            standardInputVars.backwardInput = true;
        }
        if (Input.GetKey(keyboardKeyBinds.leftKey) || Input.GetKey(controllerKeyBinds.leftKey))
        {
            standardInputVars.leftInput = true;
        }
        if (Input.GetKey(keyboardKeyBinds.rightKey) || Input.GetKey(controllerKeyBinds.rightKey))
        {
            standardInputVars.rightInput = true;
        }
        if (Input.GetKey(keyboardKeyBinds.jumpKey) || Input.GetKey(controllerKeyBinds.jumpKey))
        {
            standardInputVars.jumpInput = true;
        }
        if (Input.GetKeyDown(keyboardKeyBinds.boostKey) || Input.GetKeyDown(controllerKeyBinds.boostKey))
        {
            standardInputVars.boostInput = true;
        }
        if (Input.GetKey(keyboardKeyBinds.driftKey1) || Input.GetKey(keyboardKeyBinds.driftKey2) || Input.GetKey(controllerKeyBinds.driftKey1) || Input.GetKey(controllerKeyBinds.driftKey2))
        {
            standardInputVars.driftInput = true;
        }
        if (Input.GetKeyDown(keyboardKeyBinds.cycleLevelKey) || Input.GetKeyDown(controllerKeyBinds.cycleLevelKey))
        {
            standardInputVars.cycleLevelInput = true;
        }

        // Axis
        standardInputVars.forwardAxis = Input.GetAxis("Vertical");
        standardInputVars.sidewaysAxis = Input.GetAxis("Horizontal");


        //OTHER DEBUG ONLY
        if (Input.GetKeyDown(KeyCode.R))
        {
            playerTransform.position = respanwn.position;
        }
    }
    #endregion

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
  

        /***
         * Compute movement variables for Movement, Rotation and Gravity
         */
        // Do Accel
        Accelerate();

        // Do turn
        Turn();

        // Calculate gravity value
        movement.gravity = Gravity() * movementVars.gravityMultiplier;

        /***
         * Clean up
         */
        //CleanUpJump();
        //CleanUpDrift();

        /***
         * Do stuff relating to Input
         */
        if (standardInputVars.forwardInput)
        {
        }
        if (standardInputVars.backwardInput)
        {
        }
        if (standardInputVars.leftInput)
        {
        }
        if (standardInputVars.rightInput)
        {
        }
        if (standardInputVars.jumpInput)
        {
            Jump();
        }
        if (standardInputVars.boostInput && !movement.boostLock)
        {
            boostCoroutine = BoostTimer(movementVars.boostLockTime, movementVars.boostInterval, movementVars.boostTicks);
            StartCoroutine(boostCoroutine);
        }
        

        /***
         * Handle Ground and Turning
         */
        //playerTransform.Rotate(0, movement.rotation, 0, Space.Self);
        //playerRigidbody.MoveRotation(playerTransform.rotation);
        


        /***
         * Handle Translation and Gravity
         */
        Vector3 forwardVector = playerTransform.forward * (movement.speed + movement.boostTranslation) * Time.fixedDeltaTime;
        //forwardVector = forwardVector + movementVars.turnVectorMultiplier * playerTransform.right * movement.turning * Time.fixedDeltaTime;
        Vector3 gravityVector = Vector3.down * movement.gravity * Time.fixedDeltaTime;
        Vector3 jumpVector = playerTransform.up * movement.jump * Time.fixedDeltaTime;
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
        // If drift or breake state have already been entered, only the drift button needs to be checked
        if(standardInputVars.driftInput)
        {
            // If already drifting, continue drift if the drift button is held down
            if (movement.corneringStates == Movement.CorneringStates.DriftingL)
            {
                movement.corneringStates = Movement.CorneringStates.DriftingL;
                return;
            }
            if(movement.corneringStates == Movement.CorneringStates.DriftingR)
            {
                movement.corneringStates = Movement.CorneringStates.DriftingR;
                return;
            }
            // If already braking, continue braking if the brake button is held down
            if (movement.corneringStates == Movement.CorneringStates.Braking)
            {
                movement.corneringStates = Movement.CorneringStates.Braking;
                return;
            }
        }
        if (standardInputVars.sidewaysAxis == 0)
        {
            if (standardInputVars.driftInput)
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
            if (movement.translationState != Movement.TranslationStates.Stationary && standardInputVars.driftInput)
            {
                if(standardInputVars.sidewaysAxis > 0)
                {
                    movement.corneringStates = Movement.CorneringStates.DriftingR;
                }
                else
                {
                    movement.corneringStates = Movement.CorneringStates.DriftingL;
                }
            }
            else if(standardInputVars.sidewaysAxis != 0)
            {
                movement.corneringStates = Movement.CorneringStates.Turning;
            }
        }
    }

    private void SetMiscState()
    {
        
    }
    #endregion

    #region BasicTranslation
    /// <summary>
    /// Handles everything to do with Acceleration, except boosting
    /// </summary>
    private void Accelerate()
    {
        // Case 1: LowSpeed
        /*
         * LowSpeed Accel only takes into account if the player is braking or not (turning is ignored)
         */
        if (movement.translationState == Movement.TranslationStates.LowSpeed)
        {
            if (movement.corneringStates == Movement.CorneringStates.Braking)
            {
                movement.speed = movement.speed - movementVars.brakeDeceleration;
                // Negative speed should not be a thing
                if (movement.speed < 0)
                {
                    movement.speed = 0;
                }
            }
            else
            {
                if(standardInputVars.jumpInput)
                {
                    if(movement.speed < movementVars.jumpChargeMinSpeed)
                    {
                        movement.speed = movementVars.fastAcceleration + movement.speed;
                        return;
                    }
                    else
                    {
                        movement.speed = movement.speed - movementVars.jumpChargeDeceleration;
                    }
                }
                movement.speed = movementVars.fastAcceleration + movement.speed;
                return;
            }
        }

        // Case 2: HighSpeed
        if (movement.translationState == Movement.TranslationStates.HighSpeed)
        {
            if (movement.corneringStates == Movement.CorneringStates.Braking)
            {
                movement.speed = movement.speed - movementVars.brakeDeceleration;
                // Negative speed should not be a thing
                if (movement.speed < 0)
                {
                    movement.speed = 0;
                }
            }
            if (movement.corneringStates == Movement.CorneringStates.Turning)
            {
                // Testing quadaratic cornering decel
                //movement.speed = movement.speed - ( movementVars.corneringDeceleration * Mathf.Pow(Mathf.Abs(standardInputVars.sidewaysAxis), 2.0f));
                //movement.speed = movement.speed - (movementVars.corneringDeceleration * Mathf.Abs(standardInputVars.sidewaysAxis));
                movement.speed = movement.speed - (movementVars.test.Evaluate(movement.speed / 10) * Mathf.Abs(standardInputVars.sidewaysAxis) * movementVars.corneringDeceleration) ;
            }
            if (movement.corneringStates == Movement.CorneringStates.None)
            {
                // Make sure speed is always accelerated to the relevant level move speed cap
                switch(movement.level)
                {
                    case 1:
                        if (movement.speed < movementVars.level1SpeedCap)
                        {
                            movement.speed = movementVars.acceleration + movement.speed;
                            return;
                        }
                        break;
                    case 2:
                        if (movement.speed < movementVars.level2SpeedCap)
                        {
                            movement.speed = movementVars.acceleration + movement.speed;
                            return;
                        }
                        break;
                    case 3:
                        if (movement.speed < movementVars.level3SpeedCap)
                        {
                            movement.speed = movementVars.acceleration + movement.speed;
                            return;
                        }
                        break;
                    default:
                        Debug.LogWarning($"ERROR: Level was { movement.level }");
                        break;
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

    private void Boost()
    {

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
            movement.turningRaw = standardInputVars.sidewaysAxis * movementVars.turnrate;
            // Factor in SpeedTier
            if (movement.speed <= movementVars.turnSpeedThreshhold)
            {
                //movement.turning = movement.turningRaw * movementVars.lowSpeedTurnMultiplier;
            }
            else
            {
                //movement.turning = movement.turningRaw * movementVars.highSpeedTurnMultiplier;
            }
            movement.turning = movement.turningRaw * movementVars.turnrateCurve.Evaluate(movement.speed / 1000);

            // Make frame indipendent
            movement.turning = movement.turning * Time.deltaTime;
            playerTransform.Rotate(0, movement.turning, 0, Space.Self);
            playerRigidbody.MoveRotation(playerTransform.rotation);
        }
        else 
        {
            if(movement.corneringStates == Movement.CorneringStates.DriftingL)
            {
                // For left turn, values have to negative
                movement.driftTurning = movementVars.driftTurnratePassive * (-1) + movementVars.driftTurnrate * standardInputVars.sidewaysAxis;
                if (movement.driftTurning > movementVars.driftTurnrateMin)
                {
                    movement.driftTurning = movementVars.driftTurnrateMin * (-1);
                }
            }
            if(movement.corneringStates == Movement.CorneringStates.DriftingR)
            {
                // For right turn, values have to positive
                movement.driftTurning = movementVars.driftTurnratePassive + movementVars.driftTurnrate * standardInputVars.sidewaysAxis;
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
        Debug.Log("Jumping");
    }
    #endregion

    #region WallCollision
    private void OnCollisionEnter(Collision collision)
    {
        playerRigidbody.freezeRotation = true;
        if (collision.collider.gameObject.layer != grounded.layerMask)
        {
            StartCoroutine("BumpOfWall");
        }
        playerRigidbody.freezeRotation = false;
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
        if (standardInputVars.cycleLevelInput)
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
    #endregion

    #region Visuals
    private void ManipulatePlayerVisuals()
    {
        float xOffset = standardInputVars.sidewaysAxis * visuals.xOffset;
        Vector3 visualPosition = new Vector3(0 - xOffset, 0, 0);
        visualPlayerTransform.localPosition = Vector3.Lerp(visualPlayerTransform.localPosition, visualPosition, visuals.xOffsetDamping);

        float zTilt = standardInputVars.sidewaysAxis * visuals.zRotationMaxAngle * (-1);
        Quaternion visualZTilt = Quaternion.Euler(0, 0, zTilt);
        visualPlayerTransform.localRotation = Quaternion.Slerp(visualPlayerTransform.localRotation, visualZTilt, visuals.zRotationDamping);

        float yTilt = standardInputVars.sidewaysAxis * visuals.yRoationMaxAngle;
        Quaternion visualYTilt = Quaternion.Euler(0, yTilt, 0);
        visualPlayerTransform.localRotation = Quaternion.Slerp(visualPlayerTransform.localRotation, visualYTilt, visuals.yRotationDamping);
    }

    private void ComputeAxisFactor()
    {

    }
    #endregion

    #region Gravity & Grounded
    private void Grounded()
    {
        int layerMask = 1 << grounded.layerMask;
        if (Physics.Raycast(playerTransform.position, playerTransform.up * (-1), out RaycastHit hit, grounded.maxDistance, layerMask))
        {
            if(HasGroundChanged(hit.transform.gameObject))
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

    private void HandleGroundChanged(GameObject newGround)
    {
        Debug.Log("HandleGroundChanged is being executed");
        groundInfo.previousGround = groundInfo.currentGround;
        groundInfo.currentGround = newGround;

        float currentRotationAngle = playerTransform.rotation.eulerAngles.y + groundInfo.previousGround.transform.rotation.eulerAngles.y;
        Vector3 currentRotationVector = new Vector3(0, currentRotationAngle, 0);

        Quaternion targetRotation = Quaternion.LookRotation(groundInfo.currentGround.transform.forward, groundInfo.currentGround.transform.up);
        // WIP playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, targetRotation, Time.time * 0.1f);
        playerTransform.rotation = targetRotation;
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

    private IEnumerator BoostTimer(float blt, float bi, int bt)
    {
        for (int i = bt; i >= 0; i--)
        {
            movement.boostTranslation = (movementVars.boostSpeed * i);
            movement.boostLock = true;
            yield return new WaitForSeconds(bi);
        }
        yield return new WaitForSecondsRealtime(blt);
        movement.boostLock = false;
    }

    private IEnumerator JumpTimer(float jumpTime)
    {
        yield return new WaitForSeconds(jumpTime);
    }

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
        string text = $"Speed: { movement.speed }{ Environment.NewLine }BoostTranslation: { movement.boostTranslation }" +
                $"{ Environment.NewLine }Rotation: { movement.rotation}{ Environment.NewLine }BoostLock: { movement.boostLock }" +
                $"{ Environment.NewLine }Grounded: { movement.grounded}{ Environment.NewLine }Gravity: { movement.gravity }" +
                $"{ Environment.NewLine }QuaternionAngle: { quaternionAngle }{ Environment.NewLine }EulerAngle: { eulerAngle }" +
                $"{ Environment.NewLine }Level: { movement.level }";
        string rigidbodyText = $"Velocity-X: { playerRigidbody.velocity.x }{ Environment.NewLine }Velocity-Y: { playerRigidbody.velocity.y}" +
                $"{ Environment.NewLine }Velocity-Z: { playerRigidbody.velocity.z }{ Environment.NewLine }FPS: { fps }";
        debugDump.text = text;
        rigidbodyDebugDump.text = rigidbodyText;
    }

    void OnDrawGizmos()
    {

    }
    #endregion

}
