using UnityEngine;

namespace Ryders.Core.Player.Collectible.Item.SpeedShoesItem
{
    [CreateAssetMenu(fileName = "SpeedShoesSettings", menuName = "ScriptableObjects/ItemSettings/SpeedShoesSettings", order = 0)]
    public class SpeedShoesSettings : ScriptableObject
    {
        [SerializeField, Tooltip("The amount of additive Speed applied to the player") ] public float SpeedBoost;
        [Tooltip("The Icon representing the ItemType inside the Box")] public Material SpeedShoesMaterial;
    }
}