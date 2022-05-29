using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ryders.Core.Player.ExtremeGear;

namespace Ryders.Core.Player
{
    public class BlueStarBehaviour : PlayerBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update() 
        {
            base.UpdateBase();
            
        }

        private void FixedUpdate()
        {
            base.FixedUpdateBase();
        }
    }
}
