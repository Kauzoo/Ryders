using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryders.Core.Player
{
    public interface IStatLoader
    {
        public virtual void LoadStats(int level)
        {
            LoadTopSpeed(level);
            LoadBoostSpeed(level);
            LoadBoostChainModifier();
            LoadDriftDashSpeed(level);
            LoadDriftCap(level);
            LoadDrifDashFrames();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void LoadTopSpeed(int level) { speedStats.TopSpeed = (DefaultPlayerStats.TopSpeedLevelUp * level) + CharacterData.TopSpeed + ExtremeGearData.movementVars.TopSpeed; }
        public virtual void LoadMinSpeed() { speedStats.MinSpeed = DefaultPlayerStats.MinSpeedDefault; }
        public virtual void LoadFastAccelleration() { speedStats.FastAccelleration = DefaultPlayerStats.FastAccelerationDefault; }
        /// <summary>
        /// Affected by Level (ExtremeGear).
        /// BoostSpeed = CharacterData.BoostSpeed + ExtremeGearData.BoostSpeedLvl
        /// <see cref="PlayerBehaviour.BoostSpeed"/>
        /// </summary>
        /// <param name="level"></param>
        public virtual void LoadBoostSpeed(int level)
        {
            switch(level)
            {
                case 1:
                    speedStats.BoostSpeed = CharacterData.BoostSpeed + ExtremeGearData.movementVars.BoostSpeedLvl1;
                    break;
                case 2:
                    speedStats.BoostSpeed = CharacterData.BoostSpeed + ExtremeGearData.movementVars.BoostSpeedLvl2;
                    break;
                case 3:
                    speedStats.BoostSpeed = CharacterData.BoostSpeed + ExtremeGearData.movementVars.BoostSpeedLvl3;
                    break;
                default:
                    throw new System.NotImplementedException("Invalid Level");
            }
            
        }
        /// <summary>
        /// BoostChainModifier = CharacterData.BoostChainModifier + ExtremeGearData.BoostChainModifier
        /// </summary>
        public virtual void LoadBoostChainModifier() { speedStats.BoostChainModifier = CharacterData.BoostChainModifier + ExtremeGearData.movementVars.BoostChainModifier; }
        public virtual void LoadBreakeDecelleration() { speedStats.BreakeDecelleration = DefaultPlayerStats.BreakeDecelerationDefault; }
        /// <summary>
        /// Affected by Level (ExtremeGear).
        /// DriftDashSpeed = CharacterData.Drift + ExtremeGearData.DriftDashSpeedLvl;
        /// </summary>
        /// <param name="level"></param>
        public virtual void LoadDriftDashSpeed(int level)
        {
            switch(level)
            {
                case 1:
                    speedStats.DriftDashSpeed = CharacterData.Drift + ExtremeGearData.movementVars.DriftDashSpeedLvl1;
                    break;
                case 2:
                    speedStats.DriftDashSpeed = CharacterData.Drift + ExtremeGearData.movementVars.DriftDashSpeedLvl2;
                    break;
                case 3:
                    speedStats.DriftDashSpeed = CharacterData.Drift + ExtremeGearData.movementVars.DriftDashSpeedLvl3;
                    break;
                default:
                    throw new System.NotImplementedException("Invalid Level");
            }
            
        }

        public virtual void LoadDriftCap(int level) { speedStats.DriftCap = (DefaultPlayerStats.DriftCapLevelUp * level) + CharacterData.Drift + ExtremeGearData.movementVars.DriftCap; }
        public virtual void LoadDrifDashFrames() { speedStats.DriftDashFrames = ExtremeGearData.movementVars.DriftDashChargeDuration; }
        public virtual void LoadTurnSpeedLoss() { speedStats.TurnSpeedLoss = DefaultPlayerStats.TurnSpeedLossCurveDefault; }
        public virtual void LoadJumpChargeMinSpeed() { speedStats.JumpChargeMinSpeed = DefaultPlayerStats.JumpChargeMinSpeedDefault; }
        public virtual void LoadJumpChargeDecelleration() { speedStats.JumpChargeDecelleration = DefaultPlayerStats.JumpChargeDecelerationDefault; }

        /**
         * TURNING
         */
        public virtual void LoadTurnrate() { turnStats.Turnrate = DefaultPlayerStats.TurnrateDefault; }
        public virtual void LoadTurnSpeedLossCurve() { turnStats.TurnSpeedLossCurve = DefaultPlayerStats.TurnSpeedLossCurveDefault; }
        public virtual void LoadTurnrateCurve() { turnStats.TurnrateCurve = DefaultPlayerStats.TurnrateCurveDefault; }
        public virtual void LoadDriftTurnratePassive() { turnStats.DriftTurnratePassive = DefaultPlayerStats.DriftTurnrateDefault; }
        public virtual void LoadDriftTurnrateMin() { turnStats.DriftTurnrateMin = DefaultPlayerStats.DriftTurnrateMinDefault; }
        public virtual void LoadDriftTurnrate() { turnStats.DriftTurnrate = DefaultPlayerStats.DriftTurnrateDefault; }

        /**
         * JUMP
         */
        public virtual void LoadJumpSpeedMax() { jumpStats.JumpSpeedMax = DefaultPlayerStats.JumpSpeedMaxDefault; }
        public virtual void LoadJumpAccelleration() { jumpStats.JumpAccelleration = DefaultPlayerStats.JumpAccelDefault; }

        /**
         * FUEL
         */
        public virtual void LoadFuelType() { fuelStats.FuelType = ExtremeGearData.fuelVars.Fuel; }
        public virtual void LoadJumpChargeMultiplier() { fuelStats.JumpChargeMultiplier = ExtremeGearData.fuelVars.JumpChargeMultiplier; }
        public virtual void LoadTrickFuelGain() { fuelStats.TrickFuelGain = ExtremeGearData.fuelVars.TrickFuelGain; }
        public virtual void LoadTypeFuelGain() { fuelStats.TypeFuelGain = ExtremeGearData.fuelVars.TypeFuelGain;  }
        public virtual void LoadQTEFuelGain() { fuelStats.QTEFuelGain = ExtremeGearData.fuelVars.QTEFuelGain; }
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
    }
}