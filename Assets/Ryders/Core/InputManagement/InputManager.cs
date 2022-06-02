using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ryders.Core.InputManagement
{
    public class MasterInput : MonoBehaviour
    {
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
        [Header("Axis")]
        public string VerticalAxis;
        public string HorizontalAxis;

        [Header("Directions")]
        public KeyCode Up;
        public KeyCode Down;
        public KeyCode Left;
        public KeyCode Right;

        [Header("Functions")]
        public KeyCode Jump;
        public KeyCode Boost;
        public KeyCode Drift;
    }
}
