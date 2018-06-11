using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardExecution : MonoBehaviour{


    public int storedAttkValue;
    public int resolvedValue;
    public Card.eCardType attackType;
    //public Card.eCardType evadeType;


	public void AttackTrigger(Card attkInfo)
    {
        storedAttkValue = attkInfo.powerLevel;
        attackType = attkInfo.type;
        Debug.Log("storedAttkValue = " + storedAttkValue);
    }

    public void EvadeTrigger(Card evadeInfo)
    {
        if(evadeInfo.type == Card.eCardType.Spin)
        {
            if(attackType == Card.eCardType.Laser)
            {

            }

            if (attackType == Card.eCardType.Bomb)
            {

            }

            if (attackType == Card.eCardType.Charge)
            {

            }
        }

        if (evadeInfo.type == Card.eCardType.Uturn)
        {
            if (attackType == Card.eCardType.Laser)
            {

            }

            if (attackType == Card.eCardType.Bomb)
            {

            }

            if (attackType == Card.eCardType.Charge)
            {

            }
        }

        if (evadeInfo.type == Card.eCardType.Boost)
        {
            if (attackType == Card.eCardType.Laser)
            {

            }

            if (attackType == Card.eCardType.Bomb)
            {

            }

            if (attackType == Card.eCardType.Charge)
            {

            }
        }
        //resolvedValue = storedAttkValue - evdValue;
        /*
        if(resolvedValue <= 0)
        {
            Debug.Log("Attack evaded");
        }
        else
        {
            Debug.Log("Evade takes " + resolvedValue + " damage.");
        }
        */
    }

    void FullEvade()
    {

    }

    void PartialEvade()
    {

    }

    void NoEvade()
    {

    }

}
