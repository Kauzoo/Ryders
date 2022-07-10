using System;

namespace Ryders.Core.Player
{
    public interface IRydersPlayerEventPublisher
    {
        public event EventHandler SpeedBoostEvent;
        public event EventHandler LevelUpEvent;
        public event EventHandler LevelDownEvent;

        public void RaiseSpeedBoostEvent(EventArgs e);
    }
}