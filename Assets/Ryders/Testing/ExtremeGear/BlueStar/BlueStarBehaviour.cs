using Ryders.Core.Player.DefaultBehaviour;
using UnityEngine;

namespace Ryders.Testing.ExtremeGear.BlueStar
{
    public class BlueStarBehaviour : PlayerBehaviour
    {
        // Start is called before the first frame update
        private void Awake()
        {
            Application.targetFrameRate = 120;
            base.Setup();
            base.statLoaderPack.Setup();
        }
        
        private void FixedUpdate()
        {
            base.FixedUpdateTest();   
        }

        private void Update()
        {
            base.UpdateTest();
        }
    }
}
