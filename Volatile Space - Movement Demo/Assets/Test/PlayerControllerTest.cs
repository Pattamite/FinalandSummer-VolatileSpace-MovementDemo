using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class NewTestScript {

    [UnityTest]
    public IEnumerator ShipIDTest() {
        int shipCount = 3;
        GameTracker gameTracker = new GameObject().AddComponent<GameTracker>();
        gameTracker.playerShips = new PlayerShip[shipCount];
        for (int i = 0; i < shipCount; i++) {
            gameTracker.playerShips[i] = new GameObject().AddComponent<PlayerShip>();
        }

        gameTracker.SetShipsID();
        

        for(int i = 0; i < shipCount; i++) {
            Assert.AreEqual(i, gameTracker.playerShips[i].ID);
        }
        yield return null;
    }

    [UnityTest]
    public IEnumerator InitShipSelectTest () {
        int shipCount = 3;
        GameTracker gameTracker = new GameObject().AddComponent<GameTracker>();
        PlayerController playerController = new GameObject().AddComponent<PlayerController>();
        gameTracker.playerShips = new PlayerShip[shipCount];
        for (int i = 0; i < shipCount; i++) {
            gameTracker.playerShips[i] = new GameObject().AddComponent<PlayerShip>();
        }

        gameTracker.SetShipsID();

        Assert.AreEqual(false, playerController.isAnyShipSelected);
        for (int i = 0; i < shipCount; i++) {
            Assert.AreEqual(false, gameTracker.playerShips[i].isSelected);
        }
        yield return null;
    }

    [UnityTest]
    public IEnumerator ShipSelectAndDeSelectTest () {
        int shipIDToSelect = 0;
        int shipCount = 3;
        GameTracker gameTracker = new GameObject().AddComponent<GameTracker>();
        PlayerController playerController = new GameObject().AddComponent<PlayerController>();
        gameTracker.playerShips = new PlayerShip[shipCount];
        for (int i = 0; i < shipCount; i++) {
            gameTracker.playerShips[i] = new GameObject().AddComponent<PlayerShip>();
        }

        gameTracker.SetShipsID();
        playerController.gameTracker = gameTracker;
        playerController.SelectShip(shipIDToSelect);

        Assert.AreEqual(true, playerController.isAnyShipSelected);
        Assert.AreEqual(shipIDToSelect, playerController.selectedShipID);
        Assert.AreEqual(true, gameTracker.playerShips[shipIDToSelect].isSelected);
        for (int i = 0; i < shipCount; i++) {
            if(i != shipIDToSelect) {
                Assert.AreEqual(false, gameTracker.playerShips[i].isSelected);
            }
        }

        playerController.DeselectAllShip();
        Assert.AreEqual(false, playerController.isAnyShipSelected);
        Assert.AreNotEqual(shipIDToSelect, playerController.selectedShipID);
        for (int i = 0; i < shipCount; i++) {
            Assert.AreEqual(false, gameTracker.playerShips[i].isSelected);
        }

        yield return null;
    }

    [TearDown]
    public void DeleteAllPlayerController () {
        foreach (PlayerController controller in GameObject.FindObjectsOfType<PlayerController>()) {
            Object.Destroy(controller.gameObject);
        }
    }

    [TearDown]
    public void DeleteAllPlayerShip () {
        foreach (PlayerShip controller in GameObject.FindObjectsOfType<PlayerShip>()) {
            Object.Destroy(controller.gameObject);
        }
    }
}
