using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTracker : MonoBehaviour {
    public int shipsPerSide = 3;
    public PlayerShip[] playerShips;

    public static int STATE_PLAN_PLAYER_1 = 0;
    public static int STATE_PLAN_PLAYER_2 = 1;
    public static int STATE_EXECUTE = 2;
    public static int STATE_END_PLAYER_1 = 3;
    public static int STATE_END_PLAYER_2 = 4;
    public static int STATE_END_DRAW = 5;
    public int currentState { get; private set; }
    public bool isDebug;

    public string player1Tag = "Player";
    public string player2Tag = "Enemy";
    public string currentTag { get; private set; }

    private PlayerController playerController;

    private void OnValidate () {
        if (playerShips.Length != (shipsPerSide * 2)) {
            Array.Resize<PlayerShip>(ref playerShips, shipsPerSide * 2);
        }
    }

    // Use this for initialization
    void Start () {
        SetInitialValue();
        if (isDebug) SetShipsID();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateState();
    }

    private void SetInitialValue () {
        playerController = FindObjectOfType<PlayerController>();
        currentState = STATE_PLAN_PLAYER_1;
        currentTag = player1Tag;
    }

    public void SetShipsID () {
        for (int i = 0; i < playerShips.Length; i++) {
            playerShips[i].ID = i;
        }
    }

    public bool IsAllShipDone () {
        bool answer = true;
        foreach(PlayerShip ship in playerShips) {
            if (ship) {
                answer &= ship.isExecuteDone;
            }
        }
        return answer;
    }

    public void StartPlanPlayer1State () {
        RemoveAllPreviewLine();
        playerController.DeselectAllShip();
        currentState = STATE_PLAN_PLAYER_1;
        currentTag = player1Tag;

        foreach (PlayerShip ship in playerShips) {
            if (ship) {
                ship.StartPlanState();
            }
        }
        CheckShipCount();
    }

    public void StartPlanPlayer2State () {
        RemoveAllPreviewLine();
        playerController.DeselectAllShip();
        currentState = STATE_PLAN_PLAYER_2;
        currentTag = player2Tag;
    }

    public void StartExecuteState () {
        RemoveAllPreviewLine();
        playerController.DeselectAllShip();
        currentState = STATE_EXECUTE;

        foreach (PlayerShip ship in playerShips) {
            if (ship) {
                ship.StartExecuteState();
            }
        }
    }

    private void UpdateState () {
        if (currentState == STATE_EXECUTE) {
            if (IsAllShipDone()) {
                StartPlanPlayer1State();
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

    public void RemoveAllPreviewLine () {
        foreach (PlayerShip ship in playerShips) {
            if (ship) {
                ship.RemovePreViewLine();
            }
        }
    }

    private void CheckShipCount () {
        int p1 = 0;
        int p2 = 0;

        foreach (PlayerShip ship in playerShips) {
            if (ship) {
                if (ship.tag == player1Tag) p1++;
                else if (ship.tag == player2Tag) p2++;
            }
        }

        if(p1 == 0 || p2 == 0) {
            if (p1 == 0 && p2 == 0) {
                currentState = STATE_END_DRAW;
                //show draw
            }
            else if (p1 == 0) {
                currentState = STATE_END_PLAYER_2;
                //show player 2 win
            }
            else if (p2 == 0) {
                currentState = STATE_END_PLAYER_1;
                //show player 1 win
            }
            //show restart or exit button
        }
    }
}
