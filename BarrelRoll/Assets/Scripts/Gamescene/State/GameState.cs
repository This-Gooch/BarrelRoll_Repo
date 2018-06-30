//////////////////////////////////////////
//	Create by Leonard Marineau-Quintal  //
//		www.leoquintgames.com			//
//////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

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
    public enum eGameState
    {
        Idle,
        Attack,
        TransitionToDefence,
        Defence,
        TransitionToAttack,
        GameOver
    }

    public eGameState m_State;
    ////////////////////////////////
    ///			Protected		 ///
    ////////////////////////////////

    ////////////////////////////////
    ///			Private			 ///
    ////////////////////////////////

    #region Unity API
    #endregion

    #region Public API
    public virtual void OnBegin()
    {

    }

    public void UpdateState()
    {
        if (IsDone())
        {
            OnEnd();
        }
    }

    public virtual bool IsDone()
    {
        return false;
    }

    public virtual void OnEnd()
    {

    }
    #endregion

    #region Protect
    #endregion

    #region Private
    #endregion
}
