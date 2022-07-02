using System;
using System.Collections;
using Ryders.Core.Player.Collectible.Item;
using Ryders.Core.Player.DefaultBehaviour;
using UnityEditor;
using UnityEngine;

namespace Ryders.Core.Player.Collectible
{
    /// <summary>
    /// Behaviour for Ring collectibles
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Ring : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("Toggle Ring as respawning")]
        public bool respawn;
        [Tooltip("Respawn Timer in Seconds")]
        public float respawnTimer;

        private float _respawnTimer;
        private Collider _collider;
        private MeshRenderer _meshRendererTorus;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _meshRendererTorus = GetComponentInChildren<MeshRenderer>();

            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(UnityEngine.Collider other)
        {
            if (other.gameObject.TryGetComponent<PlayerBehaviour>(out var playerBehaviour))
            {
                playerBehaviour.fuelPack.AddRing();
                if(respawn)
                    StartCoroutine(Respawn());
            }
            
        }

        private IEnumerator Respawn()
        {
            _meshRendererTorus.enabled = false;
            _collider.enabled = false;
            yield return new WaitForSeconds(respawnTimer);
            _meshRendererTorus.enabled = true;
            _collider.enabled = true;
        }
    }
}