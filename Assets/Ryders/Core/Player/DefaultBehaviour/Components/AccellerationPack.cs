using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine.InputSystem.Controls;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    /**
     *  Notes for Accel/Decel:
     *  -Drifting seems to completly disable all Accel (Decel still applys)
     */
    
    /// <summary>
    /// This Interface contains methods to calculate all kinds of Acceleration and Deceleration
    /// The static implementations act like pseudo functions and contain the actual default logic
    /// The non-static methods that can be overridden if needed all contain a default Implementation
    /// that uses their static Variant
    /// </summary>
    public abstract class AccelerationPack : MonoBehaviour
    {
        private PlayerBehaviour _playerBehaviour;
        private const int JumpChargeTargetSpeed = 80;
        private const int CorneringTargetSpeed = 130;

        private void Start()
        {
            _playerBehaviour = GetComponent<PlayerBehaviour>();
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
        /// This formula uses InGame floats 
        /// </summary>
        /// <param name="speed"> Expects the Speed value from movement</param>
        /// <param name="maxSpeed"> Expects the MaxSpeed value from movement</param>
        /// <returns> Value between 0 and 10</returns>
        public static float StandardDeceleration(float speed, float maxSpeed)
        {
            // Calculate OvermaxSpeed
            var overmaxSpeed = (float)(speed/216) - (float)(maxSpeed/216);
            float speedLossPerFrame = 0f;
            // Do not Decelerate when the player is not over MaxSpeed
            if (overmaxSpeed < 0)
            {
                return speedLossPerFrame;
            }

            // Determine which formula to use based on MaxSpeed and apply
            if (maxSpeed > 200f)
            {
                speedLossPerFrame = (Mathf.Pow((overmaxSpeed / 60f), 2f) + 0.2f) / 100f; // modified
                // speedLossPerFrame = (Mathf.Pow((overmaxSpeed / 60), 2) + 0.2f);
            }
            else
            {
                speedLossPerFrame = (Mathf.Pow((overmaxSpeed / (260.0f - (float)(maxSpeed/216f))), 2f) + 0.2f) / 100f; // modified
                // speedLossPerFrame = (Mathf.Pow((overmaxSpeed / (260 - maxSpeed)), 2) + 0.2f);
            }

            // SpeedLoss is capped at 10 units per frame
            if (speedLossPerFrame > 0.04629f)
            {
                speedLossPerFrame = 10;
            }

            return speedLossPerFrame * 216 * (-1);
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

        public static float JumpChargeDeceleration(float speed, float minSpeed, bool grounded, bool jumpInput)
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

        public virtual void MasterAcceleration()
        {
            // TODO Add other Accelerations and Decelerations
            // TODO Make sure Standards only accelerate / decelerate to MaxSpeed
            SetMaxSpeed();
            _playerBehaviour.movement.Speed += StandardAcceleration() + StandardDeceleration();
        }

        protected virtual float StandardAcceleration()
        {
            return StandardAcceleration(_playerBehaviour.movement.Speed,
                _playerBehaviour.movement.MaxSpeed, _playerBehaviour.speedStats.FastAccelleration,
                _playerBehaviour.speedStats.Acceleration, _playerBehaviour.movement.CorneringState,
                _playerBehaviour.movement.DriftState,
                _playerBehaviour.movement.GroundedState);
        }

        protected virtual float StandardDeceleration()
        {
            return StandardDeceleration(_playerBehaviour.movement.Speed, _playerBehaviour.movement.MaxSpeed);
        }

        protected virtual void SetMaxSpeed()
        {
            // Differentiate between Type 0 and Type 1
            if (_playerBehaviour.movement.MaxSpeedState == MaxSpeedState.Cruising)
            {
                _playerBehaviour.movement.MaxSpeed = _playerBehaviour.speedStats.TopSpeed;
            }
            if (_playerBehaviour.movement.MaxSpeedState == MaxSpeedState.Boosting)
            {
                _playerBehaviour.movement.MaxSpeed = _playerBehaviour.speedStats.BoostSpeed;
            }
            
            // TODO this is terrible. I will think of better way
            float JumpChargeMaxSpeed = JumpCharge();
            float BreakMaxSpeed = Break();
            float OffRoadMaxSpeed = OffRoad();
            float CorneringMaxSpeed = Cornering();
            if (!float.IsPositiveInfinity(JumpChargeMaxSpeed) || !float.IsPositiveInfinity(BreakMaxSpeed) ||
                !float.IsPositiveInfinity(OffRoadMaxSpeed) || !float.IsPositiveInfinity(CorneringMaxSpeed))
            {
                float newMaxSpeed = Mathf.Min(JumpChargeMaxSpeed, BreakMaxSpeed);
                newMaxSpeed = Mathf.Min(newMaxSpeed, OffRoadMaxSpeed);
                newMaxSpeed = Mathf.Min(newMaxSpeed, CorneringMaxSpeed);
                _playerBehaviour.movement.MaxSpeed = newMaxSpeed;
            }
        }
        
        protected virtual float JumpCharge()
        {
            if (_playerBehaviour.inputPlayer.GetInputContainer().Jump && _playerBehaviour.movement.Grounded)
            {
                return JumpChargeTargetSpeed;
            }
            return float.PositiveInfinity;
        }
        
        protected virtual float Cornering()
        {
            // TODO this conrering determination is flawed
            if (_playerBehaviour.movement.CorneringState == CorneringStates.Cornering)
            {
                return CorneringTargetSpeed;
            }
            return float.PositiveInfinity;
        }

        protected virtual float Break()
        {
            if (_playerBehaviour.movement.DriftState == DriftStates.Break)
            {
                return 0;
            }
            return float.PositiveInfinity;
        }

        protected virtual float OffRoad()
        {
            // TODO Implement OffRoad
            return float.PositiveInfinity;
        }
    }
}