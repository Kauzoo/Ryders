using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ryders.Core.Player.ExtremeGear.Movement;

namespace Ryders.Core.Player.ExtremeGear.Movement
{
    /// <summary>
    /// This class contains all Methods to calculate all kinds of Accelleration and Deceleration
    /// All methods are virtual
    /// </summary>
    public class AccellerationPack
    {
        /// <summary>
        /// This includes both Regular as well as FastAccel
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="maxSpeed"></param>
        /// <returns></returns>
        public virtual float StandardAcceleration(float speed, float maxSpeed, float fastAccel, float regAccel,
            CorneringStates corneringStates, DriftStates driftStates, GroundedStates groundedStates)
        {
            // Calculate BelowMaxSpeed
            float belowMaxSpeed = maxSpeed - speed;
            float speedGainPerFrame = 0;
            // Do not accelerate if above maxSpeed or not grounded or drifting or cornering
            if (belowMaxSpeed < 0 || groundedStates != GroundedStates.Grounded || driftStates != DriftStates.None ||
                corneringStates != CorneringStates.None)
            {
                return speedGainPerFrame;
            }

            // If speed is below 130, do fast accel, else use regular accel
            if (speed < 130)
            {
                speedGainPerFrame += fastAccel;
            }
            else
            {
                speedGainPerFrame += regAccel;
            }

            return speedGainPerFrame;
        }

        public virtual float DownhillAccelleration()
        {
            // TODO: Implement DownhillDeceleration
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calcualte the current Decel using Formula from SRDX.
        /// StandardDeceleration is always applied when the player is above MaxSpeed
        /// </summary>
        /// <param name="speed"> Expects the Speed value from movement</param>
        /// <param name="maxSpeed"> Expects the MaxSpeed value from movement</param>
        /// <returns> Value between 0 and 10</returns>
        public virtual float StandardDeceleration(float speed, float maxSpeed)
        {
            // Calculate OvermaxSpeed
            float overmaxSpeed = speed - maxSpeed;
            float speedLossPerFrame = 0;
            // Do not Decelerate when the player is not over MaxSpeed
            if (overmaxSpeed < 0)
            {
                return speedLossPerFrame;
            }

            // Determine which formula to use based on MaxSpeed and apply
            if (maxSpeed > 200)
            {
                speedLossPerFrame = (Mathf.Pow((overmaxSpeed / 60), 2) + 0.2f) / 1000;
            }
            else
            {
                speedLossPerFrame = (Mathf.Pow((overmaxSpeed / (260 - maxSpeed)), 2) + 0.2f) / 1000;
            }

            // SpeedLoss is capped at 10 units per frame
            if (speedLossPerFrame > 10)
            {
                speedLossPerFrame = 10;
            }

            return speedLossPerFrame;
        }

        public virtual float CorneringDeceleration(float speed, CorneringStates corneringStates)
        {
            // TODO: Implement CorneringDeceleration
            throw new NotImplementedException();
        }

        public virtual float BreakingDeceleration(float speed)
        {
            // TODO: Implement BreakingDeceleration
            throw new NotImplementedException();
        }

        public virtual float JumpChargeDeceleration(float speed)
        {
            // TODO: Implement JumpChargeDeceleration
            throw new NotImplementedException();
        }

        public virtual float UphillDeceleration(float speed)
        {
            // TODO: Implement UphillDeceleration
            throw new NotImplementedException();
        }

        public virtual float OffroadDeceleration(float speed)
        {
            // TODO: Implement OffroadDeceleration
            throw new NotImplementedException();
        }
    }
}
