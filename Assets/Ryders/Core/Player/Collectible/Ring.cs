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
            Debug.LogWarning("Test");
        }

        private void OnTriggerEnter(UnityEngine.Collider other)
        {
            Debug.LogError("Was geht?");
            StartCoroutine(Respawn());
        }

        private IEnumerator Respawn()
        {
            Debug.LogWarning("wtf");
            _meshRenderer.enabled = false;
            _collider.enabled = false;
            Debug.LogWarning("pls start");
            yield return new WaitForSeconds(3.0f);
            _meshRenderer.enabled = true;
            _collider.enabled = true;
            Debug.LogWarning("Do i go here?");
            //gameObject.SetActive(true);
        }
    }
}