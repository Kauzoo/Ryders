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
    }

    [System.Serializable]
    public class Movement
    {
        public float translation = 0;
        public float boostTranslation = 0;
        public float rotation = 0;
        public float jump = 0;
        public bool boostLock = false;
    }

    [System.Serializable]
    public class Visuals
    {
        public float xOffset;
        public float yRotationMultiplier;
        public float yRotationDamping;
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        DebugRays();
        FillDebugDump();
        Grounded();
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
        movement.translation = movementVars.baseSpeed * Time.fixedDeltaTime;
        movement.translation -= Mathf.Abs(standardInputVars.sidewaysAxis) * movementVars.corneringDeceleration * Time.fixedDeltaTime;
        movement.rotation = standardInputVars.sidewaysAxis * movementVars.rotationSpeed * Time.deltaTime;
        //movement.rotation = 1 * movementVars.rotationSpeed * Time.deltaTime;

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

        Debug.Log($"Boost: { movement.boostTranslation }");

        playerTransform.Rotate(0, movement.rotation, 0);
        playerRigidbody.velocity = playerTransform.forward * (movement.translation + movement.boostTranslation);
        //ManipulatePlayerVisuals();
    }

    private void Drift()
    {

    }
    #endregion

    #region Visuals
    private void ManipulatePlayerVisuals()
    {
        float xOffset = standardInputVars.sidewaysAxis * visuals.xOffset;
        Vector3 visualPosition = new Vector3(playerTransform.position.x - xOffset, playerTransform.position.y, playerTransform.position.z);
        visualPlayerTransform.localPosition = visualPosition;

        float zTilt = standardInputVars.sidewaysAxis * visuals.zRotationMaxAngle * (-1);
        Quaternion targetQuaternion = Quaternion.Euler(visualPlayerTransform.rotation.eulerAngles.x, visualPlayerTransform.rotation.eulerAngles.y, zTilt);

        visualPlayerTransform.rotation = Quaternion.Slerp(visualPlayerTransform.rotation, targetQuaternion, visuals.zRotationDamping);

        //visualPlayerTransform.rotation = Quaternion.Euler(visualPlayerTransform.rotation.eulerAngles.x, visualPlayerTransform.rotation.eulerAngles.y, zTilt);
        visualPlayerTransform.Rotate(0, movement.rotation * visuals.yRotationMultiplier, 0, Space.World);
    }
    #endregion

    private void Grounded()
    {
        int layerMask = 1 << grounded.layerMask;
        if (Physics.Raycast(playerTransform.position, playerTransform.up * (-1), out RaycastHit hit, grounded.maxDistance, layerMask))
        {
            Debug.Log(hit.collider.gameObject.name);
            Debug.Log(hit.collider.name);
        }
    }

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
        string text = $"Translation: { movement.translation }{ Environment.NewLine }BoostTranslation: { movement.boostTranslation }" +
                $"{ Environment.NewLine }Rotation: { movement.rotation}{ Environment.NewLine }BoostLock: { movement.boostLock }";
        debugDump.text = text;
    }
    #endregion

}
