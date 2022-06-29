using UnityEngine;

namespace Ryders.Core.Player.Collectible.Item
{
    [CreateAssetMenu(fileName = "ItemBoxSettings", menuName = "ScriptableObjects/ItemBoxSettings", order = 0)]
    public class ItemBoxSettings : ScriptableObject
    {
        [Header("Settings"), Tooltip("Toggle ItemBox as Respawning")]
        public bool respawn;
        [Tooltip("Respawn Timer in Seconds")]
        public float respawnTimer;
        
        [Header("ItemSettings")] public Item[] itemsDefault;
        public float[] oddsDefault;
    }
}