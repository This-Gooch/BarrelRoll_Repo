﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObject/Card", order = 1)]
public class Card : ScriptableObject {

    public enum eCardType
    {
        Laser,
        Bomb,
        Charge,
        Spin,
        Boost,
        Uturn,
        Environment,
        Special
    }

    public enum eCardStage
    {
        StageOne,
        StageTwo,
        StageThree
    }


    public new string name;
    public string description;
    [Tooltip("Base value for damage done and/or damage reduced.")]
    public int powerLevel;
    public eCardType type;
    public eCardStage powerStage;
    public Sprite cardArt;
    public Material visuals;

}
