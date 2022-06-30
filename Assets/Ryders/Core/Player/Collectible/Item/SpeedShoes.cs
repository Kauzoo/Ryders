using System;
using Ryders.Core.Player.DefaultBehaviour;
using Unity.VisualScripting;
using UnityEngine;

namespace Ryders.Core.Player.Collectible.Item
{
    public class SpeedShoes : Item
    {
        [SerializeField, Tooltip("The amount of additive Speed applied to the player") ] protected float SpeedBoost;

        private PlayerBehaviour _playerBehaviour;

        public override void ApplyItemEffect(PlayerBehaviour target)
        {
            _playerBehaviour = target;
            ApplySpeedBoost();
        }

        protected virtual void ApplySpeedBoost()
        {
            _playerBehaviour.accelerationPack.AddAdditiveSpeedSingleExternal(SpeedBoost);
        }
    }
}