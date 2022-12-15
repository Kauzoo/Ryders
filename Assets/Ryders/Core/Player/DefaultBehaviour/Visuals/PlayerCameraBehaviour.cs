using System;
using Ryders.Core.Player.DefaultBehaviour.Components;
using UnityEngine;
using Nyr.UnityDev.Util;

namespace Ryders.Core.Player.DefaultBehaviour.Visuals
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(Transform))]
    public class PlayerCameraBehaviour : MonoBehaviour, IRydersPlayerComponent, IRydersPlayerEvents
    {
        // ReSharper disable once MemberCanBePrivate.Global
        protected Transform cameraTransform;
        protected Camera cam;
        // EXTERNAL
        // ReSharper disable once MemberCanBePrivate.Global
        protected Transform playerTransform;
        // ReSharper disable once MemberCanBePrivate.Global
        protected PlayerBehaviour playerBehaviour;
        // ReSharper disable once MemberCanBePrivate.Global
        protected BoostPack boostPack;
        // ReSharper disable once MemberCanBePrivate.Global
        protected DriftPack driftPack;

        private Vector3 _offset;

        private void Awake()
        {
            Setup();
        }
        
        private void OnDisable()
        {
            (this as IRydersPlayerEvents).Unsubscribe(boostPack);
            (this as IRydersPlayerEvents).Unsubscribe(driftPack);
        }

        private void FixedUpdate()
        {
            cameraTransform.position = Vector3.Lerp(transform.position, playerTransform.TransformPoint(_offset), 0.9f);
            var oldRot = transform.rotation;
            cameraTransform.LookAt(playerTransform);
            var rotation = cameraTransform.rotation;
            var newRot = rotation;
            rotation = oldRot;
            rotation = Quaternion.Lerp(oldRot, newRot, 0.4f);
            cameraTransform.rotation = rotation;
        }

        public virtual void Setup()
        {
            cameraTransform = GetComponent<Transform>();
            cam = GetComponent<Camera>();
            this.SafeGetComponentInParent(ref playerBehaviour);
            playerTransform = playerBehaviour.transform;
            playerBehaviour.SafeGetComponent(ref boostPack);
            playerBehaviour.SafeGetComponent(ref driftPack);
            (this as IRydersPlayerEvents).Subscribe(boostPack);
            (this as IRydersPlayerEvents).Subscribe(driftPack);
            _offset = transform.localPosition;
            transform.SetParent(null);
        }

        public virtual void Master()
        {
            throw new NotImplementedException();
        }

        public virtual void OnSpeedBoost(object sender, EventArgs e)
        {
            Debug.LogWarning("Event received");
        }
    }
}