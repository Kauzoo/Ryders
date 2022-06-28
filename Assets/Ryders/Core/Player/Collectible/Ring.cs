using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Ryders.Core.Player.Collectible
{
    /// <summary>
    /// Behaviour for Ring collectibles
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Ring : MonoBehaviour
    {
        [Header("Settings"), Tooltip("Toggle Ring as respawning")]
        public bool respawn;
        [Tooltip("Respawn Timer in Seconds")]
        public float respawnTimer;

        private float _respawnTimer;
        private Collider _collider;
        private MeshRenderer _meshRenderer;

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _meshRenderer = GetComponent<MeshRenderer>();

            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(UnityEngine.Collider other)
        {
            StartCoroutine(Respawn());
        }

        private IEnumerator Respawn()
        {
            _meshRenderer.enabled = false;
            _collider.enabled = false;
            yield return new WaitForSeconds(respawnTimer);
            _meshRenderer.enabled = true;
            _collider.enabled = true;
        }
    }
}