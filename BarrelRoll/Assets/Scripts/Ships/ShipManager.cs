using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ship", menuName = "ScriptableObject/Ship", order = 2)]
public class ShipManager : ScriptableObject {

    public enum eShipPosition
    {
        Attacking,
        Evading
    }

    public int health;
    public int shields;
    public Animation idle;
    public eShipPosition position;


}
