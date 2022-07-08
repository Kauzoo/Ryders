using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    /// <summary>
    /// Methods in this class are responsible for entering / exiting Drift / Break State as well as DriftDash
    /// and DriftTimer (including applying SpeedBoost / BreakDeceleration), but does not handle any of the Cornering
    /// behaviour associated with drifting
    /// </summary>
    public abstract class DriftPack : RydersPlayerEventPublisher, IRydersPlayerComponent
    {
        /*
            List of things that should end DriftState
            a) Letting go of Drift (will trigger Dash if grounded)
            b) Becoming Airborne (including all kind of ramps)
            c) Hard Bonks (MostWalls, Cars | smaller Bonks don't matter)
            d) Getting Attacked
            e) Attacking yourself
            f) QTEs / ControlLock
            Only a) will cause a DriftDash
         */
        // TODO Bonking should knock the player from DriftState into Breaking without releasing charge
        
        // TODO Implement DriftMomentum
        // How much your momentum follows you during a drift.
        // (Basically how much your current angle and velocity affects the drift by decreasing
        // how much you can turn. Higher = turn less).
        
        private const float DriftInputThreshold = 0;
        [SerializeReference] protected PlayerBehaviour _playerBehaviour;

        private void Start()
        {
            Setup();
        }
        
        public virtual void Setup()
        {
            if (TryGetComponent<PlayerBehaviour>(out var pb))
                _playerBehaviour = pb;
            else
                Debug.LogError("Missing PlayerBehaviour", this);
        }

        public virtual void Master()
        {
            MasterDrift();
            MasterDriftTurn();
        }

        /// <summary>
        /// Call this method for all Drift or Break related things
        /// Determines everything related to entering / staying in / exiting Drift / Break-State
        /// </summary>
        protected virtual void MasterDrift()
        {
            // TODO this logic can probably be refined
            // Can not Drift while Airborne
            if (!_playerBehaviour.movement.Grounded)
            {
                // Exit Drift / Break State and set DriftTimer to 0
                _playerBehaviour.movement.DriftState = DriftStates.None;
                _playerBehaviour.movement.DriftTimer = 0;
                return;
            }
            // TODO HardBonks
            // TODO Getting Attacked
            // TODO Attacking
            // TODO QTE / ControlLock
            
            // TODO CleanUp Logic
            // Behaviour primarily depends on if Drift is held or not
            // Entering / Staying in Drift or Break (if DriftInput is held)
            if (_playerBehaviour.inputPlayer.GetInputContainer().Drift)
            {
                // ENTER_DRIFT: If Axis is greater 0 Drift to the right 
                // CONTINUE_DRIFT: If in DriftRight State and still holding Drift, continue DriftRight
                if ((_playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis > DriftInputThreshold ||
                    _playerBehaviour.movement.DriftState == DriftStates.DriftingR) &&
                    _playerBehaviour.movement.DriftState is not (DriftStates.Break or DriftStates.DriftingL))
                {
                    DriftRight();
                    return;
                }

                // ENTER_DRIFT: If Axis less than DriftInputThreshold Drift to the left
                // CONTINUE_DRIFT: If in DriftLeft State and still holding Drift, continue DriftLeft
                if ((_playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis < DriftInputThreshold * (-1) ||
                    _playerBehaviour.movement.DriftState == DriftStates.DriftingL) &&
                    _playerBehaviour.movement.DriftState is not (DriftStates.Break or DriftStates.DriftingR))
                {
                    DriftLeft();
                    return;
                }

                // Otherwise Break
                if (MathF.Abs(_playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis) <= DriftInputThreshold ||
                    _playerBehaviour.movement.DriftState == DriftStates.Break)
                {
                    Break();
                    return;
                }

                throw new RydersDriftException("@MasterDrift(): This state should be unreachable");
            }
            // EXIT_DRIFT (if DriftInput is released)
            else
            {
                switch (_playerBehaviour.movement.DriftState)
                {
                    case DriftStates.DriftingL or DriftStates.DriftingR:
                        ReleaseDrift();
                        return;
                    case DriftStates.Break:
                        ReleaseBreak();
                        return;
                    default:
                        _playerBehaviour.movement.DriftState = DriftStates.None;
                        break;
                }
            }
        }

        #region Drift

        /// <summary>
        /// Stay in DriftLeft State and increment DriftTimer
        /// </summary>
        protected virtual void DriftLeft()
        {
            _playerBehaviour.movement.DriftState = DriftStates.DriftingL;
            _playerBehaviour.movement.DriftTimer++;
        }

        /// <summary>
        /// Stay in DriftRight State and increment DriftTimer
        /// </summary>
        protected virtual void DriftRight()
        {
            _playerBehaviour.movement.DriftState = DriftStates.DriftingR;
            _playerBehaviour.movement.DriftTimer++;
        }

        /// <summary>
        /// This method is meant for manual Drift release only (i.e. letting go off DriftInput)
        /// This is not meant for exiting DriftState via Bonking / Airborne
        /// </summary>
        protected virtual void ReleaseDrift()
        {
            _playerBehaviour.movement.DriftState = DriftStates.None;
            // If DriftTimer >= DriftDashFrames the DriftDash has been sufficiently charged
            if (_playerBehaviour.speedStats.DriftDashFrames <= _playerBehaviour.movement.DriftTimer)
            {
                DriftDash();
            }
            // Reset DriftTimer
            _playerBehaviour.movement.DriftTimer = 0;
        }

        // TODO Implement a static function style version
        /// <summary>
        /// Apply the Speed gain from a Drift Dash
        /// This should only be called from inside ReleaseDrift by default since it does not check for
        /// eligibility itself 
        /// </summary>
        protected virtual void DriftDash()
        {
            // TODO speed probably shouldn't be modified directly
            // Can not gain any Speed if already moving faster than DriftCap (but also wont lose Speed)
            if (_playerBehaviour.movement.Speed < _playerBehaviour.speedStats.DriftCap)
            {
                RaiseSpeedBoostEvent(EventArgs.Empty);
                _playerBehaviour.movement.Speed += _playerBehaviour.speedStats.DriftDashSpeed;
                // Can not gain Speed beyond the DriftCap
                if (_playerBehaviour.movement.Speed > _playerBehaviour.speedStats.DriftCap)
                {
                    _playerBehaviour.movement.Speed = _playerBehaviour.speedStats.DriftCap;
                }
            }
        }

        #endregion

        #region Break
        /// <summary>
        /// Enter / stay in BreakState and decelerate
        /// </summary>
        protected virtual void Break()
        {
            // TODO potential interactions with automatic accel
            // TODO special interaction for boost while breaking
            _playerBehaviour.movement.DriftState = DriftStates.Break;
            //playerBehaviour.movement.Speed -= playerBehaviour.speedStats.BreakeDecelleration;
            if (_playerBehaviour.movement.Speed < 0)
            {
                _playerBehaviour.movement.Speed = 0;
            }
        }

        /// <summary>
        /// Exit BreakState
        /// </summary>
        protected virtual void ReleaseBreak()
        {
            _playerBehaviour.movement.DriftState = DriftStates.None;
        }

        #endregion

        #region DriftTurning
        /// <summary>
        /// 
        /// </summary>
        protected virtual void MasterDriftTurn()
        {
            switch (_playerBehaviour.movement.DriftState)
            {
                case DriftStates.DriftingL:
                    DriftTurnLeft();
                    break;
                case DriftStates.DriftingR:
                    DriftTurnRight();
                    break;
                case DriftStates.None:
                case DriftStates.Break:
                default:
                    return;
            }
        }

        protected virtual void DriftTurnLeft()
        {
            // STEP 0: Calculate new additive offset
            var driftTurnOffset = _playerBehaviour.turnStats.DriftTurnrate *
                                  _playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis;
            // STEP 1: Add offset
            _playerBehaviour.movement.DriftTurning += driftTurnOffset;
            // STEP 2: Makes abs for Min / Max evaluations
            var driftTurningAbs = Mathf.Abs(_playerBehaviour.movement.DriftTurning);
            // STEP 3: Make sure the absolute value of the DriftTurning doesn't exceed the DriftTurnRateMax
            driftTurningAbs = Mathf.Min(driftTurningAbs,
                _playerBehaviour.turnStats.DriftTurnrateMax);
            // STEP 4: Make sure the absolute value of the DriftTurning doesn't underflow the DriftTurnRateMin
            // STEP 5: Multiply with (-1) again because TurningLeft
            _playerBehaviour.movement.DriftTurning = Mathf.Max(driftTurningAbs,
                _playerBehaviour.turnStats.DriftTurnrateMin) * (-1);
        }

        protected virtual void DriftTurnRight()
        {
            // STEP 0: Calculate new additive offset
            var driftTurnOffset = _playerBehaviour.turnStats.DriftTurnrate *
                                  _playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis;
            // STEP 1: Add offset
            _playerBehaviour.movement.DriftTurning += driftTurnOffset;
            // STEP 2: Make sure the absolute value of the DriftTurning doesn't exceed the DriftTurnRateMax
            _playerBehaviour.movement.DriftTurning = Mathf.Min(_playerBehaviour.movement.DriftTurning,
                _playerBehaviour.turnStats.DriftTurnrateMax);
            // STEP 3: Make sure the absolute value of the DriftTurning doesn't underflow the DriftTurnRateMin
            _playerBehaviour.movement.DriftTurning = Mathf.Max(_playerBehaviour.movement.DriftTurning,
                _playerBehaviour.turnStats.DriftTurnrateMin);
        }
        #endregion
    }

    /// <summary>
    /// Use this Exceptions that a Problem related to Drift in Ryders.Core occured
    /// </summary>
    [Serializable]
    public class RydersDriftException : Exception
    {
        public RydersDriftException()
        {
        }

        public RydersDriftException(string message)
            : base(message)
        {
        }
    }
}