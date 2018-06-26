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

    ////////////////////////////////
    ///			Protected		 ///
    ////////////////////////////////

    ////////////////////////////////
    ///			Private			 ///
    ////////////////////////////////
    private Stack<Card> m_Deck = new Stack<Card>();



    #region Unity API
    #endregion

    #region Public API
    public void Add(Card card)
    {
        m_Deck.Push(card);
    }

    public Card Draw()
    {
        return m_Deck.Pop();
    }

    public void Shuffle()
    {
        m_Deck.Shuffle();
    }

    public void Print()
    {
        Debug.Log(m_Deck);
    }
    #endregion

    #region Protect
    #endregion

    #region Private
    #endregion
}
