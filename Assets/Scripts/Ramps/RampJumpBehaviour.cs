using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampJumpBehaviour : MonoBehaviour
{
    /**
     * Settings
     */
    public AnimationCurve baseJumpArc;      // Jump arc is parabola shaped
    public float rotMul;

    /**
     * Static Requirements
     */
    public GameObject targetArea;
    public GameObject jumpOffArea;
    public GameObject jumpCamera;
    public GameObject mainCamera;

    /**
     * Vars
     */
    public GameObject player;
    public PlayerBehaviour playerBehaviour;
    public Transform visualPlayer;
    public float jumpChargeDuration;
    public JumpDirection jumpDirection;
    public Vector3 startPoint;  // start point in local space
    public Vector3 endPoint;    // end point in local space
    public bool jumping = false;

    /**
     * Testing
     */
    public GameObject testObject;
    public float speed;
    public float currentSpeed;


    // enums
    public enum JumpDirection
    {
        UP, DOWN
    }

    public enum TrickDirection
    {
        UP, DOWN, LEFT, RIGHT
    }

    // Start is called before the first frame update
    void Start()
    {
        //testObject.transform.position = gameObject.transform.TransformPoint(jumpOffArea.transform.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if(jumping)
        {
            JumpArc();
            DoTricks();
        }
    }

    public void DoTricks()
    {
        visualPlayer.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * Time.deltaTime * rotMul, 0), Space.Self);
        Debug.Log("tets");
    }

    public void DoJump(GameObject player, PlayerBehaviour playerBehaviour, float jumpChargeDuration, JumpDirection jumpDirection)
    {
        jumping = true;
        this.player = player;
        this.playerBehaviour = playerBehaviour;
        this.jumpChargeDuration = jumpChargeDuration;
        this.jumpDirection = jumpDirection;
        this.visualPlayer = playerBehaviour.visualPlayerTransform;
        playerBehaviour.movement.overrideControl = true;
        playerBehaviour.playerRigidbody.isKinematic = true;
        player.transform.rotation = Quaternion.Euler(0, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z);
        jumpCamera.SetActive(true);
        var camBehaviour = jumpCamera.GetComponent<JumpCameraBehaviour>();
        camBehaviour.AttachToPlayer(player);
        mainCamera.SetActive(false);
        
        DetermineJumpCurve();
    }

    /**
     * Jump parameters
     * distance, height, arc, speed
     *  
     *  -Precompute a function, which is used as the flight arc (parabola)
     */
    public void DetermineJumpCurve()
    {
        GetStartPoint();
        GetEndPoint(jumpChargeDuration, 0.0f, jumpDirection);
        baseJumpArc.AddKey(startPoint.x, startPoint.y);
        baseJumpArc.AddKey(endPoint.x, endPoint.y);
    }

    private void JumpArc()
    {
        float newY = baseJumpArc.Evaluate(currentSpeed);

        player.transform.Translate((gameObject.transform.TransformPoint(currentSpeed, newY, 0) - player.transform.position), Space.World); //= gameObject.transform.TransformPoint(currentSpeed, newY, 0);

        currentSpeed += speed * Time.deltaTime;

        // Exit condition
        if (gameObject.transform.InverseTransformPoint(player.transform.position).x > endPoint.x)
        {
            jumping = false;
            playerBehaviour.movement.overrideControl = false;
            playerBehaviour.playerRigidbody.isKinematic = false;
            jumpCamera.SetActive(false);
            mainCamera.SetActive(true);
            currentSpeed = 0;
        }
    }

    #region Getters&Setters
    public void GetStartPoint()
    {
        startPoint = gameObject.transform.InverseTransformPoint(player.transform.position);
    }

    public void GetEndPoint(float jumpChargeDuration, float releasePoint, JumpDirection jumpDirection)
    {
        endPoint = targetArea.transform.localPosition;
    }
    #endregion
}
