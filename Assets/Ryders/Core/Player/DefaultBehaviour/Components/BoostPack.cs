using System;
using System.Collections;
using System.Collections.Generic;
using Ryders.Core.InputManagement;
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

    /// <summary>
    /// Careful: Changes TranslationState to Boosting
    /// This Method should always be written in such a way that it can only enter BoostState when not currently in
    /// BoostState. It's not responsible for exiting BoostState again. 
    /// </summary>
    public virtual void Boost()
    {
        if (playerBehaviour.inputPlayer.GetInputContainer().Boost &&
            playerBehaviour.movement.TranslationState != TranslationStates.Boosting)
        {
            playerBehaviour.movement.MaxSpeed = playerBehaviour.speedStats.BoostSpeed;
            playerBehaviour.movement.Speed = playerBehaviour.speedStats.BoostSpeed;
            playerBehaviour.movement.BoostTimer = playerBehaviour.speedStats.BoostDuration;
            playerBehaviour.movement.TranslationState = TranslationStates.Boosting;
            if (playerBehaviour.movement.DriftState is DriftStates.DriftingL or DriftStates.DriftingR)
            {
                BoostChain();
            }
        }
    }

    /// <summary>
    /// Responsible for CountingDown the BoostTimer. Only counts down while in BoostState.
    /// If BoostTimer drops below 0 it's reset to Zero.
    /// BoostTimer is also reset to 0 when BoostState is left.
    /// </summary>
    public virtual void CountdownBoostTimer()
    {
        if (playerBehaviour.movement.TranslationState == TranslationStates.Boosting &&
            playerBehaviour.movement.BoostTimer > 0)
        {
            playerBehaviour.movement.BoostTimer--;
        }
        // Reset BoostTimer to 0
        if (playerBehaviour.movement.BoostTimer < 0 || playerBehaviour.movement.TranslationState != TranslationStates.Boosting)
        {
            playerBehaviour.movement.BoostTimer = 0;
        }
    }
    
    public virtual void BoostChain()
    {
        playerBehaviour.movement.Speed *= playerBehaviour.speedStats.BoostChainModifier;
    }
}
