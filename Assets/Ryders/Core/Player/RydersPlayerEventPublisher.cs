using System;
using UnityEngine;

namespace Ryders.Core.Player
{
    public abstract class RydersPlayerEventPublisher : MonoBehaviour, IRydersPlayerEventPublisher
    {
        public event EventHandler SpeedBoostEvent;
        public event EventHandler LevelUpEvent;
        public event EventHandler LevelDownEvent;
        public event EventHandler LevelChangeEvent;

        public virtual void RaiseSpeedBoostEvent(EventArgs e)
        {
            var raiseEvent = SpeedBoostEvent;
            raiseEvent?.Invoke(this, e);
        }
        
        public virtual void RaiseLevelUpEvent(EventArgs e)
        {
            var raiseEvent = LevelUpEvent;
            raiseEvent?.Invoke(this, e);
        }
        
        public virtual void RaiseLevelDownEvent(EventArgs e)
        {
            var raiseEvent = LevelDownEvent;
            raiseEvent?.Invoke(this, e);
        }
        
        public virtual void RaiseLevelChangeEvent(EventArgs e)
        {
            var raiseEvent = LevelChangeEvent;
            raiseEvent?.Invoke(this, e);
        }
    }
}