using System;
using System.Collections;
using System.Collections.Generic;
using Ryders.Core.InputManagement;
using Ryders.Core.Player.DefaultBehaviour;
using Ryders.Core.Player.DefaultBehaviour.Components;
using UnityEngine;

/// <summary>
/// Contains Method for entering a regular Boost, BoostChaining and CountingDown the BoostTimer
/// "BoostState" is based of the BoostTimer. BoostState basically comes down to MaxSpeed.
/// A Boost is Reset when the Timer is reset to 0, but IMPORTANT, whenever the BoostTimer is reset to 0
/// the MaxSpeed is also reset it CruisingValue 
/// </summary>
public abstract class BoostPack : MonoBehaviour
{
    public PlayerBehaviour playerBehaviour;

    public void Start()
    {
        playerBehaviour = GetComponent<PlayerBehaviour>();
    }

    /// <summary>
    /// Comment Not Current
    /// Careful: Changes TranslationState to Boosting
    /// This Method should always be written in such a way that it can only enter BoostState when not currently in
    /// BoostState. It's not responsible for exiting BoostState again.
    /// Behaviour depends on BoostInput, TranslationState as well as DriftState 
    /// </summary>
    public virtual void Boost()
    {
        if (playerBehaviour.inputPlayer.GetInputContainer().Boost && playerBehaviour.movement.BoostTimer == 0)
        {
            playerBehaviour.movement.MaxSpeed = playerBehaviour.speedStats.BoostSpeed;
            playerBehaviour.movement.Speed = playerBehaviour.speedStats.BoostSpeed;
            playerBehaviour.movement.BoostTimer = playerBehaviour.speedStats.BoostDuration;
            if (playerBehaviour.movement.DriftState is DriftStates.DriftingL or DriftStates.DriftingR)
            {
                BoostChain();
            }
        }
    }

    /// <summary>
    /// Responsible for Resetting the BoostTimer to 0 / Counting it down and resetting the MaxSpeed to it's cruising
    /// Speed value when the BoostTimer is reset
    /// Thereby basically responsible for Resetting Boost
    /// </summary>
    public virtual void DetermineBoostState()
    {
        // TODO Implement other things that should reset the BoostTimer to 0. Look into Bonks
        // TODO Bonks
        // Whenever the BoostTimer is reset to 0, the Boost is essentially refreshed
        // MaxSpeed also has to be 
        // Jump cancels Boost | Not being grounded cancels Boost
        // QTE should cancel Boost | Getting Attacked should cancel Boost | Bonks should cancel
        if (playerBehaviour.inputPlayer.GetInputContainer().Jump || !playerBehaviour.movement.Grounded)
        {
            playerBehaviour.movement.BoostTimer = 0;
            playerBehaviour.movement.MaxSpeed = playerBehaviour.speedStats.TopSpeed;
        }
        // TODO Cleanup the MaxSpeed Reset
        // If the BoostTimer is greater 0 count down
        if (playerBehaviour.movement.BoostTimer > 0)
        {
            playerBehaviour.movement.BoostTimer--;
            if (playerBehaviour.movement.BoostTimer <= 0)
            {
                playerBehaviour.movement.MaxSpeed = playerBehaviour.speedStats.TopSpeed;
            }
        }
    }

    /// <summary>
    /// Comment not Current
    /// This method should only be called from inside the Boost method
    /// Boost should call this method when Boost is attempted while DriftState
    /// </summary>
    protected virtual void BoostChain()
    {
        playerBehaviour.movement.Speed *= playerBehaviour.speedStats.BoostChainModifier;
    }
}