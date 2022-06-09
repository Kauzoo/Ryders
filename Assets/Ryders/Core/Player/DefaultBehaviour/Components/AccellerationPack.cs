using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    /// <summary>
    /// This Interface contains methods to calculate all kinds of Acceleration and Deceleration
    /// The static implementations act like pseudo functions and contain the actual default logic
    /// The non-static methods that can be overridden if needed all contain a default Implementation
    /// that uses their static Variant
    /// </summary>
    public abstract class AccelerationPack : MonoBehaviour
    {
        protected PlayerBehaviour playerBehaviour;

        private void Start()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }
        
        // TODO Add in MediumAccel
        /// <summary>
        /// This includes both Regular as well as FastAccel
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="maxSpeed"></param>
        /// <returns></returns>
        public static float StandardAcceleration(float speed, float maxSpeed, float fastAccel, float regAccel,
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
        
        public static float DownhillAcceleration()
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
        public static float StandardDeceleration(float speed, float maxSpeed)
        {
            // Calculate OvermaxSpeed
            var overmaxSpeed = speed - maxSpeed;
            float speedLossPerFrame = 0;
            // Do not Decelerate when the player is not over MaxSpeed
            if (overmaxSpeed < 0)
            {
                return speedLossPerFrame;
            }

            // Determine which formula to use based on MaxSpeed and apply
            if (maxSpeed > 200)
            {
                // speedLossPerFrame = (Mathf.Pow((overmaxSpeed / 60), 2) + 0.2f) / 1000; OG SRDX
                speedLossPerFrame = (Mathf.Pow((overmaxSpeed / 60), 2) + 0.2f) / 10;
            }
            else
            {
                //speedLossPerFrame = (Mathf.Pow((overmaxSpeed / (260 - maxSpeed)), 2) + 0.2f) / 1000; OG SRDX
                speedLossPerFrame = (Mathf.Pow((overmaxSpeed / (260 - maxSpeed)), 2) + 0.2f) / 10;
            }

            // SpeedLoss is capped at 10 units per frame
            if (speedLossPerFrame > 10)
            {
                speedLossPerFrame = 10;
            }
            Debug.Log("SpeedLossPerFrame:" + speedLossPerFrame);
            return speedLossPerFrame * (-1);
        }
        
        public static float CorneringDeceleration(float speed, CorneringStates corneringStates)
        {
            // TODO: Implement CorneringDeceleration
            throw new NotImplementedException();
        }
        
        public static float BreakingDeceleration(float speed)
        {
            // TODO: Implement BreakingDeceleration
            throw new NotImplementedException();
        }
        
        public static float JumpChargeDeceleration(float speed)
        {
            // TODO: Implement JumpChargeDeceleration
            throw new NotImplementedException();
        }
        
        public static float UphillDeceleration(float speed)
        {
            // TODO: Implement UphillDeceleration
            throw new NotImplementedException();
        }

        public static float OffroadDeceleration(float speed)
        {
            // TODO: Implement OffroadDeceleration
            throw new NotImplementedException();
        }
        
        public virtual float StandardAcceleration()
        {
            return StandardAcceleration(playerBehaviour.movement.Speed, playerBehaviour.movement.MaxSpeed, playerBehaviour.speedStats.FastAccelleration,
                playerBehaviour.speedStats.Acceleration, playerBehaviour.movement.CorneringState, playerBehaviour.movement.DriftState,
                playerBehaviour.movement.GroundedState);
        }
        
        public virtual float StandardDeceleration()
        {
            return StandardDeceleration(playerBehaviour.movement.Speed, playerBehaviour.movement.MaxSpeed);
        }
    }
}


