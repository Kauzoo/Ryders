using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nyr.UnityDev.Component;
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
    [RequireComponent(typeof(PlayerBehaviour))]
    public abstract class GravityPack : MonoBehaviour, IRydersPlayerComponent
    {
        protected PlayerBehaviour playerBehaviour;
        // ReSharper disable once MemberCanBePrivate.Global
        protected Transform playerTransform;
        // ReSharper disable once MemberCanBePrivate.Global
        protected Transform rotationAnchor;

        private Vector3 _normalSum = Vector3.up;

        [System.Serializable]
        public class RayCastSettings
        {
            [Header("GroundSettings")] public LayerMask Mask;
            [Min(0f)] public float DistanceGround;
            public Transform GroundedOrigin;
            [Header("GroundAlignment"), Min(0f)] public float DistanceAlignment;
            public Transform RaycastOriginLeftFront;
            public Transform RaycastOriginRightFront;
            public Transform RaycastOriginLeftBack;
            public Transform RaycastOriginRightBack;
        }
        public RayCastSettings rayCastSettings = new();

        private void Start()
        {
            Setup();
        }
        
        public virtual void Setup()
        {
            this.SafeGetComponent(ref playerBehaviour);
            playerTransform = playerBehaviour.playerTransform;
            rotationAnchor = playerBehaviour.rotationAnchor;
        }

        public virtual void Master()
        {
            GroundAlignment();
            Grounded();
        }
        
        protected virtual void Grounded()
        {
            if (Physics.Raycast(rayCastSettings.GroundedOrigin.position, -playerBehaviour.playerTransform.up,
                    out var hit, rayCastSettings.DistanceGround, rayCastSettings.Mask))
            {
                NormalForce(hit);
                Gravity(true);
                playerBehaviour.movement.Grounded = true;
                return;
            }

            Gravity(false);
            playerBehaviour.movement.Grounded = false;
        }

        protected virtual void Gravity(bool grounded) => playerBehaviour.movement.Gravity = grounded ? 0 : 1;
        
        protected virtual void NormalForce(RaycastHit hit)
        {
            if (hit.distance < (rayCastSettings.DistanceGround - 0.5f))
            {
                playerTransform.position += (rayCastSettings.DistanceGround - (hit.distance - 0.1f)) * Vector3.up;
            }
        }
        
        protected virtual void GroundAlignment()
        {
            var hits = new List<RaycastHit>();

            if (Physics.Raycast(rayCastSettings.RaycastOriginLeftFront.position, -playerBehaviour.playerTransform.up,
                    out var hitLeftFront, rayCastSettings.DistanceAlignment, rayCastSettings.Mask))
            {
                hits.Add(hitLeftFront);
            }

            if (Physics.Raycast(rayCastSettings.RaycastOriginRightFront.position, -playerBehaviour.playerTransform.up,
                    out var hitRightFront, rayCastSettings.DistanceAlignment, rayCastSettings.Mask))
            {
                hits.Add(hitRightFront);
            }

            if (Physics.Raycast(rayCastSettings.RaycastOriginLeftBack.position, -playerBehaviour.playerTransform.up,
                    out var hitLeftBack, rayCastSettings.DistanceAlignment, rayCastSettings.Mask))
            {
                hits.Add(hitLeftBack);
            }

            if (Physics.Raycast(rayCastSettings.RaycastOriginRightBack.position,
                    -playerBehaviour.playerTransform.up,
                    out var hitRightBack, rayCastSettings.DistanceAlignment, rayCastSettings.Mask))
            {
                hits.Add(hitRightBack);
            }

            if (Physics.Raycast(rayCastSettings.RaycastOriginLeftFront.position, Vector3.down,
                    out var hit0, rayCastSettings.DistanceAlignment, rayCastSettings.Mask))
            {
                hits.Add(hit0);
            }

            if (Physics.Raycast(rayCastSettings.RaycastOriginRightFront.position, Vector3.down,
                    out var hit1, rayCastSettings.DistanceAlignment, rayCastSettings.Mask))
            {
                hits.Add(hit1);
            }

            if (Physics.Raycast(rayCastSettings.RaycastOriginLeftBack.position, Vector3.down,
                    out var hit2, rayCastSettings.DistanceAlignment, rayCastSettings.Mask))
            {
                hits.Add(hit2);
            }

            if (Physics.Raycast(rayCastSettings.RaycastOriginRightBack.position,
                    Vector3.down,
                    out var hit3, rayCastSettings.DistanceAlignment, rayCastSettings.Mask))
            {
                hits.Add(hit3);
            }

            if (Physics.Raycast(rayCastSettings.GroundedOrigin.position,
                    Vector3.down,
                    out var hit4, rayCastSettings.DistanceAlignment, rayCastSettings.Mask))
            {
                hits.Add(hit4);
            }

            if (hits.Count != 0)
            {
                var newUp = new Vector3(0f, 0f, 0f);
                hits.ForEach((hit) => newUp += hit.normal);
                newUp.Normalize();
                _normalSum = newUp;
                playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation,
                    Quaternion.FromToRotation(Vector3.up, newUp), 1f);
                playerTransform.Rotate(0f, rotationAnchor.rotation.eulerAngles.y, 0f, Space.Self);
            }
        }

        private void OnDrawGizmos()
        {
            /*Gizmos.DrawRay(rayCastSettings.RaycastOriginLeftFront.position,
                Vector3.down * rayCastSettings.DistanceAlignment);
            Gizmos.DrawRay(rayCastSettings.RaycastOriginRightFront.position,
                Vector3.down * rayCastSettings.DistanceAlignment);
            Gizmos.DrawRay(rayCastSettings.RaycastOriginRightBack.position,
                Vector3.down * rayCastSettings.DistanceAlignment);
            Gizmos.DrawRay(rayCastSettings.RaycastOriginLeftBack.position,
                Vector3.down * rayCastSettings.DistanceAlignment);
            Gizmos.DrawRay(rayCastSettings.RaycastOriginLeftFront.position,
                -playerTransform.up * rayCastSettings.DistanceAlignment);
            Gizmos.DrawRay(rayCastSettings.RaycastOriginRightFront.position,
                -playerTransform.up * rayCastSettings.DistanceAlignment);
            Gizmos.DrawRay(rayCastSettings.RaycastOriginRightBack.position,
                -playerTransform.up * rayCastSettings.DistanceAlignment);
            Gizmos.DrawRay(rayCastSettings.RaycastOriginLeftBack.position,
                -playerTransform.up * rayCastSettings.DistanceAlignment);
            Gizmos.DrawRay(playerTransform.position, _normalSum * 2);*/
        }
        
        /// <summary>
        /// Allignment Magic is done here
        /// </summary>
        /// <param name="newGround"></param>
        /// <param name="previousGround"></param>
        /// <param name="currentGround"></param>
        /// <param name="playerTransform"></param>
        [Obsolete]
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
    }
}