using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    [SerializeField] private PowerupScriptable powerupScriptable;

    public PowerupScriptable GetPowerupScriptable() {
        return powerupScriptable;
    }

    public enum PowerUpEnum {
        Speed,
        Shield,
        TripleShoot,
        Health
    }

    public PowerUpEnum powerup;



    private void Update() {

        transform.position += Vector3.down * powerupScriptable.GetSpeed() * Time.deltaTime;

        if (transform.position.y <= -5.5f) {
            SpawnManager.Instance.RemovePowerupObjectFromSpawnManagerList(this);
            Destroy(this.gameObject);

        }

    }


    public PowerUpEnum GetPowerUpEnum() {
        return powerup;
    }


    private void OnTriggerEnter(Collider other) {
        if (other.transform.GetComponent<Player>() is Player player) {

            MusicManager.Instance.PlayPowerupSound(transform.position, 1f);

            switch (GetPowerUpEnum()) {

                case PowerUpEnum.Speed:
                    if (!player.GetIsSpeedPowerupActive()) {
                        player.ActiveSpeedCoroutine();
                    }
                    break;
                case PowerUpEnum.Shield:
                    if (!player.GetIsShieldActive()) {
                        player.ShowPlayerShield();
                    }
                    break;
                case PowerUpEnum.TripleShoot:
                    if (!player.GetIsTripleShotActive()) {
                        player.ActiveTripleShotCoroutine();
                    }
                    break;
                case PowerUpEnum.Health:
                    if (player.GetPlayerHealth() != 0) {
                        HealthUI.Instance.UpdateHealthVisualOnPowerup();
                    }
                    break;
            }
            SpawnManager.Instance.RemovePowerupObjectFromSpawnManagerList(this);
            DestroySelf();

        }
    }


    private void DestroySelf() {
        Destroy(this.gameObject);
    }


}
