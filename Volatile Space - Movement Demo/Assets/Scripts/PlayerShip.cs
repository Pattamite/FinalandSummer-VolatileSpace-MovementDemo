using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour {
    public bool isDebug;
    public bool useSimepleMovement;
    public bool isSelected { get; private set; }
    public int ID;
    public bool isDrawPreview;
    public bool isExecuteDone;

    [System.NonSerialized]
    public Vector2 targetPosition;

    private Vector3 velocity = Vector3.zero;
    private float angleVelocity = 0;
    public float smoothDistanceTime = 4f;
    public float smoothAngleTime = 1f;

    [Range(1f, 5f)]
    public float movementLimitValue = 2.5f;

    public LineRenderer travelLine;
    public SpriteRenderer shipSprite;
    public SpriteRenderer selectSprite;
    public SpriteRenderer weaponAreaSprite;
    public bool isActivate;
    public float minDistance = 0.01f;
    public float minAngle = 1f;

    public float timeLimit = 5f;
    private float currentTime = 0f;

    public PlayerShipWeapon weapon;
    private PlayerShipEnergy energy;

    public int maxHP = 1;
    private int currentHP;

    public float[] maxDistance;
    public float[] maxAngle;

    private float targetAngle;

    // Use this for initialization
    void Start () {
        SetInitialValue();
        Deselect();
    }
	
	// Update is called once per frame
	void Update () {
        if (isActivate) {
            Movement();
            DrawPreviewLine();
            CheckActivateTime();
        }
	}

    private void OnValidate () {
        energy = GetComponent<PlayerShipEnergy>();
        if (maxDistance.Length != (energy.maxMove + 1)) {
            Array.Resize<float>(ref maxDistance, energy.maxMove + 1);
        }

        if (maxAngle.Length != (energy.maxMove + 1)) {
            Array.Resize<float>(ref maxAngle, energy.maxMove + 1);
        }
    }

    private void SetInitialValue () {
        isSelected = false;
        isActivate = false;
        isExecuteDone = false;
        currentHP = maxHP;
        energy = GetComponent<PlayerShipEnergy>();
        targetAngle = transform.eulerAngles.z;
    }

    private void Movement () {
        Vector3 target = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
        float currentDistance = Vector2.Distance(transform.position, targetPosition);

        if (useSimepleMovement) {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, targetAngle);

            if (currentDistance <= minDistance) {
                transform.position = target;
                ExecuteDone();
            }
            else {
                transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothDistanceTime);
            }
        }
        else {
            if (currentDistance <= minDistance) {
                transform.position = target;
                ExecuteDone();
            }
            else {
                targetAngle = Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x) * Mathf.Rad2Deg;
                float deltaAngle = Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle);
                float absDeltaAngle = Mathf.Abs(deltaAngle);

                if (absDeltaAngle <= minAngle) {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, targetAngle);
                    transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothDistanceTime);
                }
                else {
                    float newAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle, ref angleVelocity, smoothAngleTime);
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newAngle);
                    Vector3 tempTarget = target + (currentDistance * new Vector3(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad), 0));
                    transform.position = Vector3.SmoothDamp(transform.position, tempTarget, ref velocity, smoothDistanceTime);
                }
            }
        }

        
    }

    private void CheckActivateTime () {
        currentTime += Time.deltaTime;
        if(currentTime >= timeLimit) {
            ExecuteDone();
        }
    }

    private void ExecuteDone () {
        //Debug.Log(gameObject.name + " is done!");
        isExecuteDone = true;
    }

    private void OnCollisionEnter2D (Collision2D collision) {
        //Debug.Log("Player (" + gameObject.name + ") hit");
    }

    public void Select () {
        isSelected = true;
        selectSprite.gameObject.SetActive(true);
        weaponAreaSprite.gameObject.SetActive(true);
    }

    public void Deselect () {
        isSelected = false;
        selectSprite.gameObject.SetActive(false);
        weaponAreaSprite.gameObject.SetActive(false);
    }

    public void SetTargetPosition (Vector2 position) {
        float distance = Vector2.Distance(transform.position, position);
        targetAngle = Mathf.Atan2(position.y - transform.position.y, position.x - transform.position.x) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle);
        float absDeltaAngle = Mathf.Abs(deltaAngle);
        float currentMaxDistance = maxDistance[energy.currentMove];
        float currentMaxAngle = maxAngle[energy.currentMove];

        if (absDeltaAngle <= currentMaxAngle && distance <= GetMaxDistanceByAngle(deltaAngle, currentMaxDistance)) {
            targetPosition = position;
        }
        else {
            deltaAngle = (deltaAngle / absDeltaAngle) * (absDeltaAngle <= currentMaxAngle ? absDeltaAngle : currentMaxAngle);
            distance = (distance <= GetMaxDistanceByAngle(deltaAngle, currentMaxDistance) ? distance : GetMaxDistanceByAngle(deltaAngle, currentMaxDistance));

            float x = transform.position.x + (distance * Mathf.Cos((transform.eulerAngles.z + deltaAngle) * Mathf.Deg2Rad));
            float y = transform.position.y + (distance * Mathf.Sin((transform.eulerAngles.z + deltaAngle) * Mathf.Deg2Rad));

            targetPosition = new Vector2(x, y);
        }

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
        weapon.ResetAttackCount();
    }

    public void StartExecuteState () {
        currentTime = 0;
        isActivate = true;
        //Debug.Log(gameObject.name + " start");
    }

    public void GetHit(int damage) {
        if(energy.currentShield < damage) {
            currentHP -= (damage - energy.currentShield);
            if (currentHP <= 0) Dead();
        }
    }

    public bool Attack(PlayerShip ship) {
        if (energy.currentWeapon > 0) {
            ship.GetHit(energy.currentWeapon);
            return true;
        }
        else return false;
        
    }

    private void Dead () {
        Destroy(gameObject);
    }

    private float GetMaxDistanceByAngle(float angle, float maxDistance) {
        return ((Mathf.Cos(angle * Mathf.Deg2Rad) + movementLimitValue) / (movementLimitValue + 1)) * maxDistance;
    }
}
