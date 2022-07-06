using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ryders.Core.Player.DefaultBehaviour;
using UnityEngine;
using UnityEngine.UI;

namespace Ryders.Core.Player.ExtremeGear.Movement
{
    /// <summary>
    /// Contains Methods for both Gravity and Grounded
    /// If you are implementing ExtremeGear that uses different Ground/Gravity Behaviour,
    /// derive from this class
    /// </summary>
    public abstract class GravityPack : MonoBehaviour
    {
        // TODO: Implement Gravity Pack
        protected PlayerBehaviour _playerBehaviour;
        public Transform _playerTransform;
        public Transform _rotationAnchor;

        private Vector3 normalSum = Vector3.up;

        [System.Serializable]
        public class RayCastSettings
        {
            [Header("GroundSettings")] public LayerMask Mask;
            public float DistanceGround;
            public Transform GroundedOrigin;
            [Header("GroundAlignment")] public float DistanceAlignment;
            public Transform RaycastOriginLeftFront;
            public Transform RaycastOriginRightFront;
            public Transform RaycastOriginLeftBack;
            public Transform RaycastOriginRightBack;
        }

        public RayCastSettings _rayCastSettings = new();

        private void Awake()
        {
            _playerBehaviour = GetComponent<PlayerBehaviour>();
            _playerTransform = _playerBehaviour.transform;
        }

        private void Start()
        {
            _playerBehaviour = GetComponent<PlayerBehaviour>();
            _playerTransform = _playerBehaviour.transform;
            _rotationAnchor = _playerBehaviour.rotationAnchor;
        }

        public virtual void GravityPackMaster()
        {
            GroundAlignment();
            Grounded();
        }

        protected virtual void Grounded()
        {
            if (Physics.Raycast(_rayCastSettings.GroundedOrigin.position, -_playerBehaviour.playerTransform.up,
                    out var hit, _rayCastSettings.DistanceGround, _rayCastSettings.Mask))
            {
                Debug.Log("Grounded");
                //GroundAlignment();
                AntiGravity(hit);
                Gravity(true);
                _playerBehaviour.movement.Grounded = true;
                return;
            }

            Gravity(false);
            _playerBehaviour.movement.Grounded = false;
        }

        protected virtual void Gravity(bool grounded) => _playerBehaviour.movement.Gravity = grounded ? 0 : 1;

        /// <summary>
        /// Allignment Magic is done here
        /// </summary>
        /// <param name="newGround"></param>
        /// <param name="previousGround"></param>
        /// <param name="currentGround"></param>
        /// <param name="playerTransform"></param>
        private void NewGroundChanged(GameObject newGround, GameObject previousGround, GameObject currentGround,
            Transform playerTransform)
        {
            previousGround = currentGround;
            currentGround = newGround;

            var localSpacePlayerForward = previousGround.transform.InverseTransformDirection(playerTransform.forward);

            //Quaternion targetRotation = Quaternion.LookRotation(groundInfo.currentGround.transform.forward, groundInfo.currentGround.transform.up);
            //playerTransform.rotation = targetRotation;

            var worldSpaceForward = currentGround.transform.TransformDirection(localSpacePlayerForward);
            playerTransform.rotation = Quaternion.LookRotation(worldSpaceForward, currentGround.transform.up);
        }

        protected virtual void AntiGravity(RaycastHit hit)
        {
            if (hit.distance < (_rayCastSettings.DistanceGround - 0.5f))
            {
                _playerTransform.position += (_rayCastSettings.DistanceGround - (hit.distance - 0.1f)) * Vector3.up;
            }
        }

        protected virtual void NormalForce()
        {
        }

        protected virtual void GroundAlignment()
        {
            var hits = new List<RaycastHit>();

            if (Physics.Raycast(_rayCastSettings.RaycastOriginLeftFront.position, -_playerBehaviour.playerTransform.up,
                    out var hitLeftFront, _rayCastSettings.DistanceAlignment, _rayCastSettings.Mask))
            {
                hits.Add(hitLeftFront);
            }

            if (Physics.Raycast(_rayCastSettings.RaycastOriginRightFront.position, -_playerBehaviour.playerTransform.up,
                    out var hitRightFront, _rayCastSettings.DistanceAlignment, _rayCastSettings.Mask))
            {
                hits.Add(hitRightFront);
            }

            if (Physics.Raycast(_rayCastSettings.RaycastOriginLeftBack.position, -_playerBehaviour.playerTransform.up,
                    out var hitLeftBack, _rayCastSettings.DistanceAlignment, _rayCastSettings.Mask))
            {
                hits.Add(hitLeftBack);
            }

            if (Physics.Raycast(_rayCastSettings.RaycastOriginRightBack.position,
                    -_playerBehaviour.playerTransform.up,
                    out var hitRightBack, _rayCastSettings.DistanceAlignment, _rayCastSettings.Mask))
            {
                hits.Add(hitRightBack);
            }

            if (Physics.Raycast(_rayCastSettings.RaycastOriginLeftFront.position, Vector3.down,
                    out var hit0, _rayCastSettings.DistanceAlignment, _rayCastSettings.Mask))
            {
                hits.Add(hit0);
            }

            if (Physics.Raycast(_rayCastSettings.RaycastOriginRightFront.position, Vector3.down,
                    out var hit1, _rayCastSettings.DistanceAlignment, _rayCastSettings.Mask))
            {
                hits.Add(hit1);
            }

            if (Physics.Raycast(_rayCastSettings.RaycastOriginLeftBack.position, Vector3.down,
                    out var hit2, _rayCastSettings.DistanceAlignment, _rayCastSettings.Mask))
            {
                hits.Add(hit2);
            }

            if (Physics.Raycast(_rayCastSettings.RaycastOriginRightBack.position,
                    Vector3.down,
                    out var hit3, _rayCastSettings.DistanceAlignment, _rayCastSettings.Mask))
            {
                hits.Add(hit3);
            }

            if (Physics.Raycast(_rayCastSettings.GroundedOrigin.position,
                    Vector3.down,
                    out var hit4, _rayCastSettings.DistanceAlignment, _rayCastSettings.Mask))
            {
                hits.Add(hit4);
            }

            if (hits.Count != 0)
            {
                var newUp = new Vector3(0f, 0f, 0f);
                hits.ForEach((hit) => newUp += hit.normal);
                newUp.Normalize();
                normalSum = newUp;
                /*var newRot = Quaternion.Lerp(_playerTransform.rotation, Quaternion.FromToRotation(_playerTransform.up, newUp),
                    1f);*/
                _playerTransform.rotation = Quaternion.Lerp(_playerTransform.rotation,
                    Quaternion.FromToRotation(Vector3.up, newUp), 1f);
                _playerTransform.Rotate(0f, _rotationAnchor.rotation.eulerAngles.y, 0f, Space.Self);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(_rayCastSettings.RaycastOriginLeftFront.position,
                Vector3.down * _rayCastSettings.DistanceAlignment);
            Gizmos.DrawRay(_rayCastSettings.RaycastOriginRightFront.position,
                Vector3.down * _rayCastSettings.DistanceAlignment);
            Gizmos.DrawRay(_rayCastSettings.RaycastOriginRightBack.position,
                Vector3.down * _rayCastSettings.DistanceAlignment);
            Gizmos.DrawRay(_rayCastSettings.RaycastOriginLeftBack.position,
                Vector3.down * _rayCastSettings.DistanceAlignment);
            Gizmos.DrawRay(_rayCastSettings.RaycastOriginLeftFront.position,
                -_playerTransform.up * _rayCastSettings.DistanceAlignment);
            Gizmos.DrawRay(_rayCastSettings.RaycastOriginRightFront.position,
                -_playerTransform.up * _rayCastSettings.DistanceAlignment);
            Gizmos.DrawRay(_rayCastSettings.RaycastOriginRightBack.position,
                -_playerTransform.up * _rayCastSettings.DistanceAlignment);
            Gizmos.DrawRay(_rayCastSettings.RaycastOriginLeftBack.position,
                -_playerTransform.up * _rayCastSettings.DistanceAlignment);
            Gizmos.DrawRay(_playerTransform.position, normalSum * 2);
        }
    }
}