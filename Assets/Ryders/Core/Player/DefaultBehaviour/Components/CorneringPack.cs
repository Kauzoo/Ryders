using UnityEngine;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    /**
     *  Notes for Cornering:
     *  
     */
    public abstract class CorneringPack : MonoBehaviour
    {
        // TODO Look into SpeedHandlingMultiplier and TurnLowSpeedMultiplier
        private PlayerBehaviour _playerBehaviour;

        private void Start()
        {
            _playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        public virtual void MasterCornering()
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
            if ((_playerBehaviour.movement.DriftState == DriftStates.None) && 
                _playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis != 0)
            {
                var tempCorneringState =
                    _playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis < 0
                        ? CorneringStates.CorneringL
                        : CorneringStates.CorneringR;
                // Reset TurnRate to 0 if the player changes Direction while turning
                if (tempCorneringState != _playerBehaviour.movement.CorneringState)
                {
                    _playerBehaviour.movement.Turning = 0;
                }
                _playerBehaviour.movement.CorneringState = tempCorneringState;
            }
            else
            {
                _playerBehaviour.movement.Turning = 0;
                _playerBehaviour.movement.CorneringState = CorneringStates.None;
            }
        }

        protected virtual void CalculateTurning()
        {
            var turnRateIntermediate = _playerBehaviour.movement.Turning + _playerBehaviour.turnStats.Turnrate *
                                 _playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis;
            if (turnRateIntermediate < 0)
            {
                _playerBehaviour.movement.Turning = Mathf.Min(Mathf.Abs(turnRateIntermediate),
                    _playerBehaviour.turnStats.TurnRateMax) * (-1);
            }
            else
            {
                _playerBehaviour.movement.Turning = Mathf.Min(turnRateIntermediate,
                    _playerBehaviour.turnStats.TurnRateMax);
            }
        }
    }
}