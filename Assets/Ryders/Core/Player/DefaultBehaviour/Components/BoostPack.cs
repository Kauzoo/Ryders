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

    public virtual void BoostTimer()
    {
        if (playerBehaviour.movement.TranslationState == TranslationStates.Boosting && playerBehaviour.movement.BoostTimer > 0)
        {
            playerBehaviour.movement.BoostTimer--;
        }
    }
    
    public virtual void BoostChain()
    {
        playerBehaviour.movement.Speed *= playerBehaviour.speedStats.BoostChainModifier;
    }
}
