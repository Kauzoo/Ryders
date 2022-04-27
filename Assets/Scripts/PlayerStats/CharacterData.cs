using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    /// <summary>
    /// SRDX
    /// </summary>
    public float BoostDurationLvl1;
    public float BoostDurationLvl2;
    public float BoostDurationLvl3;
    public Enums.CharacterType ShortCutType;
    public Enums.CharacterType StatsType;
    public Enums.CharacterClass CharacterClass;
    public float BoostSpeed;
    public float BoostChainModifier;
    public float Drift;
    public float TopSpeed;

    public List<float> AdditionalAttributes;
}
