using UnityEngine;
using Nyr.UnityDev.Component;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    [RequireComponent(typeof(PlayerBehaviour))]
    public abstract class JumpPack : MonoBehaviour, IRydersPlayerComponent
    {
        // TODO: Implement JumpPack
        // ReSharper disable once MemberCanBePrivate.Global
        protected PlayerBehaviour playerBehaviour;
        
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
