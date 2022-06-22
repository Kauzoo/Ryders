using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine.InputSystem.Controls;
using UnityEngine.PlayerLoop;

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

        // Unknown precomputed value. Probably 1 [f1]
        public float TurningSpeedLossMultiplierMul = 1f;
        public float MagicStickPercentageModifier = 0.98f;


        // TODO Look into SpeedHandlingMultiplier and TurnLowSpeedMultiplier
        private void Start()
        {
            _playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        /// <summary>
        /// This includes both Regular as well as FastAccel
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="maxSpeed"></param>
        /// <returns></returns>
        public static float StandardAcceleration(float speed, float maxSpeed, float lowAccel, float mediumAccel,
            float highAccel, float lowThreshold, float mediumThreshold, float offRoadThreshold,
            CorneringStates corneringStates, DriftStates driftStates, GroundedStates groundedStates)
        {
            // Calculate BelowMaxSpeed
            float belowMaxSpeed = maxSpeed - speed;
            float speedGainPerFrame = 0;
            // Do not accelerate if above maxSpeed or not grounded or drifting or cornering
            if (belowMaxSpeed < 0 || groundedStates != GroundedStates.Grounded || driftStates != DriftStates.None ||
                corneringStates != CorneringStates.None)
                return speedGainPerFrame;

            // If speed is below 130, do fast accel, else use regular accel
            if (speed < lowThreshold)
                speedGainPerFrame += lowAccel;

            if (speed < mediumThreshold)
                speedGainPerFrame += mediumAccel;
            else
                speedGainPerFrame += highAccel;
            // TODO Add OffRoad

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
            var overmaxSpeed = Formula.SpeedToRidersSpeed(speed) - Formula.SpeedToRidersSpeed(maxSpeed);
            float speedLossPerFrame = 0f;
            // Do not Decelerate when the player is not over MaxSpeed
            if (overmaxSpeed < 0f)
            {
                return speedLossPerFrame;
            }

            // Determine which formula to use based on MaxSpeed and apply
            if (maxSpeed > 200f)
            {
                speedLossPerFrame = (Mathf.Pow((overmaxSpeed / 60f), 2f) + 0.2f) / 1000f; // modified
                // speedLossPerFrame = (Mathf.Pow((overmaxSpeed / 60), 2) + 0.2f);
            }
            else
            {
                speedLossPerFrame =
                    (Mathf.Pow((overmaxSpeed / (260.0f - Formula.SpeedToRidersSpeed(maxSpeed))), 2f) + 0.2f) /
                    1000f; // modified
                // speedLossPerFrame = (Mathf.Pow((overmaxSpeed / (260 - maxSpeed)), 2) + 0.2f);
            }

            // SpeedLoss is capped at 10 units per frame
            if (speedLossPerFrame > Formula.SpeedToRidersSpeed(10f))
            {
                speedLossPerFrame = 10f;
            }

            speedLossPerFrame = (0.00462963f * speedLossPerFrame) + 0.000925926f;
            return Formula.RidersSpeedToSpeed(speedLossPerFrame) * (-1);
        }

        public static float CorneringDeceleration(float speed, CorneringStates corneringStates)
        {
            // TODO: Implement CorneringDeceleration
            // TODO: TurnSpeedLoss is apparently a property in the OG game, so look into this
            throw new NotImplementedException();
        }

        public static float DriftDeceleration()
        {
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

        public static float OffRoadDeceleration(float speed)
        {
            // TODO: Implement OffRoadDeceleration
            throw new NotImplementedException();
        }

        public virtual void MasterAcceleration()
        {
            // TODO Add other Accelerations and Decelerations
            // TODO Make sure Standards only accelerate / decelerate to MaxSpeed
            SetMaxSpeed();
            _playerBehaviour.movement.Speed += StandardAcceleration() + StandardDeceleration() + TurnSpeedLoss();
        }

        protected virtual float StandardAcceleration()
        {
            return StandardAcceleration(_playerBehaviour.movement.Speed, _playerBehaviour.movement.MaxSpeed,
                _playerBehaviour.speedStats.AccelerationLow, _playerBehaviour.speedStats.AccelerationMedium,
                _playerBehaviour.speedStats.AccelerationHigh, _playerBehaviour.speedStats.AccelerationLowThreshold,
                _playerBehaviour.speedStats.AccelerationMediumThreshold,
                _playerBehaviour.speedStats.AccelerationOffRoadThreshold, _playerBehaviour.movement.CorneringState,
                _playerBehaviour.movement.DriftState, _playerBehaviour.movement.GroundedState);
        }

        protected virtual float StandardDeceleration()
        {
            return StandardDeceleration(_playerBehaviour.movement.Speed, _playerBehaviour.movement.MaxSpeed);
        }

        protected virtual float TurnSpeedLoss()
        {
            float speedLossPerFrame = 0;
            if (IsCornering())
            {
                var TurningSpeedLossMultiplier =
                    (_playerBehaviour.speedStats.TurnSpeedLoss + 0.8f) * TurningSpeedLossMultiplierMul;
                var MagicStickValue =
                    Mathf.Pow(
                        (Formula.SpeedToRidersSpeed(_playerBehaviour.movement.MaxSpeed * MagicStickPercentageModifier)),
                        2.0f) *
                    Mathf.Abs(_playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis);
                var LinearMultiplier = (MagicStickValue / TurningSpeedLossMultiplier) * (-0.000462963f);
                var CubedMultiplier =
                    Mathf.Pow(Mathf.Abs(_playerBehaviour.inputPlayer.GetInputContainer().HorizontalAxis), 3);
                speedLossPerFrame = CubedMultiplier * LinearMultiplier;
                Debug.Log("TurnSpeedLoss (inGame): " + speedLossPerFrame);
                Debug.Log("TurnSpeedLoss (Speedo): " + Formula.RidersSpeedToSpeed(speedLossPerFrame));
            }

            return Formula.RidersSpeedToSpeed(speedLossPerFrame);
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
            if (!float.IsPositiveInfinity(JumpChargeMaxSpeed) || !float.IsPositiveInfinity(BreakMaxSpeed) ||
                !float.IsPositiveInfinity(OffRoadMaxSpeed))
            {
                _playerBehaviour.movement.MaxSpeed = Mathf.Min(JumpChargeMaxSpeed, BreakMaxSpeed, OffRoadMaxSpeed);
            }
        }

        protected virtual float JumpCharge() =>
            (_playerBehaviour.inputPlayer.GetInputContainer().Jump && _playerBehaviour.movement.Grounded)
                ? JumpChargeTargetSpeed
                : float.PositiveInfinity;

        /// <summary>
        /// Determines if the player is Cornering based of the CorneringState. The actual CorneringState determincation
        /// is not done here.
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsCornering() => (_playerBehaviour.movement.CorneringState is (CorneringStates.CorneringL
            or CorneringStates.CorneringR));

        protected virtual float Break() =>
            (_playerBehaviour.movement.DriftState is DriftStates.Break) ? 0f : float.PositiveInfinity;

        protected virtual float OffRoad()
        {
            // TODO Implement OffRoad
            return float.PositiveInfinity;
        }
    }
}