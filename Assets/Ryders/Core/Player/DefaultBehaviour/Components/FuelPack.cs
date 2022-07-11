using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using static Nyr.UnityDev.Component.GetComponentSafe;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    /// <summary>
    /// Handles all things level, rings and fuel
    /// </summary>
    [RequireComponent(typeof(PlayerBehaviour))]
    [RequireComponent(typeof(EventPublisherPack))]
    public abstract class FuelPack : MonoBehaviour, IRydersPlayerComponent
    {
        // TODO Implement FuelPack
        // ReSharper disable once MemberCanBePrivate.Global
        protected PlayerBehaviour playerBehaviour;

        // ReSharper disable once MemberCanBePrivate.Global
        protected EventPublisherPack eventPublisherPack;

        private void Start()
        {
            Setup();
        }

        [ContextMenu("Setup References")]
        public virtual void Setup()
        {
            SafeGetComponent(this, ref playerBehaviour);
            SafeGetComponent(this, ref eventPublisherPack);
        }

        public virtual void Master()
        {
        }

        #region Rings

        public virtual void AddRing()
        {
            playerBehaviour.fuel.Rings =
                Math.Min(playerBehaviour.fuel.Rings + 1, playerBehaviour.fuelStats.MaxRings);
            DetermineLevel();
        }
        
        public virtual void RemoveRing()
        {
            playerBehaviour.fuel.Rings =
                Math.Max(playerBehaviour.fuel.Rings - 1, playerBehaviour.fuelStats.MinRings);
            DetermineLevel();
        }
        
        public virtual void AddRings(int rings)
        {
            playerBehaviour.fuel.Rings =
                Math.Min(playerBehaviour.fuel.Rings + rings, playerBehaviour.fuelStats.MaxRings);
            DetermineLevel();
        }
        
        public virtual void RemoveRings(int rings)
        {
            playerBehaviour.fuel.Rings =
                Math.Max(playerBehaviour.fuel.Rings - rings, playerBehaviour.fuelStats.MinRings);
            DetermineLevel();
        }
        
        public virtual void SetRingCount(int rings)
        {
            playerBehaviour.fuel.Rings = Math.Min(Math.Max(rings, playerBehaviour.fuelStats.MinRings),
                playerBehaviour.fuelStats.MaxRings);
            DetermineLevel();
        }
        
        public virtual bool IsAtMaxRings() => playerBehaviour.fuel.Rings == playerBehaviour.fuelStats.MaxRings;

        public virtual bool IsAtMinRings() => playerBehaviour.fuel.Rings == playerBehaviour.fuelStats.MinRings;

        #endregion

        #region Level

        protected virtual void DetermineLevel()
        {
            var level = -1;
            for (var i = 0; i < playerBehaviour.fuelStats.LevelCaps.Length; i++)
            {
                if (playerBehaviour.fuel.Rings >= playerBehaviour.fuelStats.LevelCaps[i])
                    level = i + 1;
            }
            if (level == playerBehaviour.fuel.Level) return;
            playerBehaviour.fuel.Level = level;
            eventPublisherPack.RaiseLevelChangeEvent(EventArgs.Empty);
        }

        public virtual void SetLevel(int level)
        {
            eventPublisherPack.RaiseLevelChangeEvent(EventArgs.Empty);
        }

        public virtual void IncrementLevel()
        {
            eventPublisherPack.RaiseLevelChangeEvent(EventArgs.Empty);
        }

        #endregion

        #region Fuel

        #endregion
    }
}