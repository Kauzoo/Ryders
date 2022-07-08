using System.Collections;
using System.Collections.Generic;
using Ryders.Core.Player.DefaultBehaviour;
using Ryders.Core.Player.DefaultBehaviour.Components;
using UnityEngine;

public class TestAccel : AccelerationPack
{
    protected override float StandardAcceleration()
    {
        return base.StandardAcceleration();
    }

    protected override float StandardDeceleration()
    {
        return base.StandardDeceleration();
    }

    public override void Setup()
    {
        throw new System.NotImplementedException();
    }

    public override void Master()
    {
        throw new System.NotImplementedException();
    }
}
