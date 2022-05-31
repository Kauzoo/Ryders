using System.Collections;
using System.Collections.Generic;
using Ryders.Core.Player.DefaultBehaviour;
using Ryders.Core.Player.ExtremeGear.Movement;
using Ryders.Core.Player.ExtremeGear;
using UnityEngine;

public class TestAccel : AccelerationPack
{
    public override float StandardAcceleration(PlayerBehaviour dataContainer)
    {
        Debug.Log("Override Test");
        return base.StandardAcceleration(dataContainer);
    }

    public override float StandardDeceleration(PlayerBehaviour dataContainer, dynamic additionalArgument = null)
    {
        int i = 0;
        return base.StandardDeceleration(dataContainer);
    }
}
