using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace Ryders.Core.InputManagement
{
    public class MasterInput : MonoBehaviour
    {
        // TODO Rework the InputSysten to use the new InputSystem
        [Header("Player")] 
        [SerializeField] private bool[] initialPlayers = new []{ true, false, false, false, false, false, false, false };

        public Dictionary<PlayerSignifier, InputPlayer> players;
        
        public PlayerInputBinds DefaultPlayerInputBindsKeyboard;
        public PlayerInputBinds DefaultPlayerInputBindsGamepad;
        

        // Start is called before the first frame update
        void Start()
        {
            Setup();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        private void Setup()
        {
            players = new Dictionary<PlayerSignifier, InputPlayer>();
            for (var i = 0; i < initialPlayers.Length; i++)
            {
                if (initialPlayers[i])
                {
                    players.Add((PlayerSignifier)i,
                        new InputPlayer((PlayerSignifier)i,
                            new List<PlayerInputBinds>()
                                { DefaultPlayerInputBindsGamepad, DefaultPlayerInputBindsKeyboard }));
                }
            }
        }

        public void AddPlayer(PlayerSignifier playerSignifier)
        {
            throw new NotImplementedException();
        }

        public void RemovePlayer(PlayerSignifier playerSignifier)
        {
            throw new NotImplementedException();
        }
    }
    
    public class InputPlayer : UnityEngine.Object
    {
        // TODO Add MenuBind Support
        // TODO Make stuff more private
        [SerializeField] private readonly PlayerSignifier playerSignifier;
        [SerializeReference] private readonly InputContainer inputContainer;
        [SerializeField] private readonly List<PlayerInputBinds> playerInputBindsList;

        public InputPlayer(PlayerSignifier playerSignifier, List<PlayerInputBinds> playerInputBindsList)
        {
            this.playerSignifier = playerSignifier;
            this.inputContainer = new InputContainer();
            this.playerInputBindsList = playerInputBindsList;
        }

        public PlayerSignifier GetPlayerSignifier()
        {
            return playerSignifier;
        }

        public InputContainer GetInputContainer()
        {
            return inputContainer;
        }
        public void GetInput()
        {
            // TODO Rework this bullshit
            inputContainer.VerticalAxis = Input.GetAxis(playerInputBindsList.First().VerticalAxis);
            inputContainer.HorizontalAxis = Input.GetAxis(playerInputBindsList.First().HorizontalAxis);
            inputContainer.Up = Input.GetKey(playerInputBindsList.First().Up) ||
                                Input.GetKey(playerInputBindsList.First().UpAlt);
            inputContainer.Down = Input.GetKey(playerInputBindsList.First().Down) ||
                                  Input.GetKey(playerInputBindsList.First().DownAlt);
            inputContainer.Left = Input.GetKey(playerInputBindsList.First().Left) ||
                                  Input.GetKey(playerInputBindsList.First().LeftAlt);
            inputContainer.Right = Input.GetKey(playerInputBindsList.First().Right) ||
                                   Input.GetKey(playerInputBindsList.First().RightAlt);
            inputContainer.Jump = Input.GetKey(playerInputBindsList.First().Jump) ||
                                  Input.GetKey(playerInputBindsList.First().JumpAlt);
            inputContainer.Boost = Input.GetKey(playerInputBindsList.First().Boost) ||
                                   Input.GetKey(playerInputBindsList.First().BoostAlt);
            inputContainer.Drift = Input.GetKey(playerInputBindsList.First().Drift) ||
                                   Input.GetKey(playerInputBindsList.First().DriftAlt);
        }
        
        public class InputContainer
        {
            // AXIS
            public float VerticalAxis;
            public float HorizontalAxis;

            // DIRECTIONS
            public bool Up;
            public bool Down;
            public bool Left;
            public bool Right;

            // FUNCTIONS
            public bool Jump;
            public bool Boost;
            public bool Drift;
        }
    }
    
    public enum PlayerSignifier
    {
        Player1, Player2, Player3, Player4, Player5, Player6, Player7, Player8
    }

    public enum InputDeviceType
    {
        Keyboard, Gamepad, Other
    }

    public class IniParser
    {
        // TODO Write IniParser
    }
}
