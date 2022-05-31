using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ryders.Core.Player.Character;
using Ryders.Core.Player.DefaultBehaviour;
using Ryders.Core.Player.ExtremeGear;
using UnityEngine;

namespace Ryders.Core.Player
{
    public abstract class StatLoaderPack : MonoBehaviour
    {
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
        /*
        public virtual void LoadFuelType()
        {
            fuelStats.FuelType = ExtremeGearData.fuelVars.Fuel;
        }
    
        public virtual void LoadJumpChargeMultiplier()
        {
            fuelStats.JumpChargeMultiplier = ExtremeGearData.fuelVars.JumpChargeMultiplier;
        }
    
        public virtual void LoadTrickFuelGain()
        {
            fuelStats.TrickFuelGain = ExtremeGearData.fuelVars.TrickFuelGain;
        }
    
        public virtual void LoadTypeFuelGain()
        {
            fuelStats.TypeFuelGain = ExtremeGearData.fuelVars.TypeFuelGain;
        }
    
        public virtual void LoadQTEFuelGain()
        {
            fuelStats.QTEFuelGain = ExtremeGearData.fuelVars.QTEFuelGain;
        }
    
        public virtual void LoadPassiveDrain(int level)
        {
            switch (level)
            {
                case 1:
                    fuelStats.PassiveDrain = ExtremeGearData.fuelVars.PassiveDrainLvl1;
                    break;
                case 2:
                    fuelStats.PassiveDrain = ExtremeGearData.fuelVars.PassiveDrainLvl2;
                    break;
                case 3:
                    fuelStats.PassiveDrain = ExtremeGearData.fuelVars.PassiveDrainLvl3;
                    break;
                default:
                    throw new System.NotImplementedException("Invalid Level");
            }
        }
    
        public virtual void LoadTankSize(int level)
        {
            switch (level)
            {
                case 1:
                    fuelStats.TankSize = ExtremeGearData.fuelVars.FuelTankSizeLevel1;
                    break;
                case 2:
                    fuelStats.TankSize = ExtremeGearData.fuelVars.FuelTankSizeLevel2;
                    break;
                case 3:
                    fuelStats.TankSize = ExtremeGearData.fuelVars.FuelTankSizeLevel3;
                    break;
                default:
                    throw new System.NotImplementedException("Invalid Level");
            }
        }
    
        public virtual void LoadBoostCost(int level)
        {
            switch (level)
            {
                case 1:
                    fuelStats.BoostCost = ExtremeGearData.fuelVars.BoostCostLvl1;
                    break;
                case 2:
                    fuelStats.BoostCost = ExtremeGearData.fuelVars.BoostCostLvl2;
                    break;
                case 3:
                    fuelStats.BoostCost = ExtremeGearData.fuelVars.BoostCostLvl3;
                    break;
                default:
                    throw new System.NotImplementedException("Invalid Level");
            }
        }
    
        public virtual void LoadDriftCost(int level)
        {
            switch (level)
            {
                case 1:
                    fuelStats.DriftCost = ExtremeGearData.fuelVars.DriftCostLvl1;
                    break;
                case 2:
                    fuelStats.DriftCost = ExtremeGearData.fuelVars.DriftCostLvl2;
                    break;
                case 3:
                    fuelStats.DriftCost = ExtremeGearData.fuelVars.DriftCostLvl3;
                    break;
                default:
                    throw new System.NotImplementedException("Invalid Level");
            }
        }
    
        public virtual void LoadTornadoCost(int level)
        {
            switch (level)
            {
                case 1:
                    fuelStats.TorandoCost = ExtremeGearData.fuelVars.TornadoCostLvl1;
                    break;
                case 2:
                    fuelStats.TorandoCost = ExtremeGearData.fuelVars.TornadoCostLvl2;
                    break;
                case 3:
                    fuelStats.TorandoCost = ExtremeGearData.fuelVars.TornadoCostLvl3;
                    break;
                default:
                    throw new System.NotImplementedException("Invalid Level");
            }
        }
        */

        #region VirtualMemebers

        public virtual void LoadTopSpeed(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadMinSpeed(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadFastAcceleration(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadBoostSpeed(PlayerBehaviour pb)
        {
            pb.speedStats.BoostSpeed = pb.speedStats.BoostSpeed = LoadBoostSpeed(pb.fuel.Level,
                pb.characterData.BoostSpeed,
                pb.extremeGearData.movementVars.BoostSpeedLvl1, pb.extremeGearData.movementVars.BoostSpeedLvl2,
                pb.extremeGearData.movementVars.BoostSpeedLvl3);
        }

        /// <summary>
        /// BoostChainModifier = CharacterData.BoostChainModifier + ExtremeGearData.BoostChainModifier
        /// </summary>
        public virtual void LoadBoostChainModifier(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadBreakeDecelleration(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }
        
        public virtual void LoadDriftDashSpeed(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadDriftCap(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadDrifDashFrames(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadTurnSpeedLoss(PlayerBehaviour pb)
        {  
            throw new NotImplementedException();
        }

        public virtual void LoadJumpChargeMinSpeed(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadJumpChargeDecelleration(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }

        // TURNING
        public virtual void LoadTurnrate(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadTurnSpeedLossCurve(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadTurnrateCurve(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadDriftTurnratePassive(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadDriftTurnrateMin(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadDriftTurnrate(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }

        //Jump
        public virtual void LoadJumpSpeedMax(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadJumpAccelleration(PlayerBehaviour pb)
        {
            throw new NotImplementedException();
        }
        
        // TODO Implement virtual members for Fuel
        #endregion
    }
}