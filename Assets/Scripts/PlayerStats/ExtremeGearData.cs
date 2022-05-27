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
            public int BoostSpeedLvl1;
            public int BoostSpeedLvl2;
            public int BoostSpeedLvl3;
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
            public int DriftDashSpeedLvl1;
            public int DriftDashSpeedLvl2;
            public int DriftDashSpeedLvl3;
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

            /**
             * Other necesary stats that are board based, but not listed in the SRDX Datasheets
             */

            [Header("Acceleration")]
            /// <summary>
            /// HiddenStat
            /// Threshold for FastAccel
            /// </summary>
            public int MinSpeed;
            /// <summary>
            /// HiddenStat
            /// The rate at which the player accelerates while below their MinSpeed value
            /// </summary>
            public int FastAcceleration;

            [Header("Turning")]
            /// <summary>
            /// HiddenStat
            /// This is proabaly in the og game but kinda cryptic.
            /// Base Number affecting how fast the Board turns.
            /// </summary>
            public float Turnrate;
            /// <summary>
            /// HiddenStat
            /// Curve determening the speed lost while turning, based of the current speed
            /// </summary>
            public AnimationCurve TurnSpeedLossCurve;
            /// <summary>
            /// HiddenStat
            /// Determines the ratio of Turning relative to current speed.
            /// This stat is not based of the og game afaik
            /// </summary>
            public AnimationCurve TurnrateCurve;

            /// <summary>
            /// HiddenStat
            /// Rate at which the player passively turns towards a direction while drifting
            /// </summary>
            [Header("Drift")]
            public float DriftTurnratePassive;
            /// <summary>
            /// HiddenStat
            /// Minimum amount the player is able to turn while drifting
            /// </summary>
            public float DriftTurnrateMin;
            /// <summary>
            /// HiddenStat
            /// The turnrate used while drifting (if the player inputs a direction)
            /// </summary>
            public float DriftTurnrate;

            /// <summary>
            /// HiddenStat
            /// Rate of deceleration while breakeing
            /// </summary>
            [Header("Breake")]
            public float BreakeDeceleration;


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
            public Enums.FuelType Fuel;
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

        public MovementVars movementVars = new MovementVars();
        public FuelVars fuelVars = new FuelVars();
    }
}
