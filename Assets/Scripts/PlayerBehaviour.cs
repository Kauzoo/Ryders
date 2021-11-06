using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Transform playerTransform;
    public Rigidbody playerRigidbody;


    public Transform visualPlayerTransform;

    private IEnumerator boostCoroutine;

    public TMPro.TextMeshProUGUI debugDump;

    public Transform testObject;

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
        public float forwardAxis;
        public float sidewaysAxis;
    }

    [System.Serializable]
    public class MovementVars
    {
        public float baseSpeed;
        public float corneringDeceleration;
        public float boostSpeed;
        public float boostLockTime;
        public float boostInterval;
        public int boostTicks;
        public float speed;
        public float rotationSpeed;
        public float jumpSpeed;
        public float gravityMultiplier;
    }

    [System.Serializable]
    public class Movement
    {
        public float translation = 0;
        public float boostTranslation = 0;
        public float rotation = 0;
        public float jump = 0;
        public float gravity = 0;
        public bool grounded = false;
        public bool boostLock = false;
    }

    [System.Serializable]
    public class Roation
    {
        public float currentRotationAngleEuler;
        public float currentRoationAngleQuaternion;
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
    public Roation rotation = new Roation();
    public Visuals visuals = new Visuals();
    public GroundVars grounded = new GroundVars();
    public GroundInfo groundInfo = new GroundInfo();

    // Start is called before the first frame update
    void Start()
    {
        visualPlayerTransform.SetParent(playerTransform);
    }

    // Update is called once per frame
    void Update()
    {
        Grounded();
        GetInput();
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

        // Axis
        standardInputVars.forwardAxis = Input.GetAxis("Vertical");
        standardInputVars.sidewaysAxis = Input.GetAxis("Horizontal");
    }
    #endregion

    #region Movement
    void Move()
    {
        /***
         * Compute movement variables for Movement, Rotation and Gravity
         */
        movement.translation = movementVars.baseSpeed * Time.fixedDeltaTime;
        movement.translation -= Mathf.Abs(standardInputVars.sidewaysAxis) * movementVars.corneringDeceleration * Time.fixedDeltaTime;
        movement.rotation = standardInputVars.sidewaysAxis * movementVars.rotationSpeed * Time.deltaTime;
        movement.gravity = Gravity() * movementVars.gravityMultiplier * Time.fixedDeltaTime;

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
            playerRigidbody.velocity = playerTransform.up * movementVars.jumpSpeed;
        }
        if (standardInputVars.boostInput && !movement.boostLock)
        {
            boostCoroutine = BoostTimer(movementVars.boostLockTime, movementVars.boostInterval, movementVars.boostTicks);
            StartCoroutine(boostCoroutine);
        }
        if (standardInputVars.driftInput)
        {
            Drift();
        }

        /***
         * Handle Ground and Rotation
         */
        playerTransform.Rotate(0, movement.rotation, 0, Space.Self);

        /***
         * Handle Translation and Gravity
         */
        Vector3 forwardVector = playerTransform.forward * (movement.translation + movement.boostTranslation);
        Vector3 gravityVector = Vector3.down * movement.gravity;
        playerRigidbody.velocity = forwardVector + gravityVector;

        /***
         * Handle purely visual component of Movement
         */
        ManipulatePlayerVisuals();
    }
    private void Drift()
    {

    }

    private IEnumerator SpeedRampUp()
    {
        yield return new WaitForSeconds(1);
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
            movement.grounded = true;
        }
        else if (Physics.Raycast(playerTransform.position, Vector3.down, out RaycastHit hitAlt, grounded.maxDistance, layerMask))
        {
            {
                if (HasGroundChanged(hitAlt.transform.gameObject))
                {
                    HandleGroundChanged(hitAlt.transform.gameObject);
                }
                movement.grounded = true;
            }
        }
        else
        {
            movement.grounded = false;
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

        // Compute current y-Rotation Angle
        //float currentRotationAngle = Quaternion.Angle(groundInfo.previousGround.transform.rotation, playerTransform.rotation);
        float currentRotationAngle = playerTransform.rotation.eulerAngles.y + groundInfo.previousGround.transform.rotation.eulerAngles.y;
        Vector3 currentRotationVector = new Vector3(0, currentRotationAngle, 0);

        playerTransform.rotation = Quaternion.LookRotation(groundInfo.currentGround.transform.forward, groundInfo.currentGround.transform.up);
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

    }

    private void DebugRays()
    {
        Debug.DrawRay(playerTransform.position, playerTransform.forward, Color.green, 10, false);
        Debug.DrawRay(playerTransform.position, playerTransform.up * (-1) * grounded.maxDistance, Color.yellow, 10, false);
    }

    private void FillDebugDump()
    {
        int layerMask = 1 << grounded.layerMask;
        float angle = 0;
        float otherAngle = 0;
        if (Physics.Raycast(playerTransform.position, playerTransform.up * (-1), out RaycastHit hit, grounded.maxDistance, layerMask))
        {
            angle = Quaternion.Angle(hit.transform.rotation, playerTransform.rotation);
            otherAngle = playerTransform.rotation.eulerAngles.y + hit.transform.rotation.eulerAngles.y;
        }
        string text = $"Translation: { movement.translation }{ Environment.NewLine }BoostTranslation: { movement.boostTranslation }" +
                $"{ Environment.NewLine }Rotation: { movement.rotation}{ Environment.NewLine }BoostLock: { movement.boostLock }" +
                $"{ Environment.NewLine }Grounded: { movement.grounded} { Environment.NewLine } Gravity: { movement.gravity }" +
                $"{ Environment.NewLine }Angle: { angle } { Environment.NewLine } OherAngle: { otherAngle }";
        debugDump.text = text;
    }

    void OnDrawGizmos()
    {

    }
    #endregion

}
