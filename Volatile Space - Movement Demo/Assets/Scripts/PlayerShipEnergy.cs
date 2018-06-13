using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipEnergy : MonoBehaviour {
    public int maxEnergy = 10;
    public int maxShield = 5;
    public int maxWeapon = 5;
    public int maxMove = 5;

    public int currentShield { get; private set; }
    public int currentWeapon { get; private set; }
    public int currentMove { get; private set; }

    void Start () {
        SetInitialValue();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SetInitialValue () {
        currentShield = 0;
        currentWeapon = 0;
        currentMove = 0;
    }

    public int RemainingEnergy () {
        return maxEnergy - (currentShield + currentWeapon + currentMove);
    }

    public bool SetEnergy(int shield, int weapon, int move) {
        if((shield + weapon + move) <= maxEnergy) {
            currentShield = Mathf.Clamp(shield, 0, maxShield);
            currentWeapon = Mathf.Clamp(weapon, 0, maxWeapon);
            currentMove = Mathf.Clamp(move, 0, maxMove);

            return true;
        }
        else {
            /*if(shield != currentShield) {
                currentShield = Mathf.Clamp(shield, 0, maxShield);
                currentWeapon = Mathf.Clamp((int)((float)weapon / ((float)(shield + weapon + move)) * (float)maxEnergy), 0, maxWeapon);
                currentMove = Mathf.Clamp(maxEnergy - (currentWeapon + currentShield), 0, maxMove);
            }
            else if (weapon != currentWeapon) {
                currentWeapon = Mathf.Clamp(weapon, 0, maxWeapon);
                currentMove = Mathf.Clamp((int)((float)move / ((float)(shield + weapon + move)) * (float)maxEnergy), 0, maxMove);
                currentShield = Mathf.Clamp(maxEnergy - (currentMove + currentWeapon), 0, maxShield);
            }
            else if (move != currentMove) {
                currentShield = Mathf.Clamp((int)((float)shield / ((float)(shield + weapon + move)) * (float)maxEnergy), 0, maxShield);
                currentWeapon = Mathf.Clamp(maxEnergy - (currentShield + currentMove), 0, maxWeapon);
            }
            else {
                currentMove = Mathf.Clamp(move, 0, maxMove);
                currentShield = Mathf.Clamp((int)((float)shield / ((float)(shield + weapon + move)) * (float)maxEnergy), 0, maxShield);
                currentWeapon = Mathf.Clamp((int)((float)weapon / ((float)(shield + weapon + move)) * (float)maxEnergy), 0, maxWeapon);
                currentMove = Mathf.Clamp(maxEnergy - (currentWeapon + currentShield), 0, maxMove);
            }*/
            return false;
        }
    }
}
