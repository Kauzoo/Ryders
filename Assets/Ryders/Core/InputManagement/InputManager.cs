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
        
        
    }

    [CreateAssetMenu(fileName = "PlayerInputData", menuName = "ScriptableObjects/PlayerInputBinds", order = 1)]
    public class PlayerInputBinds : ScriptableObject
    {
        [Header("Axis")]
        public string VerticalAxis;
        public string HorizontalAxis;

        public KeyCode Up;
        public KeyCode Down;
        public KeyCode Left;
        public KeyCode Right;

        public bool Jump;
        public bool 
    }
}
