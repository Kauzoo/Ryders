using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "CharacterBase", menuName = "ScriptableObjects/Character", order = 1)]
    public class SonicCharacter : ScriptableObject, CharacterBase
    {
        [SerializeField] public float BoostDurationLvl1 { get => BoostDurationLvl1; set => throw new System.NotImplementedException(); }
        [SerializeField] public float BoostDurationLvl2 { get => BoostDurationLvl2; set => throw new System.NotImplementedException(); }
        [SerializeField] public float BoostDurationLvl3 { get => BoostDurationLvl3; set => throw new System.NotImplementedException(); }
        public CharacterType ShortCutType { get => ShortCutType; set => throw new System.NotImplementedException(); }
        public CharacterType StatsType { get => StatsType; set => throw new System.NotImplementedException(); }
        public CharacterClass CharacterClass { get => CharacterClass; set => throw new System.NotImplementedException(); }
        public float BoostSpeed { get => BoostSpeed; set => throw new System.NotImplementedException(); }
        public float BoostChainModifier { get => BoostChainModifier; set => throw new System.NotImplementedException(); }
        public float Drift { get => Drift; set => throw new System.NotImplementedException(); }
        public float TopSpeed { get => TopSpeed; set => throw new System.NotImplementedException(); }
    }
}
