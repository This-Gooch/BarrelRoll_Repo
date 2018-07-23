using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShipManager : MonoBehaviour {

    public Ship pOneShip;
    public Ship pTwoShip;
    public Image swapMeter;
    public float swapValue;
    public GameObject shipObjectA;
    public GameObject shipObjectB;
    public Image shipHealthUI_A;
    public Image shipHealthUI_B;
    private RectTransform healthRectA;
    private RectTransform healthRectB;

    private Deck deckMang;
    //public GameObject attkPostNode;
    //public GameObject evdPostNode;

    private void Awake()
    {
        deckMang = GameObject.Find("DeckManager").GetComponent<Deck>();
        pOneShip.currentHealth = pOneShip.startHealth;
        pTwoShip.currentHealth = pOneShip.startHealth;
        healthRectA = shipHealthUI_A.GetComponent<RectTransform>();
        healthRectB = shipHealthUI_B.GetComponent<RectTransform>();
      
        if(pOneShip.position == Ship.eShipPosition.Attacking)
        {
            deckMang.FillDecks(true);
            shipObjectA.transform.rotation = Quaternion.Euler(270, 0, 0);
            shipObjectB.transform.rotation = Quaternion.Euler(270, 0, 0);
            //healthRectA.anchoredPosition = new Vector2(0, -230);
            //healthRectB.anchoredPosition = new Vector2(0, 230);

        }
        else
        {
            deckMang.FillDecks(false);
            shipObjectA.transform.rotation = Quaternion.Euler(90, 180, 0);
            shipObjectB.transform.rotation = Quaternion.Euler(90, 180, 0);
           // healthRectA.anchoredPosition = new Vector2(0, 230);
           // healthRectB.anchoredPosition = new Vector2(0, -230);

        }
          
    }

    public void SwapShips()
    {
        if(pOneShip.position == Ship.eShipPosition.Attacking)
        {
            deckMang.FillDecks(false);
            pOneShip.position = Ship.eShipPosition.Evading;
            pTwoShip.position = Ship.eShipPosition.Attacking;
            shipObjectA.transform.rotation = Quaternion.Euler(90, 180, 0);
            shipObjectB.transform.rotation = Quaternion.Euler(90, 180, 0);
            //healthRectA.anchoredPosition = new Vector2(0, 230);
            //healthRectB.anchoredPosition = new Vector2(0, -230);


        }
        else
        {
            deckMang.FillDecks(true);
            pOneShip.position = Ship.eShipPosition.Attacking;
            pTwoShip.position = Ship.eShipPosition.Evading;
            shipObjectA.transform.rotation = Quaternion.Euler(270, 0, 0);
            shipObjectB.transform.rotation = Quaternion.Euler(270, 0, 0);
            healthRectA.anchoredPosition = new Vector2(0, -230);
            healthRectB.anchoredPosition = new Vector2(0, 230);

        }
        Debug.Log("Ship_A is now in the " + pOneShip.position + " position.");
        Debug.Log("Ship_B is now in the " + pTwoShip.position + " position.");
    }

    private void Update()
    {
        HealthCheck();
        swapMeter.fillAmount = swapValue * 0.01f;
    }

    public void UpdateHealth()
    {
        shipHealthUI_A.fillAmount = (float)pOneShip.currentHealth / pOneShip.startHealth;
        shipHealthUI_B.fillAmount = (float)pTwoShip.currentHealth / pTwoShip.startHealth;
    }

    public void HealthCheck()
    {
        if(pOneShip.currentHealth <= 0)
        {
            Debug.Log("Player One Ship is destroyed");
        }
        if(pTwoShip.currentHealth <= 0)
        {
            Debug.Log("Player Two Ship is destroyed");
        }
    }

    public void CheckSwapValue()
    {
        if(swapValue >= 100)
        {
            swapValue = 0;
            SwapShips();
            Debug.Log("SwapShips has been called.");
        }
    }
}
