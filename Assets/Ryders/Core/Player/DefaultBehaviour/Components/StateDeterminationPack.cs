using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    public abstract class StateDeterminationPack : MonoBehaviour
    {
        // TODO Implement StateDeterminationPack
        public PlayerBehaviour playerBehaviour;

        public void Start()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        public virtual void DetermineTranslationState()
        {
            // Change State to stationary when the players Speed is 0
            if (playerBehaviour.movement.Speed == 0)
            {
                playerBehaviour.movement.TranslationState = TranslationStates.Stationary;
            }
            // Only enter go to EnterBoost when player is not Boosting
            if (playerBehaviour.inputPlayer.GetInputContainer().Boost && playerBehaviour.movement.TranslationState != TranslationStates.Boosting)
            {
                // EnteredBoost State is responsible for entering a Boost
                playerBehaviour.movement.TranslationState = TranslationStates.EnteredBoost;
            }
        }
    }
}
