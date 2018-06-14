using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipWeapon : MonoBehaviour {


    private PlayerShip playerShip;
    public int maxAttackPerTurn = 1;

    private int attackCount;
    private LineRenderer lineRenderer;
	
	void Start () {
        SetInitialValue();
        ResetAttackCount();
    }
	
	void Update () {
		
	}

    private void SetInitialValue () {
        playerShip = transform.parent.GetComponent<PlayerShip>();
        if (!playerShip) Debug.LogError("PlayerShipWeapon (SetInitialValue): PlayerShip not found!");
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnTriggerStay2D (Collider2D collision) {
        if (playerShip.isActivate) {
            Transform otherParentTransform = collision.transform.parent;
            GameObject other;
            if (otherParentTransform) {
                other = otherParentTransform.gameObject;
                PlayerShip ship = other.GetComponent<PlayerShip>();
                if(ship && other.tag == "Enemy") {
                    if(attackCount < maxAttackPerTurn) {
                        if (playerShip.Attack(ship)) {
                            attackCount++;
                            Debug.Log("Attack " + other.name);
                            StopCoroutine(RemoveDraw());
                            DrawFire(ship);
                            StartCoroutine(RemoveDraw());
                        }
                    }
                }
            }
        }
    }

    public void ResetAttackCount () {
        attackCount = 0;
    }

    private void DrawFire (PlayerShip target) {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, target.transform.position);
    }

    public IEnumerator RemoveDraw () {
        yield return new WaitForSeconds(0.1f);
        lineRenderer.positionCount = 0;
        yield return null;
    }
}
