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
    public class Ring : MonoBehaviour
    {
        [Header("Settings"), Tooltip("Toggle Ring as respawning")]
        public bool respawn;
        [Tooltip("Respawn Timer in Seconds")]
        public float respawnTimer;

        private float _respawnTimer;

        private Collider _collider;

        private void Start()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnCollisionEnter(UnityEngine.Collision collision)
        {
            Debug.Log("Was geht?");
            gameObject.SetActive(false);
            StartCoroutine(Respawn());
        }
        
        private IEnumerator Respawn()
        {
            yield return new WaitForSecondsRealtime(respawnTimer);
            gameObject.SetActive(true);
        }
    }
}