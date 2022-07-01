using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    /// <summary>
    /// Handles all things level, rings and fuel
    /// </summary>
    public abstract class FuelPack : MonoBehaviour
    {
        // TODO Implement FuelPack
        protected PlayerBehaviour _playerBehaviour;

        [Header("RingsSettings")]
        public int MaxRings;

        public int Levels;
        public int[] LevelCaps;


        private void Start()
        {
            _playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        public virtual void AddRing()
        {
            _playerBehaviour.fuel.Rings++;
        }

        public virtual void RemoveRing()
        {
            
        }

        public virtual void AddRings()
        {
            
        }

        public virtual void RemoveRings()
        {
            
        }

        public virtual void SetRingCount()
        {
            
        }
    }
}
