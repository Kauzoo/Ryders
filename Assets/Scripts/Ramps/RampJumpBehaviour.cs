using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampJumpBehaviour : MonoBehaviour
{
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * Jump parameters
     * distance, height, arc, speed
     *  
     *  -Precompute a function, which is used as the flight arc (parabola)
     */
    public void DetermineJumpCurve(float jumpChargeDuration, float releasePoint, JumpDirection jumpDirection)
    {

    }
}
