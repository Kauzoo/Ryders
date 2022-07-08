using System;

namespace Ryders.Core.Player
{
    public interface IRydersPlayerEvents
    {
        public void Subscribe(IRydersPlayerComponent publisher) => publisher.SpeedBoostEvent += OnSpeedBoost;

        public static void Subscribe(IRydersPlayerComponent publisher, IRydersPlayerEvents subscriber) =>
            publisher.SpeedBoostEvent += subscriber.OnSpeedBoost;

        protected virtual void OnSpeedBoost(object sender, EventArgs e) { }
    }
}