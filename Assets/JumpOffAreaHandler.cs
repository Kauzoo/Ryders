using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOffAreaHandler : MonoBehaviour
{
    public RampJumpBehaviour rampJump;

    /**
     * States
     */
    public bool charging = false;
    public bool chargeRelease = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.TryGetComponent<PlayerBehaviour>(out PlayerBehaviour playerBehaviour))
            {
                Debug.Log("OnTriggerEnter");
                // Check if player is already charging a jump while entering
                if(playerBehaviour.movement.miscState == PlayerBehaviour.Movement.MiscStates.JumpCharging)
                {
                    charging = true;
                }
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(other.TryGetComponent<PlayerBehaviour>(out PlayerBehaviour playerBehaviour))
            {
                Debug.Log("OnTriggerStay");
                if(playerBehaviour.standardInputVars.jumpInput)
                {
                    charging = true;
                }

                // Charge on Enter section
                if(charging && !playerBehaviour.standardInputVars.jumpInput)
                {
                    chargeRelease = true;
                    /*
                     * Jump now
                     */
                    float jumpChargeDuration = playerBehaviour.movement.jumpChargeDuration;
                    RampJumpBehaviour.JumpDirection jumpDirection;
                    if (playerBehaviour.standardInputVars.forwardAxis > 0.2f)
                    {
                        jumpDirection = RampJumpBehaviour.JumpDirection.DOWN;
                    }
                    else
                    {
                        jumpDirection = RampJumpBehaviour.JumpDirection.UP;
                    }
                    rampJump.DoJump(playerBehaviour.gameObject, playerBehaviour, jumpChargeDuration, jumpDirection);
                }
            }
        }
    }
}
