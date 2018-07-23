using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DeckCounterUI : MonoBehaviour {

    public bool isPlayerOne;
    public Image redImage;
    public Image blueImage;
    public Image greenImage;

    private int[] colourCount;

    Deck deckMang;

    private void Start()
    {
        deckMang = GameObject.Find("DeckManager").GetComponent<Deck>();
    }

    public void TestFillUI()
    {

        if (isPlayerOne)
        {
            colourCount = deckMang.CheckCardsInDeck(deckMang.p1_Deck);
        }
        else
        {
            colourCount = deckMang.CheckCardsInDeck(deckMang.p2_Deck);
        }

        UpdateFillUI(colourCount[0], colourCount[1], colourCount[2]);
    }

    public void UpdateFillUI(int red, int blue, int green)
    {
        redImage.fillAmount = (float)red / 4;
        Debug.Log("red / 4 == " + red / 4);
        blueImage.fillAmount = (float)blue / 4;
        Debug.Log("blue / 4 == " + blue / 4);
        greenImage.fillAmount = (float)green / 4;
        Debug.Log("green / 4 == " + green / 4);

    }
}
