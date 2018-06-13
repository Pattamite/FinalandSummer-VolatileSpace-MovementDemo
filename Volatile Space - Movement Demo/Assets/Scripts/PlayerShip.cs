using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour {

    public bool isSelected { get; private set; }
    public int ID;
    public bool isDrawPreview;
    public bool isExecuteDone;

    [System.NonSerialized]
    public Vector2 targetPosition;

    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 4f;

    public LineRenderer travelLine;
    public SpriteRenderer shipSprite;
    public bool isActivate;
    public float minDistance = 0.01f;

	// Use this for initialization
	void Start () {
        SetInitialValue();

    }
	
	// Update is called once per frame
	void Update () {
        if (isActivate) {
            Movement();
            DrawPreviewLine();
        }
	}

    private void SetInitialValue () {
        isSelected = false;
        isActivate = false;
        isExecuteDone = false;
    }

    private void Movement () {
        Vector3 target = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);

        if (Vector2.Distance(transform.position, targetPosition) <= minDistance) {
            transform.position = target;
            if (!isExecuteDone) {
                print(gameObject.name + " is done!");
                isExecuteDone = true;
            }
        }
        else {
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
        }
    }

    private void OnCollisionEnter2D (Collision2D collision) {
        Debug.Log("Player (" + gameObject.name + ") hit");
    }

    public void Select () {
        isSelected = true;

        shipSprite.color = new Color(1, 1, 1, 0.5f);
    }

    public void Deselect () {
        isSelected = false;

        shipSprite.color = new Color(1, 1, 1, 1);
    }

    public void SetTargetPosition (Vector2 position) {
        targetPosition = position;
        DrawPreviewLine();
    }

    public void DrawPreviewLine () {
        travelLine.positionCount = 2;
        travelLine.SetPosition(0, gameObject.transform.position);
        travelLine.SetPosition(1, new Vector3(targetPosition.x, targetPosition.y, gameObject.transform.position.z));
    }

    public void StartPlanState () {
        isExecuteDone = false;
        isActivate = false;
        targetPosition = transform.position;
    }

    public void StartExecuteState () {
        isActivate = true;
        print(gameObject.name + " start");
    }
}
