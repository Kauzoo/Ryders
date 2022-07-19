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
        protected bool isCharging;
        
        public virtual void Setup()
        {
            this.SafeGetComponent(ref playerBehaviour);
        }

        public virtual void Master()
        {
            throw new System.NotImplementedException();
        }

        protected virtual void Jump()
        {
            if (playerBehaviour.inputPlayer.GetInputContainer().Jump && playerBehaviour.movement.Grounded)
            {
                ChargeJumpCharge();
            }
            else if(!playerBehaviour.inputPlayer.GetInputContainer().Jump && isCharging)
            {
                
            }
        }

        protected virtual void ChargeJumpCharge()
        {
            // Decelleration is handled in Accel pack
            playerBehaviour.movement.JumpCharge++;
        }

        protected virtual void ReleaseJumpCharge()
        {
            playerBehaviour.movement.JumpCharge = 0;
        }
    }
}
