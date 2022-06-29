using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Ryders.Core.Player.Collectible.Item
{
    public class SpeedShoes : Item
    {
        [SerializeField, Tooltip("The amount of additive Speed applied to the player") ] private float SpeedBoost;


        public override void ApplyItemEffect(dynamic target)
        {
            
        }
    }
}