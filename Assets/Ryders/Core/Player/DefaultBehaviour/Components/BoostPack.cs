using System;
using UnityEngine;
using Nyr.UnityDev.Component;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    /**
 *  Notes for Boost:
 *  -Sets MaxSpeed to BoostSpeed for the Duration of Boost (this is done at low priority)
 *  -Speed is set to BoostSpeed on the frame of use
 *  -
 */

    /// <summary>
    /// Contains Method for entering a regular Boost, BoostChaining and CountingDown the BoostTimer
    /// "BoostState" is based of the BoostTimer. BoostState basically comes down to MaxSpeed.
    /// A Boost is Reset when the Timer is reset to 0, but IMPORTANT, whenever the BoostTimer is reset to 0
    /// the MaxSpeed is also reset it CruisingValue 
    /// </summary>
    [RequireComponent(typeof(PlayerBehaviour))]
    public abstract class BoostPack : RydersPlayerEventPublisher, IRydersPlayerComponent
    {
        protected PlayerBehaviour playerBehaviour;

        public void Start()
        {
            Setup();
        }
        
        public virtual void Setup()
        {
            GetComponentSafe.SafeGetComponent(this, ref playerBehaviour);
        }

        public virtual void Master()
        {
            DetermineBoostState();
            Boost();
        }

        /// <summary>
        /// Comment Not Current
        /// Careful: Changes TranslationState to Boosting
        /// This Method should always be written in such a way that it can only enter BoostState when not currently in
        /// BoostState. It's not responsible for exiting BoostState again.
        /// Behaviour depends on BoostInput, TranslationState as well as DriftState 
        /// </summary>
        protected virtual void Boost()
        {
            if (playerBehaviour.inputPlayer.GetInputContainer().Boost &&
                playerBehaviour.movement.MaxSpeedState != MaxSpeedState.Boosting)
            {
                RaiseSpeedBoostEvent(EventArgs.Empty);
                playerBehaviour.movement.MaxSpeedState = MaxSpeedState.Boosting;
                //playerBehaviour.movement.MaxSpeed = playerBehaviour.speedStats.BoostSpeed;
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
        protected virtual void DetermineBoostState()
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
                playerBehaviour.movement.MaxSpeedState = MaxSpeedState.Cruising;
                //playerBehaviour.movement.MaxSpeed = playerBehaviour.speedStats.TopSpeed;
            }
            // TODO Cleanup the MaxSpeed Reset
            // If the BoostTimer is greater 0 count down
            if (playerBehaviour.movement.BoostTimer > 0)
            {
                playerBehaviour.movement.BoostTimer--;
                if (playerBehaviour.movement.BoostTimer <= 0)
                {
                    // When the BoostTimer reaches 0 the Boost should end and the MaxSpeedState is set to cruising
                    playerBehaviour.movement.MaxSpeedState = MaxSpeedState.Cruising;
                    //playerBehaviour.movement.MaxSpeed = playerBehaviour.speedStats.TopSpeed;
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
            playerBehaviour.movement.Speed += playerBehaviour.movement.Speed * playerBehaviour.speedStats.BoostChainModifier;
        }
    }
}