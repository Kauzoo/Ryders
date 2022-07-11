using System;
using Ryders.Core.Player.DefaultBehaviour;
using UnityEditor.Rendering;
using UnityEngine;

namespace Ryders.Core.Player
{
    public interface IRydersPlayerComponent
    {

        public void Setup();
        public void Master();

        void OnEnter() => Debug.LogWarning($"@{this}: OnEnter is used but not overwritten");

        void OnExit() =>  Debug.LogWarning($"@{this}: OnExit is used but not overwritten");
    }
}