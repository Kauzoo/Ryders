using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Nyr.UnityDev;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    /// <summary>
    /// Handles all things level, rings and fuel
    /// </summary>
    public abstract class FuelPack : MonoBehaviour, IRydersPlayerComponent
    {
        // TODO Implement FuelPack
        [SerializeReference] protected PlayerBehaviour _playerBehaviour;

        private void Start()
        {
        }

        public virtual void Setup()
        {
            GetComponentSafe.GetComponent(this, ref _playerBehaviour);
            _playerBehaviour = TryGetComponent<PlayerBehaviour>(out var playerBehaviour)
                ? playerBehaviour
                : throw new MissingReferenceException();
        }

        public virtual void Master()
        {
            DetermineLevel();
        }
        
        #region Rings

        public virtual void AddRing() =>
            _playerBehaviour.fuel.Rings =
                Math.Min(_playerBehaviour.fuel.Rings + 1, _playerBehaviour.fuelStats.MaxRings);

        public virtual void RemoveRing() =>
            _playerBehaviour.fuel.Rings =
                Math.Max(_playerBehaviour.fuel.Rings - 1, _playerBehaviour.fuelStats.MinRings);

        public virtual void AddRings(int rings) =>
            _playerBehaviour.fuel.Rings =
                Math.Min(_playerBehaviour.fuel.Rings + rings, _playerBehaviour.fuelStats.MaxRings);

        public virtual void RemoveRings(int rings) =>
            _playerBehaviour.fuel.Rings =
                Math.Max(_playerBehaviour.fuel.Rings - rings, _playerBehaviour.fuelStats.MinRings);

        public virtual void SetRingCount(int rings) =>
            _playerBehaviour.fuel.Rings = Math.Min(Math.Max(rings, _playerBehaviour.fuelStats.MinRings),
                _playerBehaviour.fuelStats.MaxRings);

        public virtual bool IsAtMaxRings() => _playerBehaviour.fuel.Rings == _playerBehaviour.fuelStats.MaxRings;

        public virtual bool IsAtMinRings() => _playerBehaviour.fuel.Rings == _playerBehaviour.fuelStats.MinRings;

        #endregion

        #region Level

        public virtual void DetermineLevel()
        {
            for (var i = 0; i < _playerBehaviour.fuelStats.LevelCaps.Length; i++)
            {
                if (_playerBehaviour.fuel.Rings >= _playerBehaviour.fuelStats.LevelCaps[i])
                    _playerBehaviour.fuel.Level = i + 1;
            }
        }

        public virtual void SetLevel(int level)
        {
        }

        public virtual void IncrementLevel()
        {
        }

        #endregion

        #region Fuel

        #endregion
    }
}