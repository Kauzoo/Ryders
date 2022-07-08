using System;
using Ryders.Core.Player.DefaultBehaviour.Components;
using UnityEngine;

namespace Ryders.Core.Player.DefaultBehaviour.Visuals
{
    public class PlayerCameraBehaviour : MonoBehaviour, IRydersPlayerEvents
    {
        public Transform playerTransform;
        [SerializeReference] protected PlayerBehaviour _playerBehaviour;
        [SerializeReference] protected BoostPack _boostPack;

        private Vector3 offset;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }
        

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

        protected virtual void Setup()
        {
            _playerBehaviour ??= (GetComponentInParent<PlayerBehaviour>() ?? throw new MissingReferenceException());
            _boostPack ??= (_playerBehaviour.boostPack ?? throw new MissingComponentException());
            IRydersPlayerEvents.Subscribe(_boostPack, this);
        }

        void IRydersPlayerEvents.OnSpeedBoost(object sender, EventArgs e)
        {
            Debug.Log("it works");
        }
    }
}