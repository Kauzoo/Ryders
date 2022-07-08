using System;
using UnityEngine;

namespace Ryders.Core.Player
{
    public abstract class RydersPlayerEventPublisher : MonoBehaviour, IRydersPlayerEventPublisher
    {
        public event EventHandler SpeedBoostEvent;

        public virtual void RaiseSpeedBoostEvent(EventArgs e)
        {
            var raiseEvent = SpeedBoostEvent;
            raiseEvent?.Invoke(this, e);
        }
    }
}