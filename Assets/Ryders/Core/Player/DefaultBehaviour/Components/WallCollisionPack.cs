using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    public abstract class WallCollisionPack : MonoBehaviour
    {
        // TODO Implement WallCollisionPack
        private PlayerBehaviour _playerBehaviour;
        
        private void Start()
        {
            _playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        public virtual void OnCollision(Collision collision)
        {
            
        }
    }
}
