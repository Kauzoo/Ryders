using System;
using UnityEngine;

namespace Ryders.Core.Player
{
    public interface IRydersPlayerEventPublisher
    {
        public event EventHandler SpeedBoostEvent;
        public event EventHandler LevelUpEvent;
        public event EventHandler LevelDownEvent;
        public event EventHandler LevelChangeEvent;

        public void RaiseSpeedBoostEvent(EventArgs e) => Debug.LogWarning("RaiseSpeedBoostEvent was not overwritten");
        public void RaiseLevelUpEvent(EventArgs e) => Debug.LogWarning("RaiseLevelUpEvent was not overwritten");
        public void RaiseLevelDownEvent(EventArgs e) => Debug.LogWarning("RaiseLevelDownEvent was not overwritten");
        public void RaiseLevelChangeEvent(EventArgs e) => Debug.LogWarning("RaiseLevelChangeEvent was not overwritten");
    }
}