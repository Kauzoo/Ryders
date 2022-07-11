using System;
using UnityEngine;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    public abstract class EventPublisherPack : MonoBehaviour, IRydersPlayerComponent, IRydersPlayerEventPublisher
    {
        public event EventHandler SpeedBoostEvent;
        public event EventHandler LevelUpEvent;
        public event EventHandler LevelDownEvent;
        public event EventHandler LevelChangeEvent;
        
        public void Setup()
        {
            throw new System.NotImplementedException();
        }

        public void Master()
        {
            throw new System.NotImplementedException();
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