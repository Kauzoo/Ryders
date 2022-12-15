using System;
using UnityEngine;
using static Nyr.UnityDev.Util.GetComponentSafe;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    public abstract class EventPublisherPack : MonoBehaviour, IRydersPlayerComponent, IRydersPlayerEventPublisher
    {
        protected PlayerBehaviour playerBehaviour;
        
        public event EventHandler SpeedBoostEvent;
        public event EventHandler LevelUpEvent;
        public event EventHandler LevelDownEvent;
        public event EventHandler LevelChangeEvent;
        
        public virtual void Setup()
        {
            this.SafeGetComponent(ref playerBehaviour);
        }

        public virtual void Master()
        {
        }

        public virtual void RaiseSpeedBoostEvent(EventArgs e)
        {
            var raiseEvent = SpeedBoostEvent;
            raiseEvent?.Invoke(this, e);
        }

        public virtual void RaiseLevelUpEvent(EventArgs e)
        {
            var raiseEvent = SpeedBoostEvent;
            raiseEvent?.Invoke(this, e);
        }

        public virtual void RaiseLevelDownEvent(EventArgs e)
        {
            var raiseEvent = SpeedBoostEvent;
            raiseEvent?.Invoke(this, e);
        }

        public virtual void RaiseLevelChangeEvent(EventArgs e)
        {
            var raiseEvent = SpeedBoostEvent;
            raiseEvent?.Invoke(this, e);
        }
    }
}