using System;
using System.Collections;
using System.Collections.Generic;
using Ryders.Core.Player.DefaultBehaviour;
using Ryders.Core.Player.DefaultBehaviour.Components;
using UnityEngine;

public abstract class BoostPack : MonoBehaviour
{
    public PlayerBehaviour playerBehaviour;
    
    public void Start()
    {
        playerBehaviour = GetComponent<PlayerBehaviour>();
    }

    public virtual void Boost()
    {
        playerBehaviour.movement.MaxSpeed = playerBehaviour.speedStats.BoostSpeed;
        playerBehaviour.movement.Speed = playerBehaviour.speedStats.BoostSpeed;
        if (playerBehaviour.movement.DriftState is DriftStates.DriftingL or DriftStates.DriftingR)
        {
            BoostChain();
        }
    }
    
    public virtual void BoostChain()
    {
        playerBehaviour.movement.Speed = playerBehaviour.movement.Speed * playerBehaviour.speedStats.BoostChainModifier;
    }
}
