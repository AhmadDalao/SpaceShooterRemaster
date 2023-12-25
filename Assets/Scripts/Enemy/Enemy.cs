using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy {


    private float _moveSpeed = 3f;

    private float yOffSet = 6.5f;

    [SerializeField] private GameObject _explosionParticle;


    private void Update() {

        transform.position += Vector3.down * _moveSpeed * Time.deltaTime;

        if (transform.position.y <= -5.5f) {

            transform.position = new Vector3(UnityEngine.Random.Range(-8, 8), yOffSet, 0f);

        }

    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Player>() is Player player) {

            BoxCollider box = transform.GetComponent<BoxCollider>();
            box.enabled = false;

            player.PlayerDamaged();

            SpawnManager.Instance.RemoveEnemyObjectFromSpawnManagerList(this);
            MusicManager.Instance.PlayDestroyEnemySound(transform.position);

            SpawnManager.Instance.PlayExplosionAtPoint(transform.position);

            Destroy(this.gameObject);
        }


        if (other.transform.GetComponent<Laser>() is Laser laser) {

            BoxCollider box = transform.GetComponent<BoxCollider>();
            box.enabled = false;

            SpawnManager.Instance.RemoveEnemyObjectFromSpawnManagerList(this);
            MusicManager.Instance.PlayDestroyEnemySound(transform.position);

            Player.Instance.PlayerScoreIncreased(); // increase by 10
            laser.DestroySelf();

            SpawnManager.Instance.PlayExplosionAtPoint(transform.position);

            Destroy(this.gameObject);

        }

    }

}
