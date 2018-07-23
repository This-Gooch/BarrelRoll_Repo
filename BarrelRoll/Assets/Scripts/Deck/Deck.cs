//////////////////////////////////////////
//	Create by Leonard Marineau-Quintal  //
//		www.leoquintgames.com			//
//////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Deck : MonoBehaviour {

    ////////////////////////////////
    ///			Constants		 ///
    ////////////////////////////////

    ////////////////////////////////
    ///			Statics			 ///
    ////////////////////////////////

    ////////////////////////////////
    ///	  Serialized In Editor	 ///
    ////////////////////////////////

    ////////////////////////////////
    ///			Public			 ///
    ////////////////////////////////
    public Card testLaser;
    public Card testBomb;
    public Card testCharge;
    public Card testBoost;
    public Card testSpin;
    public Card testUturn;

    public Card[] attkHand;
    public Card[] evdHand;

    public Stack<Card> p1_Deck = new Stack<Card>();
    public Stack<Card> p2_Deck = new Stack<Card>();
    ////////////////////////////////
    ///			Protected		 ///
    ////////////////////////////////

    ////////////////////////////////
    ///			Private			 ///
    ////////////////////////////////
    private Stack<Card> m_Deck = new Stack<Card>();


    public void FillDecks(bool p1_isAttacking)
    {
        p1_Deck.Clear();
        p2_Deck.Clear();

        if (p1_isAttacking)
        {
            for (int i = 0; i < 3; i++)
            {
                Add(testBomb, p1_Deck);
                Add(testLaser, p1_Deck);
                Add(testCharge, p1_Deck);
                Add(testBoost, p2_Deck);
                Add(testSpin, p2_Deck);
                Add(testUturn, p2_Deck);
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                Add(testBomb, p2_Deck);
                Add(testLaser, p2_Deck);
                Add(testCharge, p2_Deck);
                Add(testBoost, p1_Deck);
                Add(testSpin, p1_Deck);
                Add(testUturn, p1_Deck);
            }
        }

        Shuffle(p1_Deck);
        Shuffle(p2_Deck);
        //Print(p1_Deck);
        //Print(p2_Deck);
    
    }

    #region Unity API
    #endregion

    #region Public API
    public void Add(Card card, Stack<Card> deck)
    {
        deck.Push(card);
    }

    public Card Draw(bool isAttacking)
    {
        if (isAttacking)
        {
            return p1_Deck.Pop();
        }
        else
        {
            return p2_Deck.Pop();
        }
    }

    public void Shuffle(Stack<Card> deck)
    {
        deck.Shuffle();
    }

    public void Print(Stack<Card> deck)
    {
        foreach (Card card in deck)
        {
            if (card.cardPosition == Card.eCardPosition.Attack)
            {
                Debug.Log(card.ToString() + " in the Attack deck.");
            }
            else
            {
                Debug.Log(card.ToString() + " in the Evade deck.");
            }
        }
    }

    public int[] CheckCardsInDeck(Stack<Card> deck)
    {
        int[] cardColourArray;
        cardColourArray = new int[3];
        //array[0] == red
        //array[1] == blue
        //array[2] == green


        foreach (Card card in deck)
        {
            if (card.type == Card.eCardType.Bomb || card.type == Card.eCardType.Boost)
            {
                cardColourArray[0] += 1;
            }
            if (card.type == Card.eCardType.Charge || card.type == Card.eCardType.Uturn)
            {
                cardColourArray[1] += 1;
            }
            if (card.type == Card.eCardType.Laser || card.type == Card.eCardType.Spin)
            {
                cardColourArray[2] += 1;
            }
        }
        Debug.Log("Red = " + cardColourArray[0] + " Blue = " + cardColourArray[1] + " Green = " + cardColourArray[2]);
        return cardColourArray;
    }

    #endregion

    #region Protect
    #endregion

    #region Private
    #endregion
}
