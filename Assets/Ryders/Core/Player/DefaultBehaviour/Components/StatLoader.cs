using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ryders.Core.Player.Character;
using Ryders.Core.Player.DefaultBehaviour;
using Ryders.Core.Player.ExtremeGear;
using UnityEngine;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    public abstract class StatLoaderPack : MonoBehaviour
    {
        // TODO Implement this in a mors sensible way
        /**
         *  HIDDEN GLOBAL STATS
         */
        private const int FlyTypeTopSpeedLoss = -7;
        private const int PowerTypeSpeedLoss = -4;
        private const int GlobalBoostDuration = 30;
        
        public PlayerBehaviour playerBehaviour;

        // TODO Implement FastAccel for SpeedTypes and Off-Road resistance for PowerType 
        
        public virtual void Setup()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        public virtual void LoadStatsMaster()
        {
            LoadTopSpeed();
            LoadMinSpeed();
            LoadFastAcceleration();
            LoadBoostSpeed();
            LoadBoostChainModifier();
            LoadBoostDuration();
            LoadBreakeDecelleration();
            LoadDriftDashSpeed();
            LoadDriftCap();
            LoadDrifDashFrames();
            LoadTurnSpeedLoss();
            LoadJumpChargeMinSpeed();
            LoadJumpChargeDecelleration();
            LoadTurnrate();
            LoadTurnSpeedLossCurve();
            LoadTurnrateCurve();
            LoadDriftTurnratePassive();
            LoadDriftTurnrateMin();
            LoadDriftTurnrate();
            LoadJumpSpeedMax();
            LoadJumpAccelleration();
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
                CharacterType.Speed =>  (defaultTopSpeedLevelUp * (level - 1)) + characterTopSpeed + gearTopSpeed,
                CharacterType.Fly => (defaultTopSpeedLevelUp * (level - 1)) + characterTopSpeed + gearTopSpeed - 7,
                CharacterType.Power => (defaultTopSpeedLevelUp * (level - 1)) + characterTopSpeed + gearTopSpeed -4,
                _ => throw new System.NotImplementedException("Invalid Level")
            };
            return (defaultTopSpeedLevelUp * (level - 1)) + characterTopSpeed + gearTopSpeed;
        }

        public static float LoadMinSpeed(int level, float defaultMinSpeedDefault)
        {
            return defaultMinSpeedDefault;
        }

        public static float LoadFastAcceleration(int level, float defaultFastAccelerationDefault)
        {
            return defaultFastAccelerationDefault;
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
                1 => defaultBoostDuration * (level - 1) + characterBoostDurationLvl1,
                2 => defaultBoostDuration * (level - 1) + characterBoostDurationLvl2,
                3 => defaultBoostDuration * (level - 1) + characterBoostDurationLvl3,
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

        public static float LoadDrifDashFrames(float gearDriftDashChargeDuration)
        {
            return gearDriftDashChargeDuration;
        }

        public static AnimationCurve LoadTurnSpeedLoss(AnimationCurve defaultTurnSpeedLossCurveDefault)
        {
            return defaultTurnSpeedLossCurveDefault;
        }

        public static float LoadJumpChargeMinSpeed(float defaultJumpChargeMinSpeedDefault)
        {
            return defaultJumpChargeMinSpeedDefault;
        }

        public static float LoadJumpChargeDecelleration(float defaultJumpChargeDecelerationDefault)
        {
            return defaultJumpChargeDecelerationDefault;
        }

        // TURNING
        public static float LoadTurnrate(float defaultTurnrateDefault)
        {
            return defaultTurnrateDefault;
        }

        public static AnimationCurve LoadTurnSpeedLossCurve(AnimationCurve defaultTurnSpeedLossCurveDefault)
        {
            return defaultTurnSpeedLossCurveDefault;
        }

        public static AnimationCurve LoadTurnrateCurve(AnimationCurve defaultTurnrateCurveDefault)
        {
            return defaultTurnrateCurveDefault;
        }

        public static float LoadDriftTurnratePassive(float defaultDriftTurnrateDefault)
        {
            return defaultDriftTurnrateDefault;
        }

        public static float LoadDriftTurnrateMin(float defaultDriftTurnrateMinDefault)
        {
            return defaultDriftTurnrateMinDefault;
        }

        public static float LoadDriftTurnrate(float defaultDriftTurnrateDefault)
        {
            return defaultDriftTurnrateDefault;
        }

        //Jump
        public static float LoadJumpSpeedMax(float defaultJumpSpeedMaxDefault)
        {
            return defaultJumpSpeedMaxDefault;
        }

        public static AnimationCurve LoadJumpAccelleration(AnimationCurve defaultJumpAccelDefault)
        {
            return defaultJumpAccelDefault;
        }

        // FUEL
        // TODO Implement Loader for Fuel
        public static FuelType LoadFuelType(FuelType gearFuelType)
        {
            return gearFuelType;
        }

        public static float LoadJumpChargeMultiplier(float gearJumpChargeMultiplier)
        {
            return gearJumpChargeMultiplier;
        }

        public static float LoadTrickFuelGain(float gearTrickFuelGain)
        {
            return gearTrickFuelGain;
        }

        public static float LoadTypeFuelGain(float gearTypeFuelGain)
        {
            return gearTypeFuelGain;
        }

        public static float LoadQTEFuelGain(float gearQTEFuelGain)
        {
            return gearQTEFuelGain;
        }

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
            playerBehaviour.speedStats.TopSpeed = LoadTopSpeed(playerBehaviour.fuel.Level,
                playerBehaviour.defaultPlayerStats.TopSpeedLevelUp, playerBehaviour.characterData.TopSpeed,
                playerBehaviour.extremeGearData.movementVars.TopSpeed, playerBehaviour.characterData.StatsType);
        }

        public virtual void LoadMinSpeed()
        {
            playerBehaviour.speedStats.MinSpeed = LoadMinSpeed(playerBehaviour.fuel.Level,
                playerBehaviour.defaultPlayerStats.MinSpeedDefault);
        }

        public virtual void LoadFastAcceleration()
        {
            playerBehaviour.speedStats.FastAccelleration = LoadFastAcceleration(playerBehaviour.fuel.Level,
                playerBehaviour.defaultPlayerStats.FastAccelerationDefault);
        }

        public virtual void LoadBoostSpeed()
        {
            playerBehaviour.speedStats.BoostSpeed = playerBehaviour.speedStats.BoostSpeed = LoadBoostSpeed(
                playerBehaviour.fuel.Level,
                playerBehaviour.characterData.BoostSpeed,
                playerBehaviour.extremeGearData.movementVars.BoostSpeedLvl1,
                playerBehaviour.extremeGearData.movementVars.BoostSpeedLvl2,
                playerBehaviour.extremeGearData.movementVars.BoostSpeedLvl3);
        }

        /// <summary>
        /// BoostChainModifier = CharacterData.BoostChainModifier + ExtremeGearData.BoostChainModifier
        /// </summary>
        public virtual void LoadBoostChainModifier()
        {
            playerBehaviour.speedStats.BoostChainModifier = LoadBoostChainModifier(
                playerBehaviour.characterData.BoostChainModifier,
                playerBehaviour.extremeGearData.movementVars.BoostChainModifier);
        }

        public virtual void LoadBoostDuration()
        {
            playerBehaviour.speedStats.BoostDuration = LoadBoostDuration(playerBehaviour.fuel.Level,
                playerBehaviour.defaultPlayerStats.BoostDuration, playerBehaviour.characterData.BoostDurationLvl1,
                playerBehaviour.characterData.BoostDurationLvl2, playerBehaviour.characterData.BoostDurationLvl3);
        }

        public virtual void LoadBreakeDecelleration()
        {
            playerBehaviour.speedStats.BreakeDecelleration =
                LoadBreakeDecelleration(playerBehaviour.defaultPlayerStats.BreakeDecelerationDefault);
        }

        public virtual void LoadDriftDashSpeed()
        {
            playerBehaviour.speedStats.DriftDashSpeed = LoadDriftDashSpeed(playerBehaviour.fuel.Level,
                playerBehaviour.characterData.Drift, playerBehaviour.extremeGearData.movementVars.DriftDashSpeedLvl1,
                playerBehaviour.extremeGearData.movementVars.DriftDashSpeedLvl2,
                playerBehaviour.extremeGearData.movementVars.DriftDashSpeedLvl3);
        }

        public virtual void LoadDriftCap()
        {
            playerBehaviour.speedStats.DriftCap = LoadDriftCap(playerBehaviour.fuel.Level,
                playerBehaviour.defaultPlayerStats.DriftCapLevelUp, playerBehaviour.characterData.Drift,
                playerBehaviour.extremeGearData.movementVars.DriftCap);
        }

        public virtual void LoadDrifDashFrames()
        {
            playerBehaviour.speedStats.DriftDashFrames =
                LoadDrifDashFrames(playerBehaviour.extremeGearData.movementVars.DriftDashChargeDuration);
        }

        public virtual void LoadTurnSpeedLoss()
        {
            playerBehaviour.speedStats.TurnSpeedLoss =
                LoadTurnSpeedLoss(playerBehaviour.defaultPlayerStats.TurnSpeedLossCurveDefault);
        }

        public virtual void LoadJumpChargeMinSpeed()
        {
            playerBehaviour.speedStats.JumpChargeMinSpeed =
                LoadJumpChargeMinSpeed(playerBehaviour.defaultPlayerStats.JumpChargeMinSpeedDefault);
        }

        public virtual void LoadJumpChargeDecelleration()
        {
            playerBehaviour.speedStats.JumpChargeDecelleration =
                LoadJumpChargeDecelleration(playerBehaviour.defaultPlayerStats.JumpChargeDecelerationDefault);
        }

        // TURNING
        public virtual void LoadTurnrate()
        {
            playerBehaviour.turnStats.Turnrate = LoadTurnrate(playerBehaviour.defaultPlayerStats.TurnrateDefault);
        }

        public virtual void LoadTurnSpeedLossCurve()
        {
            playerBehaviour.turnStats.TurnSpeedLossCurve =
                LoadTurnSpeedLossCurve(playerBehaviour.defaultPlayerStats.TurnSpeedLossCurveDefault);
        }

        public virtual void LoadTurnrateCurve()
        {
            playerBehaviour.turnStats.TurnrateCurve = LoadTurnrateCurve(playerBehaviour.defaultPlayerStats.TurnrateCurveDefault);
        }

        public virtual void LoadDriftTurnratePassive()
        {
            playerBehaviour.turnStats.DriftTurnratePassive =
                LoadDriftTurnratePassive(playerBehaviour.defaultPlayerStats.DriftTurnratePassiveDefault);
        }

        public virtual void LoadDriftTurnrateMin()
        {
            playerBehaviour.turnStats.DriftTurnrateMin =
                LoadDriftTurnrateMin(playerBehaviour.defaultPlayerStats.DriftTurnrateMinDefault);
        }

        public virtual void LoadDriftTurnrate()
        {
            playerBehaviour.turnStats.DriftTurnrate =
                LoadDriftTurnrate(playerBehaviour.defaultPlayerStats.DriftTurnrateDefault);
        }

        //Jump
        public virtual void LoadJumpSpeedMax()
        {
            playerBehaviour.jumpStats.JumpSpeedMax = LoadJumpSpeedMax(playerBehaviour.defaultPlayerStats.JumpSpeedMaxDefault);
        }

        public virtual void LoadJumpAccelleration()
        {
            playerBehaviour.jumpStats.JumpAccelleration =
                LoadJumpAccelleration(playerBehaviour.defaultPlayerStats.JumpAccelDefault);
        }

        // FUEL
        public virtual void LoadFuelType()
        {
            playerBehaviour.fuelStats.FuelType = LoadFuelType(playerBehaviour.extremeGearData.fuelVars.Fuel);
        }

        public virtual void LoadJumpChargeMultiplier()
        {
            playerBehaviour.fuelStats.JumpChargeMultiplier =
                LoadJumpChargeMultiplier(playerBehaviour.extremeGearData.fuelVars.JumpChargeMultiplier);
        }

        public virtual void LoadTrickFuelGain()
        {
            playerBehaviour.fuelStats.TrickFuelGain =
                LoadTrickFuelGain(playerBehaviour.extremeGearData.fuelVars.TrickFuelGain);
        }

        public virtual void LoadTypeFuelGain()
        {
            playerBehaviour.fuelStats.TypeFuelGain =
                LoadTypeFuelGain(playerBehaviour.extremeGearData.fuelVars.TypeFuelGain);
        }

        public virtual void LoadQTEFuelGain()
        {
            playerBehaviour.fuelStats.QTEFuelGain =
                LoadTypeFuelGain(playerBehaviour.extremeGearData.fuelVars.QTEFuelGain);
        }

        public virtual void LoadPassiveDrain()
        {
            playerBehaviour.fuelStats.PassiveDrain = LoadPassiveDrain(playerBehaviour.fuel.Level,
                playerBehaviour.extremeGearData.fuelVars.PassiveDrainLvl1,
                playerBehaviour.extremeGearData.fuelVars.PassiveDrainLvl2,
                playerBehaviour.extremeGearData.fuelVars.PassiveDrainLvl3);
        }

        public virtual void LoadTankSize()
        {
            playerBehaviour.fuelStats.TankSize = LoadTankSize(playerBehaviour.fuel.Level,
                playerBehaviour.extremeGearData.fuelVars.FuelTankSizeLevel1,
                playerBehaviour.extremeGearData.fuelVars.FuelTankSizeLevel2,
                playerBehaviour.extremeGearData.fuelVars.FuelTankSizeLevel3);
        }

        public virtual void LoadBoostCost()
        {
            playerBehaviour.fuelStats.BoostCost = LoadBoostCost(playerBehaviour.fuel.Level,
                playerBehaviour.extremeGearData.fuelVars.BoostCostLvl1,
                playerBehaviour.extremeGearData.fuelVars.BoostCostLvl2,
                playerBehaviour.extremeGearData.fuelVars.BoostCostLvl3);
        }

        public virtual void LoadDriftCost()
        {
            playerBehaviour.fuelStats.DriftCost = LoadDriftCost(playerBehaviour.fuel.Level,
                playerBehaviour.extremeGearData.fuelVars.DriftCostLvl1,
                playerBehaviour.extremeGearData.fuelVars.DriftCostLvl2,
                playerBehaviour.extremeGearData.fuelVars.DriftCostLvl3);
        }

        public virtual void LoadTornadoCost()
        {
            playerBehaviour.fuelStats.TorandoCost = LoadTornadoCost(playerBehaviour.fuel.Level,
                playerBehaviour.extremeGearData.fuelVars.TornadoCostLvl1,
                playerBehaviour.extremeGearData.fuelVars.TornadoCostLvl2,
                playerBehaviour.extremeGearData.fuelVars.TornadoCostLvl3);
        }

        #endregion
    }
}