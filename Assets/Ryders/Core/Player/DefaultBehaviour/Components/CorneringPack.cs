using UnityEngine;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    /**
     *  Notes for Cornering:
     *  -How do I determine if I am Cornering?
     */
    public abstract class CorneringPack : MonoBehaviour
    {
        // TODO Implement CorneringPack
        private PlayerBehaviour _playerBehaviour;

        private void Start()
        {
            _playerBehaviour = GetComponent<PlayerBehaviour>();
        }
        
        
    }
}