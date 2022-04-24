using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enums
{
    public class EnumCollection : MonoBehaviour
    {
        
    }

    public enum Player
    {
        None, Player1, Player2
    }

    public enum Character
    {
        Sonic, Tails, Knuckles
    }

    public enum Board
    {
        BlueStar, RedRock
    }

    public enum CharacterType
    {
        Speed, Power, Fly
    }

    public enum CharacterClass
    {
        LateBoost, TopSpeed, Combat, Drift, AllRound, Omni
    }
}
