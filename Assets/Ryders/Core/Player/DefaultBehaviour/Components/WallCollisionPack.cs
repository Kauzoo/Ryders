using UnityEngine;
using Nyr.UnityDev.Util;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    public abstract class WallCollisionPack : MonoBehaviour, IRydersPlayerComponent
    {
        // TODO Implement WallCollisionPack
        // ReSharper disable once MemberCanBePrivate.Global
        protected PlayerBehaviour playerBehaviour;
        
        private void Start()
        {
            Setup();
        }
        
        public virtual void Setup()
        {
            this.SafeGetComponent(ref playerBehaviour);
        }

        public virtual void Master()
        {
            throw new System.NotImplementedException();
        }
    }
}
