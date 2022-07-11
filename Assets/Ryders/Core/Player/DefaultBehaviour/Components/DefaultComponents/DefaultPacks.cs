using Ryders.Core.Player.ExtremeGear.Movement;
using UnityEngine;
using static Ryders.Core.Player.DefaultBehaviour.Components.DefaultComponents.DefaultPacks;

namespace Ryders.Core.Player.DefaultBehaviour.Components.DefaultComponents
{
    // TODO Implement overrides for testing
    internal static class DefaultPacks
    {
        internal static void PrintDefaultPackWarning(UnityEngine.Object sender, string name)
        {
            Debug.LogWarning($"@{sender} is using {name}");
        }
    }
    
    internal sealed class AccelerationPackDefault : AccelerationPack
    {
        public override void Setup()
        {
            base.Setup();
            PrintDefaultPackWarning(playerBehaviour, "AccelerationPackDefault");
        }
    }

    internal sealed class BoostPackDefault : BoostPack
    {
        public override void Setup()
        {
            base.Setup();
            PrintDefaultPackWarning(playerBehaviour, "BoostPackDefault");
        }
    }

    internal sealed class CorneringPackDefault : CorneringPack
    {
        public override void Setup()
        {
            base.Setup();
            PrintDefaultPackWarning(playerBehaviour, "CorneringPackDefault");
        }
    }

    internal sealed class DriftPackDefault : DriftPack
    {
        public override void Setup()
        {
            base.Setup();
            PrintDefaultPackWarning(playerBehaviour, "DriftPackDefault");
        }
    }

    internal sealed class EventPublisherPackDefault : EventPublisherPack
    {
        public override void Setup()
        {
            base.Setup();
            PrintDefaultPackWarning(playerBehaviour, "EventPublisherPackDefault");
        }
    }

    internal sealed class FuelPackDefault : FuelPack
    {
        public override void Setup()
        {
            base.Setup();
            PrintDefaultPackWarning(playerBehaviour, "FuelPackDefault");
        }
    }

    internal sealed class GravityPackDefault : GravityPack
    {
        public override void Setup()
        {
            base.Setup();
            PrintDefaultPackWarning(playerBehaviour, "GravityPackDefault");
        }
    }

    internal sealed class JumpPackDefault : JumpPack
    {
        public override void Setup()
        {
            base.Setup();
            PrintDefaultPackWarning(playerBehaviour, "JumpPackDefault");
        }
    }
    
    internal sealed class StatLoaderPackDefault : StatLoaderPack
    {
        public override void Setup()
        {
            base.Setup();
            PrintDefaultPackWarning(playerBehaviour, "StatLoaderPackDefault");
        }
    }

    internal sealed class WallCollisionPackDefault : WallCollisionPack
    {
        public override void Setup()
        {
            base.Setup();
            PrintDefaultPackWarning(playerBehaviour, "WallCollisionPackDefault");
        }
    }
}