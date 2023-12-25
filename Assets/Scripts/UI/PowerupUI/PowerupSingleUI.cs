using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupSingleUI : MonoBehaviour {


    [SerializeField] private List<GameObject> powerupUI_Image;

    private void Awake() {
        foreach (GameObject obj in powerupUI_Image) {
            obj.SetActive(false);
        }

    }

    private void Start() {
        Player.Instance.PowerupPickedUpEvent += Instance_PowerupPickedUpEvent;
        Player.Instance.OnPowerupTimeoutEvent += Instance_OnPowerupTimeoutEvent;
    }

    private void Instance_PowerupPickedUpEvent(object sender, Player.PowerupPickedUpEventArgs e) {
        foreach (GameObject obj in powerupUI_Image)
            if (obj.name == e.PowerupName.ToString()) { // getting some sort of error here.
                obj.SetActive(true);
                if (obj.name == "Shield") continue;
            }
    }

    private void Instance_OnPowerupTimeoutEvent(object sender, Player.OnPowerupTimeoutEventArgs e) {
        foreach (GameObject obj in powerupUI_Image)
            if (obj.name == e.PowerupName) {
                obj.SetActive(false);
            }
    }



}

