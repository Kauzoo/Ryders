using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Nyr.UnityDev.Component;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    /// <summary>
    /// Handles all things level, rings and fuel
    /// </summary>
    [RequireComponent(typeof(PlayerBehaviour))]
    public abstract class FuelPack : MonoBehaviour, IRydersPlayerComponent, IRydersPlayerEventPublisher
    {
        // TODO Implement FuelPack
        protected PlayerBehaviour playerBehaviour;
        
        public event EventHandler SpeedBoostEvent;
        public event EventHandler LevelUpEvent;
        public event EventHandler LevelDownEvent;
        public event EventHandler LevelChangeEvent;

        private void Start()
        {
            Setup();
        }
        
        [ContextMenu("Setup References")]
        public virtual void Setup()
        {
            GetComponentSafe.SafeGetComponent(this, ref playerBehaviour);
        }

        public virtual void Master()
        {
            DetermineLevel();
        }
        
        #region Rings

        public virtual void AddRing() =>
            playerBehaviour.fuel.Rings =
                Math.Min(playerBehaviour.fuel.Rings + 1, playerBehaviour.fuelStats.MaxRings);

        public virtual void RemoveRing() =>
            playerBehaviour.fuel.Rings =
                Math.Max(playerBehaviour.fuel.Rings - 1, playerBehaviour.fuelStats.MinRings);

        public virtual void AddRings(int rings) =>
            playerBehaviour.fuel.Rings =
                Math.Min(playerBehaviour.fuel.Rings + rings, playerBehaviour.fuelStats.MaxRings);

        public virtual void RemoveRings(int rings) =>
            playerBehaviour.fuel.Rings =
                Math.Max(playerBehaviour.fuel.Rings - rings, playerBehaviour.fuelStats.MinRings);

        public virtual void SetRingCount(int rings) =>
            playerBehaviour.fuel.Rings = Math.Min(Math.Max(rings, playerBehaviour.fuelStats.MinRings),
                playerBehaviour.fuelStats.MaxRings);

        public virtual bool IsAtMaxRings() => playerBehaviour.fuel.Rings == playerBehaviour.fuelStats.MaxRings;

        public virtual bool IsAtMinRings() => playerBehaviour.fuel.Rings == playerBehaviour.fuelStats.MinRings;

        #endregion

        #region Level

        public virtual void DetermineLevel()
        {
            for (var i = 0; i < playerBehaviour.fuelStats.LevelCaps.Length; i++)
            {
                var level = -1;
                if (playerBehaviour.fuel.Rings >= playerBehaviour.fuelStats.LevelCaps[i])
                    level = i + 1;
                if(level != playerBehaviour.fuel.Level)
                    RaiseLevelChangeEvent(EventArgs.Empty);
            }
        }

        public virtual void SetLevel(int level)
        {
            RaiseLevelChangeEvent(EventArgs.Empty);
        }

        public virtual void IncrementLevel()
        {
            RaiseLevelChangeEvent(EventArgs.Empty);
        }

        public virtual void RaiseLevelChangeEvent(EventArgs e)
        {
            var raiseEvent = LevelChangeEvent;
            raiseEvent?.Invoke(this, e);
        }

        #endregion

        #region Fuel

        #endregion
    }
}