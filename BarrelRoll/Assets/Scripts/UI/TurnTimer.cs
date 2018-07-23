using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TurnTimer : MonoBehaviour {

    [Tooltip("Time is in seconds")]
    public float startTime;
    float currentTime;
    public bool isActive;
    Image imageUI;

	// Use this for initialization
	void Awake ()
    {
        imageUI = GetComponent<Image>();
        currentTime = startTime;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(isActive)
        {
            currentTime -= Time.deltaTime;

            if(currentTime <= 0)
            {
                isActive = false;
                currentTime = startTime; ;
            }
        }

        imageUI.fillAmount = currentTime / startTime;
	}
}
