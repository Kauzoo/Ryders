using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using Ryders.Core.Player.Collectible.Item;
using Ryders.Core.Player.DefaultBehaviour;
using Random = UnityEngine.Random;

namespace Ryders.Core.Player.Collectible
{
    [RequireComponent(typeof(Collider))]
    public class ItemBox : MonoBehaviour
    {
        // TODO Loot tables should be based of place and distance to opponent
        public ItemBoxSettings ItemBoxSettings;
        [Header("SpawnSettings")]
        [Tooltip("Toggle ItemBox as Respawning")]
        public bool respawn;
        [Tooltip("Respawn Timer in Seconds")] public float respawnTimer;

        [Tooltip("All meshes that should stop being rendered when the Item Box is collected")]
        public MeshRenderer[] meshes;

        [Header("ItemSettings")] [Serialize] public Item.Item[] itemsDefault;
        [Serialize] public float[] oddsDefault;

        private Item.Item _currentItem;
        private readonly Dictionary<Item.Item, Vector2> itemDictionary = new();
        private Collider _collider;

        /**
         * CONSTANTS
         */
        private const float OddsSumTolerance = 0.01f;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
            Setup();
        }

        /// <summary>
        /// Setup Item Determination
        /// </summary>
        private void Setup()
        {
            if (ItemBoxSettings != null)
            {
                respawn = ItemBoxSettings.respawn;
                respawnTimer = ItemBoxSettings.respawnTimer;
                itemsDefault = ItemBoxSettings.itemsDefault;
                oddsDefault = ItemBoxSettings.oddsDefault;
                Debug.Log("Using ItemBoxSettings overwrite.", this);
            }
            if (itemsDefault.Length != oddsDefault.Length)
            {
                Debug.LogWarning($"Amount of Items and Odds should match", this);
            }

            if (Math.Abs(oddsDefault.Sum() - 1f) > OddsSumTolerance)
            {
                Debug.LogWarning(
                    $"Sum of odds should be 1. Sum: {oddsDefault.Sum()} was not within Tolerance: {OddsSumTolerance}",
                    this);
            }

            var oddsRangeArray = new Vector2[itemsDefault.Length];
            switch (itemsDefault.Length)
            {
                case 0:
                    Debug.LogWarning("ItemsArray should not be empty");
                    break;
                case 1:
                    oddsRangeArray[0] = new Vector2(0f, 1f);
                    break;
                default:
                    oddsRangeArray[0] = new Vector2(0f, oddsDefault[0]);
                    oddsRangeArray[oddsDefault.Length - 1] = new Vector2(oddsRangeArray[oddsDefault.Length - 2].y, 1f);
                    break;
            }

            for (var i = 1; i < oddsRangeArray.Length - 1; i++)
            {
                var oddsRange = new Vector2(oddsRangeArray[i - 1].y, oddsRangeArray[i - 1].y + oddsDefault[i]);
                oddsRangeArray[i] = oddsRange;
            }

            for (var i = 0; i < itemsDefault.Length; i++)
            {
                itemDictionary.Add(itemsDefault[i], oddsRangeArray[i]);
            }

            DetermineItem();
        }

        /// <summary>
        /// Determine which Item to spawn in 
        /// </summary>
        private void DetermineItem()
        {
            var value = Random.value;
            foreach (var entry in itemDictionary)
            {
                if (value >= entry.Value.x && value <= entry.Value.y)
                {
                    _currentItem = entry.Key;
                    break;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<PlayerBehaviour>(out var playerBehaviour))
            {
                DeSpawn();
                _currentItem.ApplyItemEffect(playerBehaviour);
                if (respawn)
                {
                    StartCoroutine(Respawn());  
                }
            }
        }

        private void DeSpawn()
        {
            _collider.enabled = false;
            foreach (var meshRenderer in meshes)
            {
                meshRenderer.enabled = false;
            }
        }

        /// <summary>
        /// Handle De- and Respawning of the Item Box
        /// </summary>
        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(respawnTimer);
            DetermineItem();
            _collider.enabled = true;
            foreach (var meshRenderer in meshes)
            {
                meshRenderer.enabled = true;
            }
        }

        public Item.Item GetCurrentItem()
        {
            return _currentItem;
        }
    }
}