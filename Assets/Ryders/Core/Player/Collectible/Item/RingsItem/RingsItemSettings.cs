using UnityEngine;

namespace Ryders.Core.Player.Collectible.Item
{
    [CreateAssetMenu(fileName = "RingsItemSettings", menuName = "ScriptableObjects/ItemSettings/RingsItemSettings", order = 0)]
    public class RingsItemSettings : ScriptableObject
    {
        [SerializeField, Tooltip("Amount of Rings granted by the Pickup")] public int RingCount;
        [Tooltip("The Icon representing the ItemType inside the Box")] public Material RingsItemMaterial;
    }
}