#nullable enable
using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Nyr.UnityDev.Util;
using Ryders.Core.Player.DefaultBehaviour.Components;
using Ryders.Core.Player.DefaultBehaviour.Components.DefaultComponents;
using Ryders.Core.Player.ExtremeGear.Movement;


namespace Ryders.Core.Player.DefaultBehaviour
{
    public abstract partial class PlayerBehaviour
    {
        /// <summary>
        /// This does not work as intended right now
        /// </summary>
        [ContextMenu("Setup with DefaultPacks")]
        private void SetupDefaultComponents()
        {
            accelerationPack = this.SafeGetComponentNullable<AccelerationPack>() ??
                               gameObject.AddComponent<AccelerationPackDefault>();
            statLoaderPack = this.SafeGetComponentNullable<StatLoaderPack>() ??
                             gameObject.AddComponent<StatLoaderPackDefault>();
            gravityPack = this.SafeGetComponentNullable<GravityPack>() ?? gameObject.AddComponent<GravityPackDefault>();
            jumpPack = this.SafeGetComponentNullable<JumpPack>() ?? gameObject.AddComponent<JumpPackDefault>();
            wallCollisionPack = this.SafeGetComponentNullable<WallCollisionPack>() ??
                                gameObject.AddComponent<WallCollisionPackDefault>();
            fuelPack = this.SafeGetComponentNullable<FuelPack>() ?? gameObject.AddComponent<FuelPackDefault>();
            driftPack = this.SafeGetComponentNullable<DriftPack>() ?? gameObject.AddComponent<DriftPackDefault>();
            boostPack = this.SafeGetComponentNullable<BoostPack>() ?? gameObject.AddComponent<BoostPackDefault>();
            corneringPack = this.SafeGetComponentNullable<CorneringPack>() ??
                            gameObject.AddComponent<CorneringPackDefault>();
            eventPublisherPack = this.SafeGetComponentNullable<EventPublisherPack>() ??
                                 gameObject.AddComponent<EventPublisherPackDefault>();
        }
        
        public static bool IsPlayerBehaviour(dynamic? input) => (input ?? new Object()) is PlayerBehaviour;

        public static PlayerBehaviour DynamicToPlayerBehaviour(dynamic input) =>
            IsPlayerBehaviour(input) ? (PlayerBehaviour)input : throw new InvalidCastException();

        public static bool TryToPlayerBehaviour(dynamic input, out PlayerBehaviour? playerBehaviourOut)
        {
            if (IsPlayerBehaviour(input))
            {
                playerBehaviourOut = (PlayerBehaviour)input;
                return true;
            }

            playerBehaviourOut = null;
            return false;
        }

        public static (bool HasValue, PlayerBehaviour? playerBehaviour) TryToPlayerBehaviour(dynamic input) =>
            IsPlayerBehaviour(input) ? (true, (PlayerBehaviour)input) : (false, null);

        public static PlayerBehaviour? GetPlayerBehaviour(MonoBehaviour source) =>
            source.TryGetComponent<PlayerBehaviour>(out var playerBehaviour) ? playerBehaviour : null;
    }
}