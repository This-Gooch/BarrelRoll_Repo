using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObject/Card", order = 1)]
public class Card : ScriptableObject {

    public enum eCardType
    {
        Laser,
        Missile,
        Explosion
    }

    public new string name;
    public string description;
    public eCardType type;
    public Material visuals;

}
