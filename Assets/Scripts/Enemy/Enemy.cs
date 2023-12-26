using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy {



    private float yOffSet = 6.5f;

    [SerializeField] private GameObject _explosionParticle;
    [SerializeField] private PowerupScriptable _enemyLaserPrefab;
    [SerializeField] private Transform _laserSpawnLocation;

    private float _laserYOffSet = -1.5f;

    private float _laserSoundVolume = 1f;

    private float _enemyLaserTimer;
    private float _enemyTimerMax = 5f;

    private float _speed = 2.5f;

    private void Update() {

        transform.position += Vector3.down * _speed * Time.deltaTime;


        _enemyLaserTimer -= Time.deltaTime;

        if (_enemyLaserTimer < 0f) {
            _enemyLaserTimer = _enemyTimerMax;

            // fire the laser

            Instantiate(_enemyLaserPrefab.GetPowerUp(), new Vector3(transform.position.x, transform.position.y - _laserYOffSet, 0f), Quaternion.identity);
            //    enemyLaserTransform.SetParent(_laserSpawnLocation);
            MusicManager.Instance.PlayEnemyLaserSound(transform.position, _laserSoundVolume);

        }


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
            MusicManager.Instance.PlayPlayerDamageSound(transform.position);
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
