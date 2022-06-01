using System.Collections;
using System.Collections.Generic;
using Ryders.Core.Player.DefaultBehaviour;
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
            base.Setup();
            base.statLoaderPack.Setup();
        }
        
        private void FixedUpdate()
        {
            
            base.FixedUpdateTest();   
        }
    }
}
