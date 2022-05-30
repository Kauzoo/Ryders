using System.Collections;
using System.Collections.Generic;
using Ryders.Core.Player.ExtremeGear.Movement;
using Ryders.Core.Player.ExtremeGear;
using UnityEngine;

public class TestAccel<TPlayerBehaviour> : IAccelerationPack<TPlayerBehaviour>
{
    public float DownhillAcceleration(TPlayerBehaviour dataContainer)
    {
        throw new System.NotImplementedException();
    }

    public float StandardDeceleration(TPlayerBehaviour dataContainer)
    {
        throw new System.NotImplementedException();
    }

    public float CorneringDeceleration(TPlayerBehaviour dataContainer)
    {
        throw new System.NotImplementedException();
    }

    public float BreakingDeceleration(TPlayerBehaviour dataContainer)
    {
        throw new System.NotImplementedException();
    }

    public float JumpChargeDeceleration(TPlayerBehaviour dataContainer)
    {
        throw new System.NotImplementedException();
    }

    public float UphillDeceleration(TPlayerBehaviour dataContainer)
    {
        throw new System.NotImplementedException();
    }

    public float OffroadDeceleration(TPlayerBehaviour dataContainer)
    {
        throw new System.NotImplementedException();
    }
}
