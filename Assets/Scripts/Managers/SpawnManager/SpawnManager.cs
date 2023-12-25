using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public static SpawnManager Instance { get; private set; }

    [SerializeField] private GameObject _enemyPrefab;

    [SerializeField] private Transform _spawnLocation;


    [SerializeField] private Transform _spawnLocationPowerup;

    [SerializeField] private PowerUpListScriptable powerupListScriptable;

    [SerializeField] private GameObject _explosion;

    [SerializeField] private Transform _explosionSpawnLocation;

    private List<GameObject> _enemyList;

    private List<GameObject> _powerupList;

    private float spawnTime;

    private float spawnTimerMax = 4f;

    private int spawnCountLimiter = 4;

    private float yOffSet = 6.5f;

    private float spawnTimePowerup;

    private float spawnTimerMaxPowerup = 6f;

    private int spawnCountLimiterPowerup = 1;

    private void Awake() {
        _enemyList = new List<GameObject>();

        _powerupList = new List<GameObject>();

        if (Instance != null) {
            Debug.Log("there is more than 1 spawnManager instance");
        }

        Instance = this;
    }

    private void Start() {
        GameManager.Instance.OnStateChange += Instance_OnStateChange;
    }

    private void Instance_OnStateChange(object sender, System.EventArgs e) {
        ClearEnemyFromSpawnManager();
    }

    private void Update() {
        SpawnEnemy();
        SpawnPowerup();
    }

    private void SpawnEnemy() {
        if (GameManager.Instance.IsPlayingGame()) {
            spawnTime -= Time.deltaTime;
            if (spawnTime < 0f) {
                if (_enemyList.Count < spawnCountLimiter) {
                    spawnTime = spawnTimerMax;
                    GameObject enemyClone = Instantiate(_enemyPrefab, new Vector3(Random.Range(-8, 8), yOffSet, 0f), Quaternion.identity);
                    enemyClone.transform.parent = _spawnLocation;
                    _enemyList.Add(enemyClone);
                }
            }
        }
    }

    private void ClearEnemyFromSpawnManager() {
        if (GameManager.Instance.IsGameOver()) {
            spawnTime = spawnTimerMax;
            foreach (GameObject enemy in _enemyList) {
                Destroy(enemy.gameObject);
            }
            _enemyList = null;

        }
    }

    private void SpawnPowerup() {
        if (GameManager.Instance.IsPlayingGame()) {
            spawnTimePowerup -= Time.deltaTime;
            if (spawnTimePowerup < 0f) {
                if (_powerupList.Count < spawnCountLimiterPowerup) {
                    spawnTimePowerup = spawnTimerMaxPowerup;
                    Transform powerupScriptable = Instantiate(powerupListScriptable.GetPowerupScriptableList().GetPowerUp());
                    powerupScriptable.transform.parent = _spawnLocationPowerup;
                    powerupScriptable.position = new Vector3(Random.Range(-8, 8), yOffSet, 0f);
                    _powerupList.Add(powerupScriptable.GetComponent<Powerup>().gameObject);
                }
            }
        }
    }


    public void RemoveEnemyObjectFromSpawnManagerList(Enemy enemy) {
        _enemyList.Remove(enemy.gameObject);
    }

    public void RemovePowerupObjectFromSpawnManagerList(Powerup powerup) {
        _powerupList.Remove(powerup.gameObject);
    }


    public void PlayExplosionAtPoint(Vector3 point) {
        GameObject explosion = Instantiate(_explosion, point, Quaternion.identity);
        explosion.transform.parent = _explosionSpawnLocation;
        Destroy(explosion, 0.5f);
    }


}
