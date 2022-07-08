using System;
using Ryders.Core.Player.DefaultBehaviour.Components;
using UnityEngine;

namespace Ryders.Core.Player.DefaultBehaviour.Visuals
{
    public class PlayerCameraBehaviour : MonoBehaviour, IRydersPlayerComponent, IRydersPlayerEvents
    {
        public Transform playerTransform;
        [SerializeReference] protected PlayerBehaviour _playerBehaviour;
        [SerializeReference] protected BoostPack _boostPack;
        [SerializeReference] protected DriftPack _driftPack;

        private Vector3 offset;
        private IRydersPlayerComponent _rydersPlayerComponentImplementation;

        private void Awake()
        {
            Setup();
        }

        private void Start()
        {
            offset = transform.localPosition;
            transform.SetParent(null);
        }

        private void OnDisable()
        {
            (this as IRydersPlayerEvents).Unsubscribe(_boostPack);
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

        public virtual void Setup()
        {
            if(_playerBehaviour == null)
                _playerBehaviour = GetComponentInParent<PlayerBehaviour>() != null
                    ? GetComponentInParent<PlayerBehaviour>()
                    : throw new MissingReferenceException();
            if(_boostPack == null)
                _boostPack = (_playerBehaviour.boostPack != null)
                    ? _playerBehaviour.boostPack
                    : throw new MissingComponentException();
            if (_driftPack == null)
                _driftPack = (_playerBehaviour.boostPack != null)
                    ? _playerBehaviour.driftPack
                    : throw new MissingReferenceException();
            (this as IRydersPlayerEvents).Subscribe(_boostPack);
            (this as IRydersPlayerEvents).Subscribe(_driftPack);
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