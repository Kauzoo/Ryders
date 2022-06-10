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
    public abstract class DriftPack : MonoBehaviour
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
        private const float DriftInputThreshold = 0;
        public PlayerBehaviour playerBehaviour;

        private void Start()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        /// <summary>
        /// Call this method for all Drift or Break related things
        /// Determines everything related to entering / staying in / exiting Drift / Break-State
        /// </summary>
        public virtual void MasterDrift()
        {
            // TODO this logic can probably be refined
            // Can not Drift while Airborne
            if (playerBehaviour.movement.Grounded)
            {
                // Exit Drift / Break State and set DriftTimer to 0
                playerBehaviour.movement.DriftState = DriftStates.None;
                playerBehaviour.movement.DriftTimer = 0;
                return;
            }
            // TODO HardBonks
            // TODO Getting Attacked
            // TODO Attacking
            // TODO QTE / ControlLock

            // Behaviour primarily depends on if Drift is held or not
            // Entering / Staying in Drift or Break (if DriftInput is held)
            if (playerBehaviour.inputPlayer.GetInputContainer().Drift)
            {
                // ENTER_DRIFT: If Axis is greater 0 Drift to the right 
                // CONTINUE_DRIFT: If in DriftRight State and still holding Drift, continue DriftRight
                if (playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis > DriftInputThreshold ||
                    playerBehaviour.movement.DriftState == DriftStates.DriftingR)
                {
                    DriftRight();
                    return;
                }

                // ENTER_DRIFT: If Axis less than DriftInputThreshold Drift to the left
                // CONTINUE_DRIFT: If in DriftLeft State and still holding Drift, continue DriftLeft
                if (playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis < DriftInputThreshold * (-1) ||
                    playerBehaviour.movement.DriftState == DriftStates.DriftingL)
                {
                    DriftLeft();
                    return;
                }

                // Otherwise Break
                if (MathF.Abs(playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis) < DriftInputThreshold ||
                    playerBehaviour.movement.DriftState == DriftStates.Break)
                {
                    Break();
                    return;
                }

                throw new RydersDriftException("@MasterDrift(): This state should be unreachable");
            }
            // EXIT_DRIFT (if DriftInput is released)
            else
            {
                switch (playerBehaviour.movement.DriftState)
                {
                    case DriftStates.DriftingL or DriftStates.DriftingR:
                        ReleaseDrift();
                        return;
                    case DriftStates.Break:
                        ReleaseBreak();
                        return;
                    default:
                        playerBehaviour.movement.DriftState = DriftStates.None;
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
            playerBehaviour.movement.DriftState = DriftStates.DriftingL;
            playerBehaviour.movement.DriftTimer++;
        }

        /// <summary>
        /// Stay in DriftRight State and increment DriftTimer
        /// </summary>
        protected virtual void DriftRight()
        {
            playerBehaviour.movement.DriftState = DriftStates.DriftingR;
            playerBehaviour.movement.DriftTimer++;
        }

        /// <summary>
        /// This method is meant for manual Drift release only (i.e. letting go off DriftInput)
        /// This is not meant for exiting DriftState via Bonking / Airborne
        /// </summary>
        protected virtual void ReleaseDrift()
        {
            playerBehaviour.movement.DriftState = DriftStates.None;
            // If DriftTimer >= DriftDashFrames the DriftDash has been sufficiently charged
            if (playerBehaviour.speedStats.DriftDashFrames <= playerBehaviour.movement.DriftTimer)
            {
                DriftDash();
            }
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
            if (playerBehaviour.movement.Speed < playerBehaviour.speedStats.DriftCap)
            {
                playerBehaviour.movement.Speed += playerBehaviour.speedStats.DriftDashSpeed;
                // Can not gain Speed beyond the DriftCap
                if (playerBehaviour.movement.Speed > playerBehaviour.speedStats.DriftCap)
                {
                    playerBehaviour.movement.Speed = playerBehaviour.speedStats.DriftCap;
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
            playerBehaviour.movement.DriftState = DriftStates.Break;
            playerBehaviour.movement.Speed -= playerBehaviour.speedStats.BreakeDecelleration;
            if (playerBehaviour.movement.Speed < 0)
            {
                playerBehaviour.movement.Speed = 0;
            }
        }

        /// <summary>
        /// Exit BreakState
        /// </summary>
        protected virtual void ReleaseBreak()
        {
            playerBehaviour.movement.DriftState = DriftStates.None;
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