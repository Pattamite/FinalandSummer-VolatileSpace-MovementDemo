using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExecuteButton : MonoBehaviour {

    private Button button;
    private PlayerController playerController;
	
	void Start () {
        button = GetComponent<Button>();
        playerController = GameObject.FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(playerController.currentState == PlayerController.STATE_PLAN) {
            button.interactable = true;
        }
        else {
            button.interactable = false;
        }
	}
}
