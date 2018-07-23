using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Hand : MonoBehaviour {

    private List<Card> cardHand = new List<Card>();
    private Deck deckMang;
    private CardExecution cardEx;
    public bool isAttacking;
    public Card slotOne;
    public Card slotTwo;
    public Card slotThree;
    public Button slotOneButton;
    public Button slotTwoButton;
    public Button slotThreeButton;

    // Use this for initialization
    void Start ()
    {
		deckMang = GameObject.Find("DeckManager").GetComponent<Deck>();
        cardEx = GameObject.Find("CardExecutionObject").GetComponent<CardExecution>();
        FillHand();
    }

    int handSize = 3;
    public int filledSlots;
    public int redInHand;
    public int blueInHand;
    public int greenInHand;
    public void FillHand()
    {
        for(filledSlots = 0; filledSlots < handSize; filledSlots++)
        {
            cardHand.Add(deckMang.Draw(isAttacking));
        }

        redInHand = 0;
        blueInHand = 0;
        greenInHand = 0;

        foreach (Card card in cardHand)
        {
            if (card.type == Card.eCardType.Bomb || card.type == Card.eCardType.Boost)
            {
                redInHand++;
            }
            if (card.type == Card.eCardType.Charge || card.type == Card.eCardType.Uturn)
            {
                blueInHand++;
            }
            if (card.type == Card.eCardType.Laser || card.type == Card.eCardType.Spin)
            {
                greenInHand++;
            }
        }

        slotOne = cardHand[0];
        slotTwo = cardHand[1];
        slotThree = cardHand[2];
        slotOneButton.image.sprite = cardHand[0].cardArt;
        slotTwoButton.image.sprite = cardHand[1].cardArt;
        slotThreeButton.image.sprite = cardHand[2].cardArt;

    }

    public void CallSlot(int slotNmbr)
    {
        if(slotNmbr == 0)
        {
            if(isAttacking)
            {
                cardEx.AttackTrigger(slotOne);
            }
            else
            {
                cardEx.EvadeTrigger(slotOne);
            }
        }
        if(slotNmbr == 1)
        {
            if (isAttacking)
            {
                cardEx.AttackTrigger(slotTwo);
            }
            else
            {
                cardEx.EvadeTrigger(slotTwo);
            }
        }
        if(slotNmbr == 2)
        {
            if (isAttacking)
            {
                cardEx.AttackTrigger(slotThree);
            }
            else
            {
                cardEx.EvadeTrigger(slotThree);
            }
        }
    }

    public void EmptyHand()
    {
        cardHand.Clear();
    }

    public void ClearAllType(Card.eCardType cardType)
    {
        /*
        foreach(Card card in cardHand)
        {
            if(card.type == cardType)
            {
                Debug.Log(card.ToString());
                //cardHand.Remove(card);
            }
        }
       */

        for (int i = cardHand.Count - 1; i >= 0; i--)
        {
            if(cardHand[i].type == cardType)
            {
                Debug.Log("Removing " + cardHand[i].ToString() + " at index " + i);
                cardHand.RemoveAt(i);
            }
        }

        foreach(Card card in cardHand)
        {
            Debug.Log("Card " + card + "left in hand");
        }
        
    }


}
