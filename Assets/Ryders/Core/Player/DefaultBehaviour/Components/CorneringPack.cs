using UnityEngine;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
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