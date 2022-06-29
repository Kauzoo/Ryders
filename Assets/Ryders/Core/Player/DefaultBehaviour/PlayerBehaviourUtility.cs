#nullable enable
using System;
using JetBrains.Annotations;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Ryders.Core.Player.DefaultBehaviour
{
    public abstract partial class PlayerBehaviour
    {
        public static bool IsPlayerBehaviour(dynamic? input) => (input ?? new Object()) is PlayerBehaviour;
        
        public static PlayerBehaviour DynamicToPlayerBehaviour(dynamic input) =>
            IsPlayerBehaviour(input) ? (PlayerBehaviour) input : throw new InvalidCastException();

        public static bool TryToPlayerBehaviour(dynamic input, out PlayerBehaviour? playerBehaviourOut)
        {
            if (IsPlayerBehaviour(input))
            {
                playerBehaviourOut = (PlayerBehaviour) input;
                return true;
            }
            playerBehaviourOut = null;
            return false;
        }

        public static (bool HasValue, PlayerBehaviour? playerBehaviour) TryToPlayerBehaviour(dynamic input) =>
            IsPlayerBehaviour(input) ? (true, (PlayerBehaviour)input) : (false, null);
    }
}