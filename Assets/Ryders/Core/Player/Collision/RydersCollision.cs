using System;
using UnityEngine;

namespace Ryders.Core.Player.Collision
{
    // TODO consider doing this with tags
    [RequireComponent(typeof(Collider))]
    public abstract class RydersCollision : MonoBehaviour
    {
        protected Collider collider;

        public void Start()
        {
            collider = GetComponent<Collider>();
        }
        
        
    }

    public enum BonkType
    {
        SoftBonk, HardBonk, CarBonk, Wall
    }
}