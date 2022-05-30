using System.Collections;
using System.Collections.Generic;
using Ryders.Core.Player.ExtremeGear.Movement;
using UnityEngine;

public class TestAccel : IAccelerationPack
{
    
    public float StandardAcceleration(float speed, float maxSpeed, float fastAccel, float regAccel,
        CorneringStates corneringStates, DriftStates driftStates, GroundedStates groundedStates)
    {
        
        throw new System.NotImplementedException();
    }

    public float DownhillAccelleration()
    {
        throw new System.NotImplementedException();
    }

    public float StandardDeceleration(float speed, float maxSpeed)
    {
        throw new System.NotImplementedException();
    }
}
