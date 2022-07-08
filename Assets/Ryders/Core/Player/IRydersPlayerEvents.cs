using System;
using UnityEngine;

namespace Ryders.Core.Player
{
    public interface IRydersPlayerEvents
    {
        void Subscribe(IRydersPlayerEventPublisher publisher) => publisher.SpeedBoostEvent += OnSpeedBoost;
        void Unsubscribe(IRydersPlayerEventPublisher publisher) => publisher.SpeedBoostEvent -= OnSpeedBoost;

        public static void Subscribe(IRydersPlayerEventPublisher publisher, IRydersPlayerEvents subscriber) =>
            publisher.SpeedBoostEvent += subscriber.OnSpeedBoost;
        
        public static void Unsubscribe(IRydersPlayerEventPublisher publisher, IRydersPlayerEvents subscriber) =>
            publisher.SpeedBoostEvent -= subscriber.OnSpeedBoost;

        void OnSpeedBoost(object sender, EventArgs e) => Debug.LogWarning("OnSpeedBoost was not overwritten", this as UnityEngine.Object);

        void OnDrift(object sender, EventArgs e) => Debug.LogWarning("OnDrift was not overwritten", this as UnityEngine.Object);
    }
}