using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryders.Core.InputManagement
{
    [CreateAssetMenu(fileName = "PlayerInputData", menuName = "ScriptableObjects/PlayerInputBinds", order = 1)]
    public class PlayerInputBinds : ScriptableObject
    {
        public InputDeviceType InputDeviceType;

        [Header("Axis")] public string VerticalAxis;
        public string HorizontalAxis;

        [Header("Directions")] public string Up;
        public string UpAlt;
        public string Down;
        public string DownAlt;
        public string Left;
        public string LeftAlt;
        public string Right;
        public string RightAlt;

        [Header("Functions")] public string Jump;
        public string JumpAlt;
        public string Boost;
        public string BoostAlt;
        public string Drift;
        public string DriftAlt;
    }
}
