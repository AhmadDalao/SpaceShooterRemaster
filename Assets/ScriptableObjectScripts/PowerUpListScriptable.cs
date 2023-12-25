using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu()]
public class PowerUpListScriptable : ScriptableObject {


    [SerializeField] private List<PowerupScriptable> powerupScripts;

    // return random power up
    public PowerupScriptable GetPowerupScriptableList() {
        return powerupScripts[Random.Range(0, powerupScripts.Count)];
    }


}
