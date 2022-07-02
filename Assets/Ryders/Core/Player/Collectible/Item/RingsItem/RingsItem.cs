using System;
using Ryders.Core.Player.DefaultBehaviour;
using UnityEngine;

namespace Ryders.Core.Player.Collectible.Item
{
    public class RingsItem : Item
    {
        public RingsItemSettings settings;
        private PlayerBehaviour _playerBehaviour;

        private void Awake()
        {
            ItemMaterial = settings.RingsItemMaterial;
        }

        public override void ApplyItemEffect(PlayerBehaviour target)
        {
            _playerBehaviour = target;
            ApplyRingGain();
        }

        protected virtual void ApplyRingGain()
        {
            _playerBehaviour.fuelPack.AddRings(settings.RingCount);
        }
    }
}