using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public static int STATE_PLAN = 0;
    public static int STATE_EXECUTE = 1;

    public bool isDebug;
    public int UILayer = 5;
    public PlayerShip[] playerShips;

    public int selectedShipID { get; private set; }
    public bool isAnyShipSelected { get; private set; }

    public int currentState { get; private set; }


    // Use this for initialization
    void Start () {
        SetInitialValue();
        StartPlanState();
        if (isDebug) SetShipsID();
    }

	
	// Update is called once per frame
	void Update () {
		
	}

    private void SetInitialValue () {
        selectedShipID = -1;
        isAnyShipSelected = false;
    }

    public void SetShipsID () {
        for(int i = 0; i < playerShips.Length; i++) {
            playerShips[i].ID = i;
        }
    }

    public void SelectShip (int ID) {
        if(isAnyShipSelected) {
            playerShips[selectedShipID].Deselect();
        }
        if(ID >= 0 && ID < playerShips.Length) {
            playerShips[ID].Select();
            selectedShipID = ID;
            isAnyShipSelected = true;
        }
        else {
            Debug.LogError("PlayerController (SelectShip): ID = " + ID.ToString() + " is out of bound");
        }
        
    }

    public void DeselectAllShip () {
        if (isAnyShipSelected) {
            playerShips[selectedShipID].Deselect();
            selectedShipID = -1;
            isAnyShipSelected = false;
        }
    }

    public void MouseClick (Vector2 position) {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, 1 << UILayer);

        if (currentState == STATE_PLAN) {
            if (hit) {
                PlayerShip playerShip = hit.transform.gameObject.GetComponent<PlayerShip>();
                if (playerShip) {
                    SelectShip(playerShip.ID);
                }
                else {
                    //TODO
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

        if (currentState == STATE_PLAN) {
            if (isAnyShipSelected) {
                playerShips[selectedShipID].SetTargetPosition(worldPosition);
            }
            else {
                if (hit) {
                    PlayerShip playerShip = hit.transform.gameObject.GetComponent<PlayerShip>();
                    if (playerShip && !isAnyShipSelected) {
                        SelectShip(playerShip.ID);
                    }
                }
            }
        }
    }

    public void StartPlanState () {
        currentState = STATE_PLAN;

        foreach (PlayerShip ship in playerShips) {
            ship.isActivate = false;
            ship.targetPosition = ship.transform.position;
        }
    }

    public void StartExecuteState () {
        currentState = STATE_EXECUTE;

        foreach(PlayerShip ship in playerShips) {
            ship.isActivate = true;
        }
    }
}
