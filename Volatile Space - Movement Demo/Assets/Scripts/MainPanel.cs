using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainPanel : MonoBehaviour, IPointerClickHandler, IDragHandler{

    private PlayerController playerController;

    // Use this for initialization
    void Start () {
        SetInitialValue();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SetInitialValue () {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void OnPointerClick (PointerEventData eventData) {
        playerController.MouseClick(eventData.position);
    }

    public void OnDrag (PointerEventData eventData) {
        playerController.MouseDrag(eventData.position);
    }
}
