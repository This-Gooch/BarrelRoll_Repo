using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardExecution : MonoBehaviour{

    public int storedAttkValue;
    public int resolvedValue;
    public Hand p1Hand;
    public Hand p2Hand;
    public Card.eCardType attackType;
    private int attkNmbrOfType;
    private int evdNmbrOfType;
    private bool p1_IsAttacking;


    //How many of a card type is hand when played
    //public int testHandNmbrAttk;
    //public int testHandNmbrEvd;

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
        if(p1Hand.isAttacking)
        {
            p1_IsAttacking = true;
        }
        else
        {
            p1_IsAttacking = false;
        }

        if (attackType == Card.eCardType.Bomb)
        {
            if (p1_IsAttacking)
            {
                attkNmbrOfType = p1Hand.redInHand;
            }
            else
            {
                attkNmbrOfType = p2Hand.redInHand;
            }
        }
        if (attackType == Card.eCardType.Charge)
        {
            if (p1_IsAttacking)
            {
                attkNmbrOfType = p1Hand.blueInHand;
            }
            else
            {
                attkNmbrOfType = p2Hand.blueInHand;
            }
        }
        if (attackType == Card.eCardType.Laser)
        {
            if (p1_IsAttacking)
            {
                attkNmbrOfType = p1Hand.greenInHand;
            }
            else
            {
                attkNmbrOfType = p2Hand.greenInHand;
            }
        }
        //attkPowerStage == How many of card type are in attack hand? Set what power stage the attack will be at. 1 = One card, 2 = Two cards, 3 = Three cards
        //sDebug.Log("storedAttkValue = " + storedAttkValue + " at power stage " + testHandNmbrAttk);
    }

    public void EvadeTrigger(Card evadeInfo)
    {
        //evdPowerStage == How many of card type are in evade hand? Set what power stage the evade will be at. 1 = One card, 2 = Two cards, 3 = Three cards
        if (evadeInfo.type == Card.eCardType.Spin)
        {
            if(attackType == Card.eCardType.Laser)
            {
                EvdCheckNmbrOfType(2);
                FullEvade(evadeInfo);
            }

            if (attackType == Card.eCardType.Bomb)
            {
                EvdCheckNmbrOfType(0);
                NoEvade(evadeInfo);
            }

            if (attackType == Card.eCardType.Charge)
            {
                EvdCheckNmbrOfType(1);
                PartialEvade(evadeInfo);
            }
        }

        if (evadeInfo.type == Card.eCardType.Uturn)
        {
            if (attackType == Card.eCardType.Laser)
            {
                EvdCheckNmbrOfType(2);
                NoEvade(evadeInfo);
            }

            if (attackType == Card.eCardType.Bomb)
            {
                EvdCheckNmbrOfType(0);
                PartialEvade(evadeInfo);
            }

            if (attackType == Card.eCardType.Charge)
            {
                EvdCheckNmbrOfType(1);
                FullEvade(evadeInfo);
            }
        }

        if (evadeInfo.type == Card.eCardType.Boost)
        {
            if (attackType == Card.eCardType.Laser)
            {
                EvdCheckNmbrOfType(2);
                PartialEvade(evadeInfo);
            }

            if (attackType == Card.eCardType.Bomb)
            {
                EvdCheckNmbrOfType(0);
                FullEvade(evadeInfo);
            }

            if (attackType == Card.eCardType.Charge)
            {
                EvdCheckNmbrOfType(1);
                NoEvade(evadeInfo);
            }
        }
    }

    void EvdCheckNmbrOfType(int colourNmbr)
    {
        //red = 0 blue = 1 green = 2
        if(p1_IsAttacking)
        {
            if (colourNmbr == 0)
            {
                evdNmbrOfType = p2Hand.redInHand;
            }
            if (colourNmbr == 1)
            {
                evdNmbrOfType = p2Hand.blueInHand;
            }
            if (colourNmbr == 2)
            {
                evdNmbrOfType = p2Hand.greenInHand;
            }
        }
        else
        {
            if (colourNmbr == 0)
            {
                evdNmbrOfType = p1Hand.redInHand;
            }
            if (colourNmbr == 1)
            {
                evdNmbrOfType = p1Hand.blueInHand;
            }
            if (colourNmbr == 2)
            {
                evdNmbrOfType = p1Hand.greenInHand;
            }
        }

    }


    void FullEvade(Card evdCard)
    {
        Debug.Log("Full evade triggered. Evade retains full value");
        evadingShip.currentHealth = evadingShip.currentHealth - ((storedAttkValue * attkNmbrOfType) - (evdCard.powerLevel * evdNmbrOfType));
        shipMang.UpdateHealth();
        Debug.Log("Player One ship health is now at " + shipMang.pOneShip.currentHealth + ", and Player Two ship health is now at " + shipMang.pTwoShip.currentHealth);
        CallClearType(evdCard);
    }

    void PartialEvade(Card evdCard)
    {
        Debug.Log("Partial Evade Trigger. Evade value reduced by one.");
        int attkTotal = storedAttkValue * attkNmbrOfType;
        int evdTotal = (evdCard.powerLevel - 1) * evdNmbrOfType;
        evadingShip.currentHealth = evadingShip.currentHealth - (attkTotal - evdTotal);
        shipMang.UpdateHealth();
        Debug.Log("Player One ship health is now at " + shipMang.pOneShip.currentHealth + ", and Player Two ship health is now at " + shipMang.pTwoShip.currentHealth);
        CallClearType(evdCard);
    }

    void NoEvade(Card evdCard)
    {
        Debug.Log("No Evade Trigger. Evade value reduced by two");
        int attkTotal = storedAttkValue * attkNmbrOfType;
        int evdTotal = (evdCard.powerLevel - 2) * evdNmbrOfType;
        evadingShip.currentHealth = evadingShip.currentHealth - (attkTotal - evdTotal);
        shipMang.UpdateHealth();
        Debug.Log("Player One ship health is now at " + shipMang.pOneShip.currentHealth + ", and Player Two ship health is now at " + shipMang.pTwoShip.currentHealth);
        CallClearType(evdCard);
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

    void CallClearType(Card evdCard)
    {
        if (p1_IsAttacking)
        {
            p1Hand.ClearAllType(attackType);
            p2Hand.ClearAllType(evdCard.type);
        }
        else
        {
            p1Hand.ClearAllType(evdCard.type);
            p2Hand.ClearAllType(attackType);
        }
    }



}
