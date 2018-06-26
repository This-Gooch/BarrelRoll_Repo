using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ship", menuName = "ScriptableObject/Ship", order = 2)]
public class Ship: ScriptableObject {

    public enum eShipPosition
    {
        Attacking,
        Evading
    }

    public enum eSpecializedAttk
    {
        Laser,
        Bomb,
        Charge
    }

    public enum eSpecializedEvd
    {
        Spin,
        Boost,
        Uturn
    }


    public int startHealth;
    public int currentHealth;
    public int shields;
    public Animation idle;
    public eShipPosition position;
    public eSpecializedAttk attkSpecialization;
    public eSpecializedEvd evdSpecialization;


}
