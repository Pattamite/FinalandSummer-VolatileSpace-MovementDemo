using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipWeapon : MonoBehaviour {


    private PlayerShip playerShip;
    public int maxAttackPerTurn = 1;

    private int attackCount;
	
	void Start () {
        SetInitialValue();
        ResetAttackCount();
    }
	
	void Update () {
		
	}

    private void SetInitialValue () {
        playerShip = transform.parent.GetComponent<PlayerShip>();
        if (!playerShip) Debug.LogError("PlayerShipWeapon (SetInitialValue): PlayerShip not found!");
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        if (playerShip.isActivate) {
            Transform otherParentTransform = collision.transform.parent;
            GameObject other;
            if (otherParentTransform) {
                other = otherParentTransform.gameObject;
                PlayerShip ship = other.GetComponent<PlayerShip>();
                if(ship && other.tag == "Enemy") {
                    if(attackCount < maxAttackPerTurn) {
                        attackCount++;
                        Debug.Log("Attack " + other.name);
                    }
                }
            }
        }
    }

    public void ResetAttackCount () {
        attackCount = 0;
    }
}
