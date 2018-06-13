using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTracker : MonoBehaviour {
    public PlayerShip[] playerShips;

    public static int STATE_PLAN = 0;
    public static int STATE_EXECUTE = 1;
    public int currentState { get; private set; }
    public bool isDebug;

    // Use this for initialization
    void Start () {
        SetInitialValue();
        StartPlanState();
        if (isDebug) SetShipsID();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateState();
    }

    private void SetInitialValue () {
        
    }

    public void SetShipsID () {
        for (int i = 0; i < playerShips.Length; i++) {
            playerShips[i].ID = i;
        }
    }

    public bool IsAllShipDone () {
        bool answer = true;
        foreach(PlayerShip ship in playerShips) {
            answer &= ship.isExecuteDone;
        }
        return answer;
    }

    public void StartPlanState () {
        currentState = STATE_PLAN;

        foreach (PlayerShip ship in playerShips) {
            ship.StartPlanState();
        }
    }

    public void StartExecuteState () {
        currentState = STATE_EXECUTE;

        foreach (PlayerShip ship in playerShips) {
            ship.StartExecuteState();
        }
    }

    private void UpdateState () {
        if (currentState == STATE_EXECUTE) {
            if (IsAllShipDone()) {
                StartPlanState();
            }
        }
    }

    public void SetShipTargetPosition(int ID, Vector2 position) {
        playerShips[ID].SetTargetPosition(position);
    }

    public void ResetShipTargetPosition(int ID) {
        playerShips[ID].SetTargetPosition(playerShips[ID].transform.position);
    }

    public PlayerShipEnergy GetPlayerShipEnergy(int ID) {
        return playerShips[ID].gameObject.GetComponent<PlayerShipEnergy>();
    }
}
