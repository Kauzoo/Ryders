using System.Collections;
using System.Collections.Generic;
using Ryders.Core.Player.DefaultBehaviour;
using Ryders.Core.Player.DefaultBehaviour.Components;
using UnityEngine;

public class TestAccel : AccelerationPack
{
    public override float StandardAcceleration(PlayerBehaviour dataContainer)
    {
        return base.StandardAcceleration(dataContainer);
    }

    public override float StandardDeceleration(PlayerBehaviour dataContainer)
    {
        int i = 0;
        base.Setup();
        return base.StandardDeceleration(dataContainer);
    }

    public override void Setup()
    {
        base.Setup();
    }
}
