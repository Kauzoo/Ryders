using System;

namespace Ryders.Core.Player
{
    public interface IRydersPlayerEventPublisher
    {
        public event EventHandler SpeedBoostEvent;

        public void RaiseSpeedBoostEvent(EventArgs e);
    }
}