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
        public PlayerBehaviour playerBehaviour;

        #region StaticMethods

        public static float LoadTopSpeed(int level, float defaultTopSpeedLevelUp, float characterTopSpeed,
            float gearTopSpeed)
        {
            return (defaultTopSpeedLevelUp * level) + characterTopSpeed + gearTopSpeed;
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
            switch (level)
            {
                case 1:
                    return characterBoostSpeed + gearBoostSpeedLvl1;
                case 2:
                    return characterBoostSpeed + gearBoostSpeedLvl2;
                case 3:
                    return characterBoostSpeed + gearBoostSpeedLvl3;
                default:
                    Debug.LogWarning("Unimplemented Player level");
                    throw new System.NotImplementedException("Invalid Level");
            }
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
            switch (level)
            {
                case 1:
                    return characterDrift + gearDriftDashSpeedLvl1;
                case 2:
                    return characterDrift + gearDriftDashSpeedLvl2;
                case 3:
                    return characterDrift + gearDriftDashSpeedLvl3;
                    break;
                default:
                    Debug.LogWarning("Invalid Level");
                    throw new System.NotImplementedException("Invalid Level");
            }
        }

        public static float LoadDriftCap(int level, float defaultDriftCapLevelUp, float characterDrift,
            float gearDriftCap)
        {
            return (defaultDriftCapLevelUp * level) + characterDrift + gearDriftCap;
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

        public static float LoadJumpAccelleration(float defaultJumpAccelDefault)
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
                playerBehaviour.extremeGearData.movementVars.TopSpeed);
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

        public virtual void LoadBreakeDecelleration()
        {
            playerBehaviour.speedStats.BreakeDecelleration =
                LoadBreakeDecelleration(playerBehaviour.defaultPlayerStats.BreakeDecelerationDefault);
        }

        public virtual void LoadDriftDashSpeedT()
        {
            playerBehaviour.speedStats.DriftDashSpeed = LoadDriftDashSpeed(playerBehaviour.fuel.Level, )
        }

        public virtual void LoadDriftCap()
        {
            throw new NotImplementedException();
        }

        public virtual void LoadDrifDashFrames()
        {
            throw new NotImplementedException();
        }

        public virtual void LoadTurnSpeedLoss()
        {
            throw new NotImplementedException();
        }

        public virtual void LoadJumpChargeMinSpeed()
        {
            throw new NotImplementedException();
        }

        public virtual void LoadJumpChargeDecelleration()
        {
            throw new NotImplementedException();
        }

        // TURNING
        public virtual void LoadTurnrate()
        {
            throw new NotImplementedException();
        }

        public virtual void LoadTurnSpeedLossCurve()
        {
            throw new NotImplementedException();
        }

        public virtual void LoadTurnrateCurve()
        {
            throw new NotImplementedException();
        }

        public virtual void LoadDriftTurnratePassive()
        {
            throw new NotImplementedException();
        }

        public virtual void LoadDriftTurnrateMin()
        {
            throw new NotImplementedException();
        }

        public virtual void LoadDriftTurnrate()
        {
            throw new NotImplementedException();
        }

        //Jump
        public virtual void LoadJumpSpeedMax()
        {
            throw new NotImplementedException();
        }

        public virtual void LoadJumpAccelleration()
        {
            throw new NotImplementedException();
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