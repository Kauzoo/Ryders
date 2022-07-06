using System;
using UnityEngine;

namespace Ryders.Core.Player.DefaultBehaviour.Visuals
{
    public class VisualPlayerBehaviour : MonoBehaviour
    {
        public Transform playerTransform;

        private void Start()
        {
            transform.SetParent(null);
            transform.position = playerTransform.position;
            transform.rotation = playerTransform.rotation;
        }

        private void FixedUpdate()
        {
            transform.position = playerTransform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, playerTransform.rotation, 0.5f);
        }
    }
}