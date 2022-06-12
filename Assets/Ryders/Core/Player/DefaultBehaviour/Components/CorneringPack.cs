using UnityEngine;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    /**
     *  Notes for Cornering:
     *  -How do I determine if I am Cornering?
     */
    public abstract class CorneringPack : MonoBehaviour
    {
        // TODO Implement CorneringPack
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
        /// Determines CorneringState for the purposes of Decel
        /// </summary>
        protected virtual void DetermineCorneringState()
        {
            if ((_playerBehaviour.movement.DriftState == DriftStates.None) &&
                _playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis != 0)
            {
                _playerBehaviour.movement.CorneringState = CorneringStates.Cornering;
            }
            else
            {
                _playerBehaviour.movement.CorneringState = CorneringStates.None;
            }
        }

        protected virtual void CalculateTurning()
        {
            _playerBehaviour.movement.TurningRaw = _playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis;
            _playerBehaviour.movement.Turning =
                _playerBehaviour.movement.TurningRaw * _playerBehaviour.turnStats.Turnrate;
        }
    }
}