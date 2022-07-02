using System;
using Ryders.Core.Player.Collectible.Item.SpeedShoesItem;
using Ryders.Core.Player.DefaultBehaviour;
using Unity.VisualScripting;
using UnityEngine;

namespace Ryders.Core.Player.Collectible.Item
{
    public class SpeedShoes : Item
    {
        public SpeedShoesSettings settings;

        private PlayerBehaviour _playerBehaviour;

        private void Awake()
        {
            ItemMaterial = settings.SpeedShoesMaterial;
        }
        
        public override void ApplyItemEffect(PlayerBehaviour target)
        {
            _playerBehaviour = target;
            ApplySpeedBoost();
        }

        protected virtual void ApplySpeedBoost()
        {
            Debug.Log("SpeedBoostAmount: " + settings.SpeedBoost);
            _playerBehaviour.accelerationPack.AddAdditiveSpeedSingleExternal(settings.SpeedBoost);
        }
    }
}