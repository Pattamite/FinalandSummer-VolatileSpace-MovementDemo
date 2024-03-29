﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public int UILayer = 5;
    public int selectedShipID { get; private set; }
    public bool isAnyShipSelected { get; private set; }

    [System.NonSerialized]
    public GameTracker gameTracker;

    private PlayerEnergyController energyController;

    void Start () {
        SetInitialValue();
    }
	
	void Update () {

    }

    private void SetInitialValue () {
        selectedShipID = -1;
        isAnyShipSelected = false;
        gameTracker = FindObjectOfType<GameTracker>();
        energyController = GetComponent<PlayerEnergyController>();
    }

    public void SelectShip (int ID) {
        if(isAnyShipSelected) {
            gameTracker.playerShips[selectedShipID].Deselect();
        }
        if(ID >= 0 && ID < gameTracker.playerShips.Length) {
            gameTracker.playerShips[ID].Select();
            selectedShipID = ID;
            isAnyShipSelected = true;
            energyController.ShipSelected(gameTracker.GetPlayerShipEnergy(ID));
        }
        else {
            Debug.LogError("PlayerController (SelectShip): ID = " + ID.ToString() + " is out of bound");
        }
        
    }

    public void DeselectAllShip () {
        foreach(PlayerShip ship in gameTracker.playerShips) {
            if (ship) {
                ship.Deselect();
            }
        }
        selectedShipID = -1;
        isAnyShipSelected = false;
        energyController.ShipDeselected();
    }

    public void MouseClick (Vector2 position) {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, 1 << UILayer);

        if (gameTracker.currentState == GameTracker.STATE_PLAN_PLAYER_1 || gameTracker.currentState == GameTracker.STATE_PLAN_PLAYER_2) {
            if (hit) {
                PlayerShip playerShip = hit.transform.gameObject.GetComponent<PlayerShip>();
                print(playerShip);
                if (playerShip && playerShip.tag == gameTracker.currentTag) {
                    SelectShip(playerShip.ID);
                }
            }
            else {
                if (isAnyShipSelected) {
                    DeselectAllShip();
                }
            }
        }
    }

    public void MouseDrag (Vector2 position) {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, 1 << UILayer);

        if (gameTracker.currentState == GameTracker.STATE_PLAN_PLAYER_1 || gameTracker.currentState == GameTracker.STATE_PLAN_PLAYER_2) {
            if (isAnyShipSelected) {
                gameTracker.SetShipTargetPosition(selectedShipID, worldPosition);
            }
            else {
                if (hit) {
                    PlayerShip playerShip = hit.transform.gameObject.GetComponent<PlayerShip>();
                    if (playerShip && playerShip.tag == gameTracker.currentTag) {
                        SelectShip(playerShip.ID);
                    }
                }
            }
        }
    }

    public void CancelPlan () {
        if (isAnyShipSelected) {
            gameTracker.ResetShipTargetPosition(selectedShipID);
            DeselectAllShip();
        }
    }
}
