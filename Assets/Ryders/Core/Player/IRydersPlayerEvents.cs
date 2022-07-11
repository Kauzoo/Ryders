using System;
using UnityEngine;

namespace Ryders.Core.Player
{
    public interface IRydersPlayerEvents
    {
        void Subscribe(IRydersPlayerEventPublisher publisher)
        {
            publisher.SpeedBoostEvent += OnSpeedBoost;
            publisher.LevelUpEvent += OnLevelUp;
            publisher.LevelDownEvent += OnLevelDown;
            publisher.LevelChangeEvent += OnLevelChange;
        }

        void Unsubscribe(IRydersPlayerEventPublisher publisher)
        {
            publisher.SpeedBoostEvent -= OnSpeedBoost;
            publisher.LevelUpEvent -= OnLevelUp;
            publisher.LevelDownEvent -= OnLevelDown;
            publisher.LevelChangeEvent -= OnLevelChange;
        }

        public static void Subscribe(IRydersPlayerEventPublisher publisher, IRydersPlayerEvents subscriber)
        {
            publisher.SpeedBoostEvent += subscriber.OnSpeedBoost;
            publisher.LevelUpEvent += subscriber.OnLevelUp;
            publisher.LevelDownEvent += subscriber.OnLevelDown;
            publisher.LevelChangeEvent += subscriber.OnLevelChange;
        }

        public static void Unsubscribe(IRydersPlayerEventPublisher publisher, IRydersPlayerEvents subscriber)
        {
            publisher.SpeedBoostEvent -= subscriber.OnSpeedBoost;
            publisher.LevelUpEvent -= subscriber.OnLevelUp;
            publisher.LevelDownEvent -= subscriber.OnLevelDown;
            publisher.LevelChangeEvent -= subscriber.OnLevelChange;
        }

        void OnSpeedBoost(object sender, EventArgs e) =>
            Debug.LogWarning("OnSpeedBoost was not overwritten", this as UnityEngine.Object);

        void OnLevelUp(object sender, EventArgs e) =>
            Debug.LogWarning("OnLevelUp was not overwritten", this as UnityEngine.Object);

        void OnLevelDown(object sender, EventArgs e) =>
            Debug.LogWarning("OnLevelDown was not overwritten", this as UnityEngine.Object);

        void OnLevelChange(object sender, EventArgs e) =>
            Debug.LogWarning("OnLevelChange was not overwritten", this as UnityEngine.Object);

        void OnDrift(object sender, EventArgs e) =>
            Debug.LogWarning("OnDrift was not overwritten", this as UnityEngine.Object);
    }
}