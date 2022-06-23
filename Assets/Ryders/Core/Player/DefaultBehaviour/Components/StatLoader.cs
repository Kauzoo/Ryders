using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ryders.Core.InputManagement;
using Ryders.Core.Player.Character;
using Ryders.Core.Player.DefaultBehaviour;
using Ryders.Core.Player.ExtremeGear;
using UnityEngine;
using UnityEngine.Serialization;


namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    public abstract class StatLoaderPack : MonoBehaviour
    {
        /*
        * NOTES
        * Apparently this is
         * The math is:
            (TypeTopSpeedAtLevel * CharacterTopSpeedMultiplier) + GearTopSpeedBonus

            It just so happens I make TypeTopSpeedAtLevel increase by 13 per level.
            for the calc of the CharacterTopSpeed (offset) (Source: AirKingNeo)
            TypeTopSpeedAtLevel is apparently 216 (Source: AirKingNeo)
         */

        // TODO Implement this in a more sensible way
        /**
         *  HIDDEN GLOBAL STATS
         */
        private const int FlyTypeTopSpeedLoss = -7;

        private const int PowerTypeSpeedLoss = -4;
        private const int GlobalBoostDuration = 120; // BaseBoostDuration if Frames

        [FormerlySerializedAs("playerBehaviour")]
        public PlayerBehaviour _playerBehaviour;

        // TODO Implement FastAccel for SpeedTypes and Off-Road resistance for PowerType
        // TODO CleanUp which stat is retrieved from where

        public virtual void Setup()
        {
            _playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        public virtual void LoadStatsMaster()
        {
            // SPEED STATS
            LoadTopSpeed();
            // ACCEL
            LoadAccelerationLow();
            LoadAccelerationMedium();
            LoadAccelerationHigh();
            LoadAccelerationLowThreshold();
            LoadAccelerationMediumThreshold();
            LoadAccelerationOffRoadThreshold();
            // BOOST
            LoadBoostSpeed();
            LoadBoostChainModifier();
            LoadBoostDuration();
            // DRIFT
            LoadBreakeDecelleration();
            LoadDriftDashSpeed();
            LoadDriftCap();
            LoadDriftDashFrames();
            LoadTurnSpeedLoss();
            LoadJumpChargeMinSpeed();
            LoadJumpChargeDeceleration();
            // TURNING
            LoadTurnRate();
            LoadTurnRateMax();
            LoadTurnSpeedLossCurve();
            LoadTurnRateCurve();
            LoadDriftTurnRateMin();
            LoadDriftTurnRateMax();
            LoadDriftTurnRate();
            // JUMP
            LoadJumpSpeedMax();
            LoadJumpAcceleration();
            // FUEL
            LoadFuelType();
            LoadJumpChargeMultiplier();
            LoadTrickFuelGain();
            LoadTypeFuelGain();
            LoadQTEFuelGain();
            LoadPassiveDrain();
            LoadTankSize();
            LoadBoostCost();
            LoadDriftCost();
            LoadTornadoCost();
        }

        #region StaticMethods

        /// <summary>
        /// Note: Since this involves a LvLUp Stat the LvL is reduced by one
        /// </summary>
        /// <param name="level"></param>
        /// <param name="defaultTopSpeedLevelUp"></param>
        /// <param name="characterTopSpeed"></param>
        /// <param name="gearTopSpeed"></param>
        /// <returns></returns>
        public static float LoadTopSpeed(int level, float defaultTopSpeedLevelUp, float characterTopSpeed,
            float gearTopSpeed, CharacterType statsType)
        {
            return statsType switch
            {
                CharacterType.Speed => (defaultTopSpeedLevelUp * (level - 1)) + characterTopSpeed + gearTopSpeed,
                CharacterType.Fly => (defaultTopSpeedLevelUp * (level - 1)) + characterTopSpeed + gearTopSpeed +
                                     FlyTypeTopSpeedLoss,
                CharacterType.Power => (defaultTopSpeedLevelUp * (level - 1)) + characterTopSpeed + gearTopSpeed +
                                       PowerTypeSpeedLoss,
                _ => throw new System.NotImplementedException("Invalid Level")
            };
        }

        public static float LoadAccelerationLow(int level, float defaultAccelerationLowLvl1,
            float defaultAccelerationLowLvl2, float defaultAccelerationLowLvl3)
        {
            return level switch
            {
                1 => defaultAccelerationLowLvl1,
                2 => defaultAccelerationLowLvl2,
                3 => defaultAccelerationLowLvl3,
                _ => throw new System.NotImplementedException("Invalid Level")
            };
        }

        public static float LoadAccelerationMedium(int level, float defaultAccelerationMediumLvl1,
            float defaultAccelerationMediumLvl2, float defaultAccelerationMediumLvl3)
        {
            return level switch
            {
                1 => defaultAccelerationMediumLvl1,
                2 => defaultAccelerationMediumLvl2,
                3 => defaultAccelerationMediumLvl3,
                _ => throw new System.NotImplementedException("Invalid Level")
            };
        }

        public static float LoadAccelerationHigh(int level, float defaultAccelerationHighLvl1,
            float defaultAccelerationHighLvl2, float defaultAccelerationHighLvl3)
        {
            return level switch
            {
                1 => defaultAccelerationHighLvl1,
                2 => defaultAccelerationHighLvl2,
                3 => defaultAccelerationHighLvl3,
                _ => throw new System.NotImplementedException("Invalid Level")
            };
        }

        public static float LoadAccelerationLowThreshold(int level, float defaultAccelerationLowThresholdLvl1,
            float defaultAccelerationLowThresholdLvl2, float defaultAccelerationLowThresholdLvl3)
        {
            return level switch
            {
                1 => defaultAccelerationLowThresholdLvl1,
                2 => defaultAccelerationLowThresholdLvl2,
                3 => defaultAccelerationLowThresholdLvl3,
                _ => throw new System.NotImplementedException("Invalid Level")
            };
        }

        public static float LoadAccelerationMediumThreshold(int level, float defaultAccelerationMediumThresholdLvl1,
            float defaultAccelerationMediumThresholdLvl2, float defaultAccelerationMediumThresholdLvl3)
        {
            return level switch
            {
                1 => defaultAccelerationMediumThresholdLvl1,
                2 => defaultAccelerationMediumThresholdLvl2,
                3 => defaultAccelerationMediumThresholdLvl3,
                _ => throw new System.NotImplementedException("Invalid Level")
            };
        }

        public static float LoadAccelerationOffRoadThreshold(int level, float defaultAccelerationOffRoadThresholdLvl1,
            float defaultAccelerationOffRoadThresholdLvl2, float defaultAccelerationOffRoadThresholdLvl3)
        {
            return level switch
            {
                1 => defaultAccelerationOffRoadThresholdLvl1,
                2 => defaultAccelerationOffRoadThresholdLvl2,
                3 => defaultAccelerationOffRoadThresholdLvl3,
                _ => throw new System.NotImplementedException("Invalid Level")
            };
        }

        /// <summary>
        /// Affected by Level (ExtremeGear).
        /// BoostSpeed = CharacterData.BoostSpeed + ExtremeGearData.BoostSpeedLvl
        /// <see cref="PlayerBehaviour.BoostSpeed"/>
        /// </summary>
        /// <param name="level"></param>
        public static float LoadBoostSpeed(int level, float characterBoostSpeed, float gearBoostSpeedLvl1,
            float gearBoostSpeedLvl2, float gearBoostSpeedLvl3)
        {
            return level switch
            {
                1 => characterBoostSpeed + gearBoostSpeedLvl1,
                2 => characterBoostSpeed + gearBoostSpeedLvl2,
                3 => characterBoostSpeed + gearBoostSpeedLvl3,
                _ => throw new System.NotImplementedException("Invalid Level")
            };
        }

        /// <summary>
        /// Note: Since this involves a LvLUp Stat the LvL is reduced by one
        /// </summary>
        /// <param name="level"></param>
        /// <param name="defaultBoostDuration"></param>
        /// <param name="characterBoostDurationLvl1"></param>
        /// <param name="characterBoostDurationLvl2"></param>
        /// <param name="characterBoostDurationLvl3"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static float LoadBoostDuration(int level, float defaultBoostDuration, float characterBoostDurationLvl1,
            float characterBoostDurationLvl2, float characterBoostDurationLvl3)
        {
            return level switch
            {
                1 => defaultBoostDuration * (level - 1) + characterBoostDurationLvl1 + GlobalBoostDuration,
                2 => defaultBoostDuration * (level - 1) + characterBoostDurationLvl2 + GlobalBoostDuration,
                3 => defaultBoostDuration * (level - 1) + characterBoostDurationLvl3 + GlobalBoostDuration,
                _ => throw new System.NotImplementedException("Invalid Level")
            };
        }

        /// <summary>
        /// BoostChainModifier = CharacterData.BoostChainModifier + ExtremeGearData.BoostChainModifier
        /// </summary>
        public static float LoadBoostChainModifier(float characterBoostChainModifier, float gearBoostChainModifier)
        {
            return characterBoostChainModifier + gearBoostChainModifier;
        }

        public static float LoadBreakeDecelleration(float defaultBreakeDecelleration)
        {
            return defaultBreakeDecelleration;
        }

        /// <summary>
        /// Affected by Level (ExtremeGear).
        /// DriftDashSpeed = CharacterData.Drift + ExtremeGearData.DriftDashSpeedLvl;
        /// </summary>
        /// <param name="level"></param>
        public static float LoadDriftDashSpeed(int level, float characterDrift, float gearDriftDashSpeedLvl1,
            float gearDriftDashSpeedLvl2, float gearDriftDashSpeedLvl3)
        {
            return level switch
            {
                1 => characterDrift + gearDriftDashSpeedLvl1,
                2 => characterDrift + gearDriftDashSpeedLvl2,
                3 => characterDrift + gearDriftDashSpeedLvl3,
                _ => throw new System.NotImplementedException("Invalid Level")
            };
        }

        /// <summary>
        /// Note: Since this involves a LvLUp Stat the LvL is reduced by one
        /// </summary>
        /// <param name="level"></param>
        /// <param name="defaultDriftCapLevelUp"></param>
        /// <param name="characterDrift"></param>
        /// <param name="gearDriftCap"></param>
        /// <returns></returns>
        public static float LoadDriftCap(int level, float defaultDriftCapLevelUp, float characterDrift,
            float gearDriftCap)
        {
            return (defaultDriftCapLevelUp * (level - 1)) + characterDrift + gearDriftCap;
        }

        public static float LoadDriftDashFrames(float gearDriftDashChargeDuration) => gearDriftDashChargeDuration;

        public static float LoadTurnSpeedLoss(float defaultTurnSpeedLossDefault) => defaultTurnSpeedLossDefault;

        public static float LoadJumpChargeMinSpeed(float defaultJumpChargeMinSpeedDefault) =>
            defaultJumpChargeMinSpeedDefault;

        public static float LoadJumpChargeDecelleration(float defaultJumpChargeDecelerationDefault) =>
            defaultJumpChargeDecelerationDefault;

        // TURNING
        public static float LoadTurnRate(float defaultTurnRateDefault) => defaultTurnRateDefault;

        public static float LoadTurnRateMax(float defaultTurnRateMax) => defaultTurnRateMax;

        public static float LoadTurnLowSpeedMultiplier(float defaultTurnLowSpeedMultiplier) =>
            defaultTurnLowSpeedMultiplier;

        public static AnimationCurve LoadTurnSpeedLossCurve(AnimationCurve defaultTurnSpeedLossCurveDefault) =>
            defaultTurnSpeedLossCurveDefault;

        public static AnimationCurve LoadTurnRateCurve(AnimationCurve defaultTurnrateCurveDefault) =>
            defaultTurnrateCurveDefault;

        public static float LoadDriftTurnratePassive(float defaultDriftTurnrateDefault) => defaultDriftTurnrateDefault;

        public static float LoadDriftTurnRateMin(float defaultDriftTurnrateMinDefault) =>
            defaultDriftTurnrateMinDefault;

        public static float LoadDriftTurnRateMax(float defaultDriftTurnRateMaxDefault) =>
            defaultDriftTurnRateMaxDefault;

        public static float LoadDriftTurnRate(float defaultDriftTurnrateDefault) => defaultDriftTurnrateDefault;

        //Jump
        public static float LoadJumpSpeedMax(float defaultJumpSpeedMaxDefault) => defaultJumpSpeedMaxDefault;

        public static AnimationCurve LoadJumpAcceleration(AnimationCurve defaultJumpAccelDefault) =>
            defaultJumpAccelDefault;

        // FUEL
        public static FuelType LoadFuelType(FuelType gearFuelType) => gearFuelType;

        public static float LoadJumpChargeMultiplier(float gearJumpChargeMultiplier) => gearJumpChargeMultiplier;

        public static float LoadTrickFuelGain(float gearTrickFuelGain) => gearTrickFuelGain;

        public static float LoadTypeFuelGain(float gearTypeFuelGain) => gearTypeFuelGain;

        public static float LoadQTEFuelGain(float gearQTEFuelGain) => gearQTEFuelGain;

        public static float LoadPassiveDrain(int level, float gearPassiveDrainLvl1, float gearPassiveDrainLvl2,
            float gearPassiveDrainLvl3)
        {
            return level switch
            {
                1 => gearPassiveDrainLvl1,
                2 => gearPassiveDrainLvl2,
                3 => gearPassiveDrainLvl3,
                _ => throw new System.NotImplementedException("Invalid Level")
            };
        }

        public static float LoadTankSize(int level, float gearFuelTankSizeLvl1, float gearFuelTankSizeLvl2,
            float gearFuelTankSizeLvl3)
        {
            return level switch
            {
                1 => gearFuelTankSizeLvl1,
                2 => gearFuelTankSizeLvl2,
                3 => gearFuelTankSizeLvl3,
                _ => throw new System.NotImplementedException("Invalid Level")
            };
        }

        public static float LoadBoostCost(int level, float gearBoostCostLvl1, float gearBoostCostLvl2,
            float gearBoostCostLvl3)
        {
            return level switch
            {
                1 => gearBoostCostLvl1,
                2 => gearBoostCostLvl2,
                3 => gearBoostCostLvl3,
                _ => throw new System.NotImplementedException("Invalid Level")
            };
        }

        public static float LoadDriftCost(int level, float gearDriftCostLvl1, float gearDriftCostLvl2,
            float gearDriftCostLvl3)
        {
            return level switch
            {
                1 => gearDriftCostLvl1,
                2 => gearDriftCostLvl2,
                3 => gearDriftCostLvl3,
                _ => throw new System.NotImplementedException("Invalid Level")
            };
        }

        public static float LoadTornadoCost(int level, float gearTornadoCostLvl1, float gearTornadoCostLvl2,
            float gearTornadoCostLvl3)
        {
            return level switch
            {
                1 => gearTornadoCostLvl1,
                2 => gearTornadoCostLvl2,
                3 => gearTornadoCostLvl3,
                _ => throw new System.NotImplementedException("Invalid Level")
            };
        }

        #endregion

        #region VirtualMemebers

        public virtual void LoadTopSpeed()
        {
            _playerBehaviour.speedStats.TopSpeed = LoadTopSpeed(_playerBehaviour.fuel.Level,
                _playerBehaviour.defaultPlayerStats.TopSpeedLevelUp, _playerBehaviour.characterData.TopSpeed,
                _playerBehaviour.extremeGearData.movementVars.TopSpeed, _playerBehaviour.characterData.StatsType);
        }

        public virtual void LoadAccelerationLow()
        {
            _playerBehaviour.speedStats.AccelerationLow =
                LoadAccelerationLow(_playerBehaviour.fuel.Level,
                    _playerBehaviour.defaultPlayerStats.AccelerationLowLvl1,
                    _playerBehaviour.defaultPlayerStats.AccelerationLowLvl2,
                    _playerBehaviour.defaultPlayerStats.AccelerationLowLvl3);
        }

        public virtual void LoadAccelerationMedium()
        {
            _playerBehaviour.speedStats.AccelerationMedium =
                LoadAccelerationMedium(_playerBehaviour.fuel.Level,
                    _playerBehaviour.defaultPlayerStats.AccelerationMediumLvl1,
                    _playerBehaviour.defaultPlayerStats.AccelerationMediumLvl2,
                    _playerBehaviour.defaultPlayerStats.AccelerationMediumLvl3);
        }

        public virtual void LoadAccelerationHigh()
        {
            _playerBehaviour.speedStats.AccelerationHigh =
                LoadAccelerationHigh(_playerBehaviour.fuel.Level,
                    _playerBehaviour.defaultPlayerStats.AccelerationHighLvl1,
                    _playerBehaviour.defaultPlayerStats.AccelerationHighLvl2,
                    _playerBehaviour.defaultPlayerStats.AccelerationHighLvl3);
        }

        public virtual void LoadAccelerationLowThreshold()
        {
            _playerBehaviour.speedStats.AccelerationLowThreshold =
                LoadAccelerationLowThreshold(_playerBehaviour.fuel.Level,
                    _playerBehaviour.defaultPlayerStats.AccelerationLowThresholdLvl1,
                    _playerBehaviour.defaultPlayerStats.AccelerationLowThresholdLvl2,
                    _playerBehaviour.defaultPlayerStats.AccelerationLowThresholdLvl3);
        }

        public virtual void LoadAccelerationMediumThreshold()
        {
            _playerBehaviour.speedStats.AccelerationMediumThreshold =
                LoadAccelerationMediumThreshold(_playerBehaviour.fuel.Level,
                    _playerBehaviour.defaultPlayerStats.AccelerationMediumThresholdLvl1,
                    _playerBehaviour.defaultPlayerStats.AccelerationMediumThresholdLvl2,
                    _playerBehaviour.defaultPlayerStats.AccelerationMediumThresholdLvl3);
        }

        public virtual void LoadAccelerationOffRoadThreshold()
        {
            _playerBehaviour.speedStats.AccelerationOffRoadThreshold =
                LoadAccelerationOffRoadThreshold(_playerBehaviour.fuel.Level,
                    _playerBehaviour.defaultPlayerStats.AccelerationOffRoadThresholdLvl1,
                    _playerBehaviour.defaultPlayerStats.AccelerationOffRoadThresholdLvl2,
                    _playerBehaviour.defaultPlayerStats.AccelerationOffRoadThresholdLvl3);
        }

        public virtual void LoadBoostSpeed()
        {
            _playerBehaviour.speedStats.BoostSpeed = _playerBehaviour.speedStats.BoostSpeed = LoadBoostSpeed(
                _playerBehaviour.fuel.Level,
                _playerBehaviour.characterData.BoostSpeed,
                _playerBehaviour.extremeGearData.movementVars.BoostSpeedLvl1,
                _playerBehaviour.extremeGearData.movementVars.BoostSpeedLvl2,
                _playerBehaviour.extremeGearData.movementVars.BoostSpeedLvl3);
        }

        /// <summary>
        /// BoostChainModifier = CharacterData.BoostChainModifier + ExtremeGearData.BoostChainModifier
        /// </summary>
        public virtual void LoadBoostChainModifier()
        {
            _playerBehaviour.speedStats.BoostChainModifier = LoadBoostChainModifier(
                _playerBehaviour.characterData.BoostChainModifier,
                _playerBehaviour.extremeGearData.movementVars.BoostChainModifier);
        }

        public virtual void LoadBoostDuration()
        {
            _playerBehaviour.speedStats.BoostDuration = LoadBoostDuration(_playerBehaviour.fuel.Level,
                _playerBehaviour.defaultPlayerStats.BoostDuration, _playerBehaviour.characterData.BoostDurationLvl1,
                _playerBehaviour.characterData.BoostDurationLvl2, _playerBehaviour.characterData.BoostDurationLvl3);
        }

        public virtual void LoadBreakeDecelleration()
        {
            _playerBehaviour.speedStats.BreakeDecelleration =
                LoadBreakeDecelleration(_playerBehaviour.defaultPlayerStats.BreakeDecelerationDefault);
        }

        public virtual void LoadDriftDashSpeed()
        {
            _playerBehaviour.speedStats.DriftDashSpeed = LoadDriftDashSpeed(_playerBehaviour.fuel.Level,
                _playerBehaviour.characterData.Drift, _playerBehaviour.extremeGearData.movementVars.DriftDashSpeedLvl1,
                _playerBehaviour.extremeGearData.movementVars.DriftDashSpeedLvl2,
                _playerBehaviour.extremeGearData.movementVars.DriftDashSpeedLvl3);
        }

        public virtual void LoadDriftCap()
        {
            _playerBehaviour.speedStats.DriftCap = LoadDriftCap(_playerBehaviour.fuel.Level,
                _playerBehaviour.defaultPlayerStats.DriftCapLevelUp, _playerBehaviour.characterData.Drift,
                _playerBehaviour.extremeGearData.movementVars.DriftCap);
        }

        public virtual void LoadDriftDashFrames()
        {
            _playerBehaviour.speedStats.DriftDashFrames =
                LoadDriftDashFrames(_playerBehaviour.extremeGearData.movementVars.DriftDashChargeDuration);
        }

        public virtual void LoadTurnSpeedLoss()
        {
            _playerBehaviour.speedStats.TurnSpeedLoss =
                LoadTurnSpeedLoss(_playerBehaviour.defaultPlayerStats.TurnSpeedLoss);
        }

        public virtual void LoadJumpChargeMinSpeed()
        {
            _playerBehaviour.speedStats.JumpChargeMinSpeed =
                LoadJumpChargeMinSpeed(_playerBehaviour.defaultPlayerStats.JumpChargeMinSpeedDefault);
        }

        public virtual void LoadJumpChargeDeceleration()
        {
            _playerBehaviour.speedStats.JumpChargeDecelleration =
                LoadJumpChargeDecelleration(_playerBehaviour.defaultPlayerStats.JumpChargeDecelerationDefault);
        }

        // TURNING
        public virtual void LoadTurnRate()
        {
            _playerBehaviour.turnStats.Turnrate = LoadTurnRate(_playerBehaviour.defaultPlayerStats.TurnrateDefault);
        }

        public virtual void LoadTurnRateMax()
        {
            _playerBehaviour.turnStats.TurnRateMax = LoadTurnRateMax(_playerBehaviour.defaultPlayerStats.TurnRateMax);
        }

        public virtual void LoadTurnLowSpeedMultiplier()
        {
            _playerBehaviour.turnStats.TurnLowSpeedMultiplier =
                LoadTurnLowSpeedMultiplier(_playerBehaviour.defaultPlayerStats.TurnLowSpeedMultiplierDefault);
        }

        public virtual void LoadTurnSpeedLossCurve()
        {
            _playerBehaviour.turnStats.TurnSpeedLossCurve =
                LoadTurnSpeedLossCurve(_playerBehaviour.defaultPlayerStats.TurnSpeedLossCurveDefault);
        }

        public virtual void LoadTurnRateCurve()
        {
            _playerBehaviour.turnStats.TurnrateCurve =
                LoadTurnRateCurve(_playerBehaviour.defaultPlayerStats.TurnrateCurveDefault);
        }

        public virtual void LoadDriftTurnRateMin()
        {
            _playerBehaviour.turnStats.DriftTurnrateMin =
                LoadDriftTurnRateMin(_playerBehaviour.defaultPlayerStats.DriftTurnrateMinDefault);
        }

        public virtual void LoadDriftTurnRateMax()
        {
            _playerBehaviour.turnStats.DriftTurnrateMax =
                LoadDriftTurnRateMax(_playerBehaviour.defaultPlayerStats.DriftTurnRateMaxDefault);
        }

        public virtual void LoadDriftTurnRate()
        {
            _playerBehaviour.turnStats.DriftTurnrate =
                LoadDriftTurnRate(_playerBehaviour.defaultPlayerStats.DriftTurnrateDefault);
        }

        //Jump
        public virtual void LoadJumpSpeedMax()
        {
            _playerBehaviour.jumpStats.JumpSpeedMax =
                LoadJumpSpeedMax(_playerBehaviour.defaultPlayerStats.JumpSpeedMaxDefault);
        }

        public virtual void LoadJumpAcceleration()
        {
            _playerBehaviour.jumpStats.JumpAccelleration =
                LoadJumpAcceleration(_playerBehaviour.defaultPlayerStats.JumpAccelDefault);
        }

        // FUEL
        public virtual void LoadFuelType()
        {
            _playerBehaviour.fuelStats.FuelType = LoadFuelType(_playerBehaviour.extremeGearData.fuelVars.Fuel);
        }

        public virtual void LoadJumpChargeMultiplier()
        {
            _playerBehaviour.fuelStats.JumpChargeMultiplier =
                LoadJumpChargeMultiplier(_playerBehaviour.extremeGearData.fuelVars.JumpChargeMultiplier);
        }

        public virtual void LoadTrickFuelGain()
        {
            _playerBehaviour.fuelStats.TrickFuelGain =
                LoadTrickFuelGain(_playerBehaviour.extremeGearData.fuelVars.TrickFuelGain);
        }

        public virtual void LoadTypeFuelGain()
        {
            _playerBehaviour.fuelStats.TypeFuelGain =
                LoadTypeFuelGain(_playerBehaviour.extremeGearData.fuelVars.TypeFuelGain);
        }

        public virtual void LoadQTEFuelGain()
        {
            _playerBehaviour.fuelStats.QTEFuelGain =
                LoadTypeFuelGain(_playerBehaviour.extremeGearData.fuelVars.QTEFuelGain);
        }

        public virtual void LoadPassiveDrain()
        {
            _playerBehaviour.fuelStats.PassiveDrain = LoadPassiveDrain(_playerBehaviour.fuel.Level,
                _playerBehaviour.extremeGearData.fuelVars.PassiveDrainLvl1,
                _playerBehaviour.extremeGearData.fuelVars.PassiveDrainLvl2,
                _playerBehaviour.extremeGearData.fuelVars.PassiveDrainLvl3);
        }

        public virtual void LoadTankSize()
        {
            _playerBehaviour.fuelStats.TankSize = LoadTankSize(_playerBehaviour.fuel.Level,
                _playerBehaviour.extremeGearData.fuelVars.FuelTankSizeLevel1,
                _playerBehaviour.extremeGearData.fuelVars.FuelTankSizeLevel2,
                _playerBehaviour.extremeGearData.fuelVars.FuelTankSizeLevel3);
        }

        public virtual void LoadBoostCost()
        {
            _playerBehaviour.fuelStats.BoostCost = LoadBoostCost(_playerBehaviour.fuel.Level,
                _playerBehaviour.extremeGearData.fuelVars.BoostCostLvl1,
                _playerBehaviour.extremeGearData.fuelVars.BoostCostLvl2,
                _playerBehaviour.extremeGearData.fuelVars.BoostCostLvl3);
        }

        public virtual void LoadDriftCost()
        {
            _playerBehaviour.fuelStats.DriftCost = LoadDriftCost(_playerBehaviour.fuel.Level,
                _playerBehaviour.extremeGearData.fuelVars.DriftCostLvl1,
                _playerBehaviour.extremeGearData.fuelVars.DriftCostLvl2,
                _playerBehaviour.extremeGearData.fuelVars.DriftCostLvl3);
        }

        public virtual void LoadTornadoCost()
        {
            _playerBehaviour.fuelStats.TorandoCost = LoadTornadoCost(_playerBehaviour.fuel.Level,
                _playerBehaviour.extremeGearData.fuelVars.TornadoCostLvl1,
                _playerBehaviour.extremeGearData.fuelVars.TornadoCostLvl2,
                _playerBehaviour.extremeGearData.fuelVars.TornadoCostLvl3);
        }

        #endregion
    }
}