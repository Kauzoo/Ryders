using UnityEngine;
using Nyr.UnityDev.Component;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    /**
     *  Notes for Cornering:
     *  
     */
    [RequireComponent(typeof(PlayerBehaviour))]
    public abstract class CorneringPack : MonoBehaviour, IRydersPlayerComponent
    {
        // TODO Look into SpeedHandlingMultiplier and TurnLowSpeedMultiplier
        // TODO All of this is ugly and needs to be reworked completely
        protected PlayerBehaviour playerBehaviour;

        private void Start()
        {
            Setup();
        }

        public virtual void Setup()
        {
            GetComponentSafe.SafeGetComponent(this, ref playerBehaviour);
        }

        public virtual void Master()
        {
            DetermineCorneringState();
            CalculateTurning();
        }

        /// <summary>
        /// Determines CorneringState
        /// Also resets TurnRate if needed
        /// </summary>
        protected virtual void DetermineCorneringState()
        {
            if ((playerBehaviour.movement.DriftState == DriftStates.None) &&
                playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis != 0)
            {
                var tempCorneringState =
                    playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis < 0
                        ? CorneringStates.CorneringL
                        : CorneringStates.CorneringR;
                // Reset TurnRate to 0 if the player changes Direction while turning
                if (tempCorneringState != playerBehaviour.movement.CorneringState)
                {
                    playerBehaviour.movement.Turning = 0;
                }

                playerBehaviour.movement.CorneringState = tempCorneringState;
            }
            else
            {
                playerBehaviour.movement.Turning = 0;
                playerBehaviour.movement.CorneringState = CorneringStates.None;
            }
        }

        protected virtual void CalculateTurning()
        {
            // TODO Factor in SpeedHandlingMultiplier and TurnLowSpeedMultiplier
            var turnAccelerationRaw = playerBehaviour.turnStats.Turnrate *
                                      playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis;
            /*var turnAccelerationHandling =
                turnAccelerationRaw * (1 - _playerBehaviour.speedStats.SpeedHandlingMultiplier);*/
            var turnLowSpeed = (playerBehaviour.turnStats.TurnLowSpeedMultiplier /
                                Formula.SpeedToRidersSpeed(playerBehaviour.movement.Speed)) *
                               playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis * 0;
            //var turnRateIntermediate = turnAccelerationHandling + turnLowSpeed;
            var turnRateIntermediate = (turnAccelerationRaw + turnLowSpeed);
            if (turnRateIntermediate < 0)
            {
                playerBehaviour.movement.Turning = Mathf.Min(Mathf.Abs(turnRateIntermediate + playerBehaviour.movement.Turning),
                    playerBehaviour.turnStats.TurnRateMax) * (-1);
            }
            else
            {
                playerBehaviour.movement.Turning = Mathf.Min(turnRateIntermediate + playerBehaviour.movement.Turning,
                    playerBehaviour.turnStats.TurnRateMax);
            }
        }
    }
}