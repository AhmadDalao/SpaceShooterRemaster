using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy {

    private float yOffSet = 6.5f;

    [SerializeField] private GameObject _explosionParticle;
    [SerializeField] private PowerupScriptable _enemyLaserPrefab;
    private Transform _laserSpawnLocation;

    private float _laserSoundVolume = 1f;

    private float _enemyLaserTimer;
    private float _enemyTimerMax = 1f;


    private void Start() {
        _laserSpawnLocation = GameObject.FindObjectOfType<EnemySpawnLocation>().transform;
    }

    private void Update() {

        //    transform.position += Vector3.down * _speed * Time.deltaTime;


        _enemyLaserTimer -= Time.deltaTime;

        if (_enemyLaserTimer < 0f) {
            _enemyLaserTimer = _enemyTimerMax;

            // fire the laser

            Transform enemyLaserTransform = Instantiate(_enemyLaserPrefab.GetPowerUp(), transform.position, Quaternion.identity);
            enemyLaserTransform.SetParent(_laserSpawnLocation);
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
