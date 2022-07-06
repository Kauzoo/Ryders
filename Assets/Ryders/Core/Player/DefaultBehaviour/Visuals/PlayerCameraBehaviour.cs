using System;
using UnityEngine;

namespace Ryders.Core.Player.DefaultBehaviour.Visuals
{
    public class PlayerCameraBehaviour : MonoBehaviour
    {
        public Transform playerTransform;

        private Vector3 offset;
        
        private void Start()
        {
            offset = transform.localPosition;
            transform.SetParent(null);
        }

        private void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, playerTransform.TransformPoint(offset), 0.9f);
            var oldRot = transform.rotation;
            transform.LookAt(playerTransform);
            var newRot = transform.rotation;
            transform.rotation = oldRot;
            transform.rotation = Quaternion.Lerp(oldRot, newRot, 0.4f);
        }
    }
}