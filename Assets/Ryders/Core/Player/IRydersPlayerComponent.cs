using System;
using UnityEditor.Rendering;

namespace Ryders.Core.Player
{
    public interface IRydersPlayerComponent
    {
        public void Setup();
        public void Master();

        void OnEnter()
        {
        }

        void OnExit()
        {
        }
    }
}