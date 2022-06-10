using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    public abstract class DriftPack : MonoBehaviour
    {
        // TODO Implement DriftPack
        private const float DriftInputThreshold = 0;
        public PlayerBehaviour playerBehaviour;
        
        private void Start()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        public virtual void MasterDrift()
        {
            if (playerBehaviour.inputPlayer.GetInputContainer().Drift && playerBehaviour.movement.DriftTimer == 0)
            {
                // If Axis is greater 0 Drift to the right 
                if (playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis > DriftInputThreshold)
                {
                    DriftRight();
                    return;
                }
                // If Axis less than DriftInputThreshold Drift to the left
                if (playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis < DriftInputThreshold * (-1))
                {
                    DriftLeft();
                    return;
                }
                // Otherwise Break
                if (MathF.Abs(playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis) < DriftInputThreshold)
                {
                    Break();
                }
            }
        }
        
        protected virtual void DriftLeft()
        {
            playerBehaviour.movement.DriftState = DriftStates.DriftingL;
            
        }

        protected virtual void DriftRight()
        {
            
        }

        protected virtual void ReleaseDrift()
        {
            
        }

        protected virtual void Break()
        {
            
        }
    }
}
