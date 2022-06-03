using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ryders.Core.InputManagement
{
    public class MasterInput : MonoBehaviour
    {
        [Header("Player")]
        public bool Player1;
        public bool Player2;
        public bool Player3;
        public bool Player4;
        
        [System.NonSerialized] public Dictionary<PlayerSignifier, PlayerInputBinds> players;
        public PlayerInputBinds DefaultPlayerInputBindsKeyboard;
        public PlayerInputBinds DefaultPlayerInputBindsGamepad;
        

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void FixedUpdate()
        {
            throw new NotImplementedException();
        }
        
        
    }
    
    public class InputContainer : MonoBehaviour
    {
        public PlayerSignifier player;
            
        [Header("Axis")]
        public float VerticalAxis;
        public float HorizontalAxis;

        [Header("Directions")]
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;

        [Header("Functions")]
        public bool Jump;
        public bool Boost;
        public bool Drift;     
    }

    [CreateAssetMenu(fileName = "PlayerInputData", menuName = "ScriptableObjects/PlayerInputBinds", order = 1)]
    public class PlayerInputBinds : ScriptableObject
    {
        public InputDeviceType InputDeviceType;

        [Header("Axis")]
        public string VerticalAxis;
        public string HorizontalAxis;

        [Header("Directions")]
        public KeyCode Up;
        public KeyCode UpAlt;
        public KeyCode Down;
        public KeyCode DownAlt;
        public KeyCode Left;
        public KeyCode LeftAlt;
        public KeyCode Right;
        public KeyCode RightAlt;

        [Header("Functions")]
        public KeyCode Jump;
        public KeyCode JumpAlt;
        public KeyCode Boost;
        public KeyCode BoostAlt;
        public KeyCode Drift;
        public KeyCode DriftAlt;
    }
    
    public enum PlayerSignifier
    {
        Player1, Player2
    }

    public enum InputDeviceType
    {
        Keyboard, Gamepad, Other
    }
}
