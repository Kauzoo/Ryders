using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryders.Core.Player.DefaultBehaviour.Components
{
    public abstract class StateDeterminationPack : MonoBehaviour
    {
        // TODO Implement StateDeterminationPack
        public PlayerBehaviour playerBehaviour;

        public void Start()
        {
            playerBehaviour = GetComponent<PlayerBehaviour>();
        }

        public virtual void DetermineMaxSpeed()
        {
        }
    }
}
