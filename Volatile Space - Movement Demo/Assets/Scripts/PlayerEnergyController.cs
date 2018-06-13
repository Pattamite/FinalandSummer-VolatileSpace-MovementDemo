using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergyController : MonoBehaviour {

    public GameObject energyPanel;
    public Slider energySlider;
    public Slider shieldSlider;
    public Slider weaponSlider;
    public Slider moveSlider;

    private PlayerShipEnergy shipEnergy;


	// Use this for initialization
	void Start () {
        SetInitialValue();
        ShipDeselected();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void SetInitialValue () {
        energySlider.value = 0;
        shieldSlider.value = 0;
        weaponSlider.value = 0;
        moveSlider.value = 0;
    }

    public void ShipSelected(PlayerShipEnergy ship) {
        energyPanel.SetActive(true);
        shipEnergy = ship;
        SetMinMaxValue();
        SetCurrentValue();
    }

    public void ShipDeselected () {
        energyPanel.SetActive(false);
    }

    private void SetMinMaxValue() {
        if (shipEnergy) {
            energySlider.minValue = 0;
            shieldSlider.minValue = 0;
            weaponSlider.minValue = 0;
            moveSlider.minValue = 0;

            energySlider.maxValue = shipEnergy.maxEnergy;
            shieldSlider.maxValue = shipEnergy.maxShield;
            weaponSlider.maxValue = shipEnergy.maxWeapon;
            moveSlider.maxValue = shipEnergy.maxMove;
        }
    }

    private void SetCurrentValue () {
        if (shipEnergy) {
            energySlider.value = shipEnergy.RemainingEnergy();
            shieldSlider.value = shipEnergy.currentShield;
            weaponSlider.value = shipEnergy.currentWeapon;
            moveSlider.value = shipEnergy.currentMove;
        }
    }


    public void ValueChangeCheck () {
        if (shipEnergy.SetEnergy(Mathf.RoundToInt(shieldSlider.value), Mathf.RoundToInt(weaponSlider.value), Mathf.RoundToInt(moveSlider.value))) {
            SetCurrentValue();
        }
        else {
            SetCurrentValue();
        }
    }
}
