using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    public abstract class FuelPack : MonoBehaviour
    {
        // TODO Implement FuelPack
        protected PlayerBehaviour _playerBehaviour;

        private void Start()
        {
            _playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        public virtual void AddRing()
        {
            
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
