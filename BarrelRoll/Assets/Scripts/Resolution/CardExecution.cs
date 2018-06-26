using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardExecution : MonoBehaviour{

    public int storedAttkValue;
    public int attkPowerStage;
    public int evdPowerStage;
    public int resolvedValue;
    public Card.eCardType attackType;

    //How many of a card type is hand when played
    public int testHandNmbrAttk;
    public int testHandNmbrEvd;

    //public Card.eCardType evadeType;

    private ShipManager shipMang;
    private Ship evadingShip;

    private void Awake()
    {
        shipMang = GameObject.Find("ShipManager").GetComponent<ShipManager>();
    }


    public void AttackTrigger(Card attkInfo)
    {
        CheckEvade();
        storedAttkValue = attkInfo.powerLevel;
        attackType = attkInfo.type;
        //attkPowerStage == How many of card type are in attack hand? Set what power stage the attack will be at. 1 = One card, 2 = Two cards, 3 = Three cards
        Debug.Log("storedAttkValue = " + storedAttkValue + " at power stage " + testHandNmbrAttk);
    }

    public void EvadeTrigger(Card evadeInfo)
    {
        //evdPowerStage == How many of card type are in evade hand? Set what power stage the evade will be at. 1 = One card, 2 = Two cards, 3 = Three cards
        if (evadeInfo.type == Card.eCardType.Spin)
        {
            if(attackType == Card.eCardType.Laser)
            {
                FullEvade(evadeInfo);
            }

            if (attackType == Card.eCardType.Bomb)
            {
                NoEvade(evadeInfo);
            }

            if (attackType == Card.eCardType.Charge)
            {
                PartialEvade(evadeInfo);
            }
        }

        if (evadeInfo.type == Card.eCardType.Uturn)
        {
            if (attackType == Card.eCardType.Laser)
            {
                NoEvade(evadeInfo);
            }

            if (attackType == Card.eCardType.Bomb)
            {
                PartialEvade(evadeInfo);
            }

            if (attackType == Card.eCardType.Charge)
            {
                FullEvade(evadeInfo);
            }
        }

        if (evadeInfo.type == Card.eCardType.Boost)
        {
            if (attackType == Card.eCardType.Laser)
            {
                PartialEvade(evadeInfo);
            }

            if (attackType == Card.eCardType.Bomb)
            {
                FullEvade(evadeInfo);
            }

            if (attackType == Card.eCardType.Charge)
            {
                NoEvade(evadeInfo);
            }
        }
    }


    void FullEvade(Card evdCard)
    {
        Debug.Log("Full evade triggered. Evade retains full value");
        evadingShip.currentHealth = evadingShip.currentHealth - ((storedAttkValue * testHandNmbrAttk) - (evdCard.powerLevel * testHandNmbrEvd));
        shipMang.UpdateHealth();
        Debug.Log("Player One ship health is now at " + shipMang.pOneShip.currentHealth + ", and Player Two ship health is now at " + shipMang.pTwoShip.currentHealth);
    }

    void PartialEvade(Card evdCard)
    {
        Debug.Log("Partial Evade Trigger. Evade value reduced by one.");
        int attkTotal = storedAttkValue * testHandNmbrAttk;
        int evdTotal = (evdCard.powerLevel - 1) * testHandNmbrEvd;
        evadingShip.currentHealth = evadingShip.currentHealth - (attkTotal - evdTotal);
        shipMang.UpdateHealth();
        Debug.Log("Player One ship health is now at " + shipMang.pOneShip.currentHealth + ", and Player Two ship health is now at " + shipMang.pTwoShip.currentHealth);
    }

    void NoEvade(Card evdCard)
    {
        Debug.Log("No Evade Trigger. Evade value reduced by two");
        int attkTotal = storedAttkValue * testHandNmbrAttk;
        int evdTotal = (evdCard.powerLevel - 2) * testHandNmbrEvd;
        evadingShip.currentHealth = evadingShip.currentHealth - (attkTotal - evdTotal);
        shipMang.UpdateHealth();
        Debug.Log("Player One ship health is now at " + shipMang.pOneShip.currentHealth + ", and Player Two ship health is now at " + shipMang.pTwoShip.currentHealth);
    }

    void CheckEvade()
    {
        if(shipMang.pOneShip.position == Ship.eShipPosition.Evading)
        {
            evadingShip = shipMang.pOneShip;
        }
        else
        {
            evadingShip = shipMang.pTwoShip;
        }
    }



}
