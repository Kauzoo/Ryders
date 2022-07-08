using System;
using UnityEditor.Rendering;
using UnityEngine;

namespace Ryders.Core.Player
{
    public abstract class RydersPlayerComponent : MonoBehaviour, IRydersPlayerComponent
    {
        public abstract void Setup();
        public abstract void Master();
        
        public event EventHandler SpeedBoostEvent;

        public virtual void RaiseSpeedBoostEvent(EventArgs e) => SpeedBoostEvent?.Invoke(this, e);
    }
}