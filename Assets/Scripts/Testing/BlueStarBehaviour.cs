using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ryders.Core.Player.ExtremeGear;
using Ryders.Core.Player.ExtremeGear.Movement;

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
            //accelPack.StandardAcceleration();
        }

        private void FixedUpdate()
        {
            base.FixedUpdateBase();
        }
    }
}
