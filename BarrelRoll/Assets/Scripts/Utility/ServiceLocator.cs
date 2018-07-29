//////////////////////////////////////////
//	Create by Leonard Marineau-Quintal  //
//		www.leoquintgames.com			//
//////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator {

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
    private static GameContex m_GameContex;

    //properties
    public static GameContex GameContex { get { return m_GameContex; } }
    #region Unity API
    #endregion

    #region Public API
    public static void RegisterGameContex(GameContex gc)
    {
        if (gc != null)
        {
            m_GameContex = gc;
        }
    }

    public static void UnregisterGameContex(GameContex gc)
    {
        if (gc == m_GameContex)
        {
            m_GameContex = null;
        }
    }
    #endregion

    #region Protect
    #endregion

    #region Private
    #endregion
}
