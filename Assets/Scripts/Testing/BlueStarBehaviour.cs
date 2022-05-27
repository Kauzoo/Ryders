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

        public override void MasterMove()
        {
            base.MasterMove();
            playerTransform.Translate(new Vector3(0, 1, 0), Space.World);
        }
    }
}
