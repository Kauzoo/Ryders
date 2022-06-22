using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryders.Core.Player.ExtremeGear
{
    /// <summary>
    /// SRDX
    /// </summary>
    public enum Form
    {
        Board, Skates, Bike
    }

    /// <summary>
    /// SRDX
    /// This might be reworked into classes
    /// </summary>
    public enum GearArchetype
    {
        Standard, Cruising, HighBoosting, Combat, Drift, Special
    }

    public enum FuelType
    {
        Air, Ring
    }

    [CreateAssetMenu(fileName = "ExtremeGearData", menuName = "ScriptableObjects/ExtremeGearData", order = 1)]
    public class ExtremeGearData : ScriptableObject
    {
        /**
         * Everything that is classified as HiddenStat gets it's default value set by DefaultPlayerStats.
         * This means all HiddenStats are offsets from that default value. (Except AnimationCurves)
         */
        [System.Serializable]
        public class BasicInfo
        {
            public string Icon;
            public string Name;
            public Form Form;
            public GearArchetype GearArchetype1;
            public GearArchetype GearArchetype2;
        }

        /// <summary>
        /// Contains Movement variables that are not level dependent
        /// </summary>
        [System.Serializable]
        public class MovementVars
        {
            /***
             * All Stats in this category are based of the gear stats breakdown in the SRDX Datasheets
             */
            /// <summary>
            /// Determines the players usual cruising speed. This value is additive with the Character Based TopSpeed Value.
            /// Also stacks additively with TopSpeed Bonus from LevelUp
            /// </summary>
            [Header("SpeedStats")]
            public int TopSpeed;
            /// <summary>
            /// SRDX
            /// The Speed to which the player boosts. MaxSpeed is set to boost speed while boosting.
            /// Stat changes with Level. Stacks additively with <see cref="CharacterBase.CharacterStatsMovement.BoostSpeed"/> from <see cref="CharacterBase"/>
            /// </summary>
            public float BoostSpeedLvl1;
            public float BoostSpeedLvl2;
            public float BoostSpeedLvl3;
            /// <summary>
            /// SRDX
            /// The percentage Modifier used when performing a BoostChain.
            /// Value Additively stacks with the character Based BoostChainModifier.
            /// </summary>
            public float BoostChainModifier;
            /// <summary>
            /// SRDX
            /// Speed player gains from a DriftDash. This amount stacks ontop of the current speed (as long as it's below the DriftCap).
            /// Stat is affected by Level and additively stacks with the Drift stat from character.
            /// </summary>
            public float DriftDashSpeedLvl1;
            public float DriftDashSpeedLvl2;
            public float DriftDashSpeedLvl3;
            /// <summary>
            /// SRDX
            /// Player can not you DriftDash to Boost past this speed, but also can not lose Speed by boosting while above cap.
            /// Additively stacks with the Drift Value from character and the values from LevelUps. (+10 per level)
            /// </summary>
            public int DriftCap;
            /// <summary>
            /// SRDX
            /// Minimum duration the player needs to be drifting for to gain a DriftDash upon releasing the drift. Measured in seconds.
            /// Stat is originally called DriftDashFrames and is orginally measured in Frames.
            /// </summary>
            public int DriftDashChargeDuration;
            /// <summary>
            /// This is an offset
            /// Negative values = slower max speed but better turning
            /// Positive values = higher max speed but worse turning
            /// </summary>
            public float SpeedHandlingMultiplier;

            /// <summary>
            /// How fast the gear Turns towards TurnRateMax
            /// Sewer56.SonicRiders TurnAcceleration
            /// </summary>
            [Header("Turning")]
            public float TurnRate;
            /// <summary>
            /// The maximum TurnRate that can be achieved
            /// Sewer56.SonicRiders TurnMaxRadius
            /// </summary>
            public float TurnRateMax;
            /// <summary>
            /// Not sure how it works
            /// supposedly lets you turn better a low speeds
            /// </summary>
            public float TurnLowSpeedMultiplier;
            /// <summary>
            /// Influences the amount of speed lost while turning
            /// </summary>
            public float TurnSpeedLoss;
            
            /// <summary>
            /// HiddenStat
            /// Minimum amount the player is able to turn while drifting
            /// </summary>
            [Header("Drift")]
            public float DriftTurnRateMin;
            /// <summary>
            /// HiddenStat
            /// Maximum amount the player is able to turn while drifting
            /// </summary>
            public float DriftTurnRateMax;
            /// <summary>
            /// HiddenStat
            /// The TurnRate used while drifting (if the player inputs a direction)
            /// </summary>
            public float DriftTurnRate;
            /// <summary>
            /// How much your momentum follows you during a drift.
            /// (Basically how much your current angle and velocity affects the drift by decreasing
            /// how much you can turn. Higher = turn less).
            /// </summary>
            public float DriftMomentum;

            /// <summary>
            /// HiddenStat
            /// Rate of deceleration while breaking
            /// </summary>
            [Header("Break")]
            public float BreakDecelerationMultiplier;
            
            /// <summary>
            /// HiddenStat
            /// The maximum amount of speed the player hits while jumping
            /// </summary>
            [Header("Jump")]
            public float JumpSpeedMax;          // Controls jump speed relative to time
            /// <summary>
            /// HiddenStat
            /// Controls the acceleration behaviour while jumping
            /// </summary>
            public AnimationCurve JumpAccel;    // Acceleration for a jump
            /// <summary>
            /// HiddenStat
            /// The value the player the decelerates towards while charging a jump
            /// </summary>
            public float JumpChargeMinSpeed;
            /// <summary>
            /// HiddenStat
            /// Rate of deceleration while charging jump
            /// </summary>
            public float JumpChargeDeceleration;

            /// <summary>
            /// HiddenStat
            /// Acceleration whith which the player falls while affected by gravity
            /// </summary>
            [Header("Gravity")]
            public float GravityMultiplier;

            /// <summary>
            /// HiddenStat
            /// Duration for which the backwards movement while bumping of a wall lasts
            /// </summary>
            [Header("WallBump")]
            public float wallBumpTimer;
            /// <summary>
            /// HiddenStat
            /// Speedmultiplier for bumping of walls
            /// </summary>
            public float wallBumpSpeed;
        }

        [System.Serializable]
        public class FuelVars
        {
            /// <summary>
            /// SRDX
            /// Determines the type of Fuel used by a gear
            /// </summary>
            public FuelType Fuel;
            /// <summary>
            /// SRDX
            /// Multiplier to PassiveAirDrain while charging jump
            /// Is applied multiplicatively
            /// </summary>
            [Header("Jump")]
            public float JumpChargeMultiplier;
            /// <summary>
            /// SRDX
            /// Multiplier for the amount of air gained from tricks
            /// Value is a percentage that is applied to base amounts
            /// </summary>
            [Header("FuelGain")]
            public float TrickFuelGain;
            /// <summary>
            /// SRDX
            /// Multiplier for amount of air gained from type shortcuts
            /// Value is a percentage that is applied to base amounts
            /// </summary>
            public float TypeFuelGain;
            /// <summary>
            /// SRDX
            /// Multiplier for amount of air gained from QTEs
            /// Value is a percentage that is applied to base amounts
            /// </summary>
            public float QTEFuelGain;
            /// <summary>
            /// SRDX
            /// Amount of passive air drain. Scales with Level. Measured in Fuel per second
            /// Value is a total value
            /// </summary>
            [Header("PassiveDrain")]
            public float PassiveDrainLvl1;
            public float PassiveDrainLvl2;
            public float PassiveDrainLvl3;
            /// <summary>
            /// SRDX
            /// Size of the FuelTank. Scales with Level.
            /// Value is a toatl value
            /// </summary>
            [Header("FuelTankSize")]
            public int FuelTankSizeLevel1;
            public int FuelTankSizeLevel2;
            public int FuelTankSizeLevel3;
            /// <summary>
            /// SRDX
            /// Cost of performing a boost. Scales with Level
            /// Value is a total value.
            /// </summary>
            [Header("Boost")]
            public float BoostCostLvl1;
            public float BoostCostLvl2;
            public float BoostCostLvl3;
            /// <summary>
            /// SRDX
            /// FuelDrain while drifting. Scales with Level. Measured in Fuel per Second
            /// Value is a total value
            /// </summary>
            [Header("Drift")]
            public float DriftCostLvl1;
            public float DriftCostLvl2;
            public float DriftCostLvl3;
            /// <summary>
            /// SRDX
            /// Cost for performing a Tornado. Scales with Level.
            /// Value is a toatl value
            /// </summary>
            [Header("Tornado")]
            public float TornadoCostLvl1;
            public float TornadoCostLvl2;
            public float TornadoCostLvl3;

        }

        public BasicInfo basicInfo = new BasicInfo();
        public MovementVars movementVars = new MovementVars();
        public FuelVars fuelVars = new FuelVars();
    }
}
