using Ryders.Core.Player.DefaultBehaviour;
using UnityEngine;

namespace Ryders.Core.Player.Collectible.Item
{
    public abstract class Item : MonoBehaviour
    {
        public abstract void ApplyItemEffect(PlayerBehaviour target);
    }
}