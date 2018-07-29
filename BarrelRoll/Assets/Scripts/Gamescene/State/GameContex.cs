//////////////////////////////////////////
//	Create by Leonard Marineau-Quintal  //
//		www.leoquintgames.com			//
//////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using eGameState = GameState.eGameState;

public class GameContex : MonoBehaviour {

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
    protected GameState m_CurrentState;
    ////////////////////////////////
    ///			Private			 ///
    ////////////////////////////////

    #region Unity API
    private void Awake()
    {
        ServiceLocator.RegisterGameContex(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.UnregisterGameContex(this);
    }
    #endregion

    #region Public API
    public void Update()
    {
        if (m_CurrentState != null)
        {
            m_CurrentState.UpdateState();
        }
    }

    public void SetState(eGameState state)
    {

    }
    #endregion

    #region Protect
    #endregion

    #region Private
    #endregion
}
