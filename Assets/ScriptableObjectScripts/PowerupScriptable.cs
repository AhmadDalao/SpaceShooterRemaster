using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PowerupScriptable : ScriptableObject {

    [SerializeField] private Transform powerup;
    [SerializeField] private Sprite powerup_sprit;
    [SerializeField] private float speed;
    [SerializeField] private string powerupName;
    [SerializeField] private Vector3 moveDirection;


    // just in case I want to change the icons using script.
    public Sprite GetPowerupSprite() {
        return powerup_sprit;
    }

    public Transform GetPowerUp() {
        return powerup;
    }

    public float GetSpeed() {
        return speed;
    }

    public string GetPowerupName() {
        return powerupName;
    }

    public Vector3 GetMoveDirection() {
        return moveDirection;
    }

}
