using System;
using Ryders.Core.Player.Character;
using Ryders.Core.Player.ExtremeGear;
using UnityEngine;
using static Nyr.UnityDev.Util.GetComponentSafe;


namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    [RequireComponent(typeof(PlayerBehaviour))]
    [RequireComponent(typeof(EventPublisherPack))]
    public abstract class StatLoaderPack : MonoBehaviour, IRydersPlayerComponent
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

        // ReSharper disable once MemberCanBePrivate.Global
        protected PlayerBehaviour playerBehaviour;
        // ReSharper disable once MemberCanBePrivate.Global
        protected EventPublisherPack eventPublisherPack;

        // TODO Implement FastAccel for SpeedTypes and Off-Road resistance for PowerType
        // TODO CleanUp which stat is retrieved from where

        [ContextMenu("Setup")]
        public virtual void Setup()
        {
            this.SafeGetComponent(ref playerBehaviour);
            this.SafeGetComponent(ref eventPublisherPack);
            eventPublisherPack.LevelChangeEvent += OnLevelChange;
            LoadStatsMaster();
        }

        public virtual void Master()
        {
        }

        /// <summary>
        /// Load all stats
        /// </summary>
        [ContextMenu("Load all Stats")]
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
            LoadMinRings();
            LoadMaxRings();
            LoadLevelCap();
        }

        /// <summary>
        /// Only Load stats that are affected by current level
        /// </summary>
        [ContextMenu("Load Level affected Stats")]
        public virtual void LoadLevelAffectedStats()
        {
            LoadTopSpeed();
            LoadAccelerationLow();
            LoadAccelerationMedium();
            LoadAccelerationHigh();
            LoadAccelerationLowThreshold();
            LoadAccelerationMediumThreshold();
            LoadAccelerationOffRoadThreshold();
            LoadBoostSpeed();
            LoadBoostDuration();
            LoadDriftDashSpeed();
            LoadDriftCap();
            // Fuel
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

        public static int LoadMinRings(int MinRings) => MinRings;

        public static int LoadMaxRings(int MaxRings) => MaxRings;

        public static int[] LoadLevelCaps(int Level1Cap, int Level2Cap, int Level3Cap) =>
            new[] { Level1Cap, Level2Cap, Level3Cap };

        #endregion

        #region VirtualMemebers

        public virtual void LoadTopSpeed()
        {
            playerBehaviour.speedStats.TopSpeed = LoadTopSpeed(playerBehaviour.fuel.Level,
                playerBehaviour.defaultPlayerStats.TopSpeedLevelUp, playerBehaviour.characterData.TopSpeed,
                playerBehaviour.extremeGearData.movementVars.TopSpeed, playerBehaviour.characterData.StatsType);
        }

        public virtual void LoadAccelerationLow()
        {
            playerBehaviour.speedStats.AccelerationLow =
                LoadAccelerationLow(playerBehaviour.fuel.Level,
                    playerBehaviour.defaultPlayerStats.AccelerationLowLvl1,
                    playerBehaviour.defaultPlayerStats.AccelerationLowLvl2,
                    playerBehaviour.defaultPlayerStats.AccelerationLowLvl3);
        }

        public virtual void LoadAccelerationMedium()
        {
            playerBehaviour.speedStats.AccelerationMedium =
                LoadAccelerationMedium(playerBehaviour.fuel.Level,
                    playerBehaviour.defaultPlayerStats.AccelerationMediumLvl1,
                    playerBehaviour.defaultPlayerStats.AccelerationMediumLvl2,
                    playerBehaviour.defaultPlayerStats.AccelerationMediumLvl3);
        }

        public virtual void LoadAccelerationHigh()
        {
            playerBehaviour.speedStats.AccelerationHigh =
                LoadAccelerationHigh(playerBehaviour.fuel.Level,
                    playerBehaviour.defaultPlayerStats.AccelerationHighLvl1,
                    playerBehaviour.defaultPlayerStats.AccelerationHighLvl2,
                    playerBehaviour.defaultPlayerStats.AccelerationHighLvl3);
        }

        public virtual void LoadAccelerationLowThreshold()
        {
            playerBehaviour.speedStats.AccelerationLowThreshold =
                LoadAccelerationLowThreshold(playerBehaviour.fuel.Level,
                    playerBehaviour.defaultPlayerStats.AccelerationLowThresholdLvl1,
                    playerBehaviour.defaultPlayerStats.AccelerationLowThresholdLvl2,
                    playerBehaviour.defaultPlayerStats.AccelerationLowThresholdLvl3);
        }

        public virtual void LoadAccelerationMediumThreshold()
        {
            playerBehaviour.speedStats.AccelerationMediumThreshold =
                LoadAccelerationMediumThreshold(playerBehaviour.fuel.Level,
                    playerBehaviour.defaultPlayerStats.AccelerationMediumThresholdLvl1,
                    playerBehaviour.defaultPlayerStats.AccelerationMediumThresholdLvl2,
                    playerBehaviour.defaultPlayerStats.AccelerationMediumThresholdLvl3);
        }

        public virtual void LoadAccelerationOffRoadThreshold()
        {
            playerBehaviour.speedStats.AccelerationOffRoadThreshold =
                LoadAccelerationOffRoadThreshold(playerBehaviour.fuel.Level,
                    playerBehaviour.defaultPlayerStats.AccelerationOffRoadThresholdLvl1,
                    playerBehaviour.defaultPlayerStats.AccelerationOffRoadThresholdLvl2,
                    playerBehaviour.defaultPlayerStats.AccelerationOffRoadThresholdLvl3);
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

        public virtual void LoadDriftDashFrames()
        {
            playerBehaviour.speedStats.DriftDashFrames =
                LoadDriftDashFrames(playerBehaviour.extremeGearData.movementVars.DriftDashChargeDuration);
        }

        public virtual void LoadTurnSpeedLoss()
        {
            playerBehaviour.speedStats.TurnSpeedLoss =
                LoadTurnSpeedLoss(playerBehaviour.defaultPlayerStats.TurnSpeedLoss);
        }

        public virtual void LoadJumpChargeMinSpeed()
        {
            playerBehaviour.speedStats.JumpChargeMinSpeed =
                LoadJumpChargeMinSpeed(playerBehaviour.defaultPlayerStats.JumpChargeMinSpeedDefault);
        }

        public virtual void LoadJumpChargeDeceleration()
        {
            playerBehaviour.speedStats.JumpChargeDecelleration =
                LoadJumpChargeDecelleration(playerBehaviour.defaultPlayerStats.JumpChargeDecelerationDefault);
        }

        // TURNING
        public virtual void LoadTurnRate()
        {
            playerBehaviour.turnStats.Turnrate = LoadTurnRate(playerBehaviour.defaultPlayerStats.TurnrateDefault);
        }

        public virtual void LoadTurnRateMax()
        {
            playerBehaviour.turnStats.TurnRateMax = LoadTurnRateMax(playerBehaviour.defaultPlayerStats.TurnRateMax);
        }

        public virtual void LoadTurnLowSpeedMultiplier()
        {
            playerBehaviour.turnStats.TurnLowSpeedMultiplier =
                LoadTurnLowSpeedMultiplier(playerBehaviour.defaultPlayerStats.TurnLowSpeedMultiplierDefault);
        }

        public virtual void LoadTurnSpeedLossCurve()
        {
            playerBehaviour.turnStats.TurnSpeedLossCurve =
                LoadTurnSpeedLossCurve(playerBehaviour.defaultPlayerStats.TurnSpeedLossCurveDefault);
        }

        public virtual void LoadTurnRateCurve()
        {
            playerBehaviour.turnStats.TurnrateCurve =
                LoadTurnRateCurve(playerBehaviour.defaultPlayerStats.TurnrateCurveDefault);
        }

        public virtual void LoadDriftTurnRateMin()
        {
            playerBehaviour.turnStats.DriftTurnrateMin =
                LoadDriftTurnRateMin(playerBehaviour.defaultPlayerStats.DriftTurnrateMinDefault);
        }

        public virtual void LoadDriftTurnRateMax()
        {
            playerBehaviour.turnStats.DriftTurnrateMax =
                LoadDriftTurnRateMax(playerBehaviour.defaultPlayerStats.DriftTurnRateMaxDefault);
        }

        public virtual void LoadDriftTurnRate()
        {
            playerBehaviour.turnStats.DriftTurnrate =
                LoadDriftTurnRate(playerBehaviour.defaultPlayerStats.DriftTurnrateDefault);
        }

        //Jump
        public virtual void LoadJumpSpeedMax()
        {
            playerBehaviour.jumpStats.JumpSpeedMax =
                LoadJumpSpeedMax(playerBehaviour.defaultPlayerStats.JumpSpeedMaxDefault);
        }

        public virtual void LoadJumpAcceleration()
        {
            playerBehaviour.jumpStats.JumpAccelleration =
                LoadJumpAcceleration(playerBehaviour.defaultPlayerStats.JumpAccelDefault);
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

        public virtual void LoadMinRings() => playerBehaviour.fuelStats.MinRings =
            LoadMinRings(playerBehaviour.extremeGearData.fuelVars.MinRings);

        public virtual void LoadMaxRings() => playerBehaviour.fuelStats.MaxRings =
            LoadMaxRings(playerBehaviour.extremeGearData.fuelVars.MaxRings);

        public virtual void LoadLevelCap() => playerBehaviour.fuelStats.LevelCaps =
            LoadLevelCaps(playerBehaviour.extremeGearData.fuelVars.Level1Cap,
                playerBehaviour.extremeGearData.fuelVars.Level2Cap,
                playerBehaviour.extremeGearData.fuelVars.Level3Cap);

        #endregion

        protected void OnLevelChange(object sender, EventArgs e)
        {
            Debug.Log("Test Level stuff");
            LoadLevelAffectedStats();
        }
    }
}