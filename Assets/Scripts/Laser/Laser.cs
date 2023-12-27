using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    [SerializeField] private PowerupScriptable powerupScriptable;
    [SerializeField] private LayerMask _layerMask;

    public PowerupScriptable GetPowerupScriptable() {

        return powerupScriptable;
    }

    public enum moveDirection {
        up,
    }


    public moveDirection LaserMoveDirection;


    private float maxDistance = 1f;


    private void Update() {

        switch (LaserMoveDirection) {
            case moveDirection.up:
                transform.position += Vector3.up * powerupScriptable.GetSpeed() * Time.deltaTime;

                HandleInteraction();

                if (transform.position.y >= 6f) {
                    if (this.gameObject.transform.parent != null) {
                        Destroy(gameObject);
                        Destroy(transform.parent.gameObject);
                    } else {
                        Destroy(gameObject);
                    }
                }

                break;

        }
    }


    private void HandleInteraction() {

        Vector3 direction = Vector3.one;

        Debug.DrawRay(transform.position, direction, Color.red);

        if (Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, maxDistance, _layerMask)) {

            if (hitInfo.transform.GetComponent<Asteroid>() is Asteroid asteroid) {
                asteroid.DestroySelf();
                MusicManager.Instance.PlayDestroyEnemySound(transform.position);
                SpawnManager.Instance.PlayExplosionAtPoint(asteroid.transform.position);
                GameManager.Instance.AsteroidBeenDestroyed();
                DestroySelf();
            }

        }

    }

    public void DestroySelf() {
        Destroy(this.gameObject);
    }


}
