using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExecuteButton : MonoBehaviour {

    private Button button;
    private GameTracker gameTracker;

    void Start () {
        button = GetComponent<Button>();
        gameTracker = GameObject.FindObjectOfType<GameTracker>();
	}
	
	// Update is called once per frame
	void Update () {
		if(gameTracker.currentState == GameTracker.STATE_PLAN) {
            button.interactable = true;
        }
        else {
            button.interactable = false;
        }
	}
}
