using System;
using System.Collections;
using Ryders.Core.Player.Collectible.Item;
using Ryders.Core.Player.DefaultBehaviour;
using UnityEngine;
using static Nyr.UnityDev.Component.GetComponentSafe;
using Nyr.UnityDev.Component;

namespace Ryders.Core.Player.Collectible
{
    /// <summary>
    /// Behaviour for Ring collectibles
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Ring : MonoBehaviour, IPlayerCollectible
    {
        [Header("Settings")]
        [Tooltip("Toggle Ring as respawning")]
        public bool respawn;
        [Tooltip("Respawn Timer in Seconds")]
        public float respawnTimer;
        
        private Collider _collider;
        private MeshRenderer _meshRendererTorus;

        private void Start()
        {
            Setup();
        }

        public virtual void Setup()
        {
            _collider = GetComponent<Collider>();
            GetComponentSafe.SafeGetComponentInChildren(this, ref _meshRendererTorus);
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(UnityEngine.Collider other)
        {
            if (!other.gameObject.TryGetComponent<PlayerBehaviour>(out var playerBehaviour)) return;
            playerBehaviour.fuelPack.AddRing();
            if(respawn)
                StartCoroutine(Respawn());
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