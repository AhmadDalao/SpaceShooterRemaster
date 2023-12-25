using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    public event EventHandler<OnPowerupTimeoutEventArgs> OnPowerupTimeoutEvent;

    public event EventHandler<PowerupPickedUpEventArgs> PowerupPickedUpEvent;

    public class PowerupPickedUpEventArgs : EventArgs {
        public string PowerupName;
    }

    public class OnPowerupTimeoutEventArgs : EventArgs {
        public string PowerupName;
    }


    [SerializeField] private GameInputManager _gameInputManager;

    [SerializeField] private PowerupScriptable _powerupScriptable;

    [SerializeField] private PowerupScriptable _tripleLaserPowerupScriptable;

    [SerializeField] private Transform _laserSpawnLocation;

    [SerializeField] private Transform _shieldPrefab;

    [SerializeField] private LayerMask _layerMask;

    private float _moveSpeed = 8f;

    private int _playerHealth = 3;

    private int _playerMaxHealth = 3;

    private float _offSetY = 1f;

    private float _laserSoundVolume = 1f;

    private float spawnLaser;

    private float spawnLaserDelay = 0.2f;

    private bool _speedPowerup = false;

    private bool _shieldPowerup = false;

    private bool _tripleShootPowerup = false;

    private float _powerUpTimer = 7f;

    private int _playerScore;

    private bool _isPlayerMoving;

    private bool _playerMovingLeft;

    private bool _playerMovingRight;

    private void Awake() {

        if (Instance != null) {
            Debug.LogError("There is more than 1 instance for Player.cs");
        }

        Instance = this;

        _shieldPrefab.gameObject.SetActive(false);
    }

    private void Start() {
        _gameInputManager.OnShootingEvent += _gameInputManager_OnShootingEvent;

        PlayerStartingPosition();

        _playerScore = 0;
    }

    private void PlayerStartingPosition() {

        transform.position = new Vector3(0f, -3f, 0f);

    }

    private void _gameInputManager_OnShootingEvent(object sender, System.EventArgs e) {
        HandleLaserShooting();
    }



    private void Update() {
        HandlePlayerMovement();
    }

    private void HandleLaserShooting() {

        if (spawnLaser < Time.time) {
            spawnLaser = Time.time + spawnLaserDelay;
            if (_tripleShootPowerup) {
                if (transform.gameObject != null) {
                    Transform laserTransform = Instantiate(_tripleLaserPowerupScriptable.GetPowerUp(), new Vector3(transform.position.x, transform.position.y + _offSetY, 0f), Quaternion.identity);
                    laserTransform.SetParent(_laserSpawnLocation);
                    MusicManager.Instance.PlayLaserSound(transform.position, _laserSoundVolume);
                }
            } else {
                if (transform.gameObject != null) {
                    Transform laserTransform = Instantiate(_powerupScriptable.GetPowerUp(), new Vector3(transform.position.x, transform.position.y + _offSetY, 0f), Quaternion.identity);
                    laserTransform.SetParent(_laserSpawnLocation);
                    MusicManager.Instance.PlayLaserSound(transform.position, _laserSoundVolume);
                }
            }
        }

    }

    private void HandlePlayerMovement() {


        Vector2 userInput = _gameInputManager.GetPlayerMovementNormalized();

        Vector3 Direction = new Vector3(userInput.x, userInput.y, 0f);

        _isPlayerMoving = Direction != Vector3.zero;


        // handle the animation
        if (Input.GetKeyDown(KeyCode.A)) {
            _playerMovingLeft = true;
        }

        if (Input.GetKeyUp(KeyCode.A)) {
            _playerMovingLeft = false;

        }

        if (Input.GetKeyDown(KeyCode.D)) {
            _playerMovingRight = true;
        }

        if (Input.GetKeyUp(KeyCode.D)) {
            _playerMovingRight = false;

        }

        // prevent player from going outside the map on the X and Y
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -11f, 11f), Mathf.Clamp(transform.position.y, -4f, 4f), 0f);

        // wrap the player from left to right
        if (transform.position.x <= -10.5f) {
            transform.position = new Vector3(8.9f, transform.position.y, 0f);
        }

        // wrap the player from right to left
        if (transform.position.x >= 10.5f) {
            transform.position = new Vector3(-8.9f, transform.position.y, 0f);
        }


        transform.position += Direction * _moveSpeed * Time.deltaTime;

    }

    private IEnumerator GetSpeedUpForPlayer(float waitTime) {
        while (_speedPowerup) {
            _moveSpeed = 16f;
            yield return new WaitForSeconds(waitTime);
            _moveSpeed = 8f;
            _speedPowerup = false;
            OnPowerupTimeoutEvent?.Invoke(this, new OnPowerupTimeoutEventArgs {
                PowerupName = "Speed"
            });
        }
    }


    private IEnumerator GetTripleShotForPlayer(float waitTime) {
        while (_tripleShootPowerup) {
            yield return new WaitForSeconds(waitTime);
            _tripleShootPowerup = false;
            OnPowerupTimeoutEvent?.Invoke(this, new OnPowerupTimeoutEventArgs {
                PowerupName = "TripleShoot"
            });
        }
    }


    private void HidePlayerShield() {
        _shieldPowerup = false;
        _shieldPrefab.gameObject.SetActive(false);
    }


    public void PlayerDamaged() {
        Debug.Log(_shieldPowerup);
        if (_shieldPowerup) {
            HidePlayerShield();
            OnPowerupTimeoutEvent?.Invoke(this, new OnPowerupTimeoutEventArgs {
                PowerupName = "Shield"
            });
        } else {
            _playerHealth--;

            HealthUI.Instance.UpdateHealthVisualOnDamage();
            if (_playerHealth < 1) {
                Debug.Log("player dead");
                SpawnManager.Instance.PlayExplosionAtPoint(transform.position);
                Destroy(this.gameObject, 0.1f);
            }
        }
    }

    public void PlayerHealthIncreased() {
        if (_playerHealth < _playerMaxHealth) {
            _playerHealth++;
        }
    }

    public int GetPlayerHealth() {
        return _playerHealth;
    }

    public void PlayerScoreIncreased() {
        _playerScore += 10;
    }

    public bool GetIsSpeedPowerupActive() {
        return _speedPowerup;
    }

    public void ActiveSpeedCoroutine() {
        _speedPowerup = true;

        PowerupPickedUpEvent?.Invoke(this, new PowerupPickedUpEventArgs {
            PowerupName = "Speed"
        });

        StartCoroutine("GetSpeedUpForPlayer", _powerUpTimer);
    }

    public bool GetIsShieldActive() {
        return _shieldPowerup;
    }

    public void ShowPlayerShield() {
        _shieldPowerup = true;

        PowerupPickedUpEvent?.Invoke(this, new PowerupPickedUpEventArgs {
            PowerupName = "Shield"
        });

        _shieldPrefab.gameObject.SetActive(true);
    }

    public bool GetIsTripleShotActive() {
        return _tripleShootPowerup;
    }

    public void ActiveTripleShotCoroutine() {
        _tripleShootPowerup = true;

        PowerupPickedUpEvent?.Invoke(this, new PowerupPickedUpEventArgs {
            PowerupName = "TripleShoot"
        });

        StartCoroutine("GetTripleShotForPlayer", _powerUpTimer);
    }

    public bool IsPlayerMoving() {
        return _isPlayerMoving;
    }

    public bool IsPlayerMovingRight() {
        return _playerMovingRight;
    }

    public bool IsPlayerMovingLeft() {
        return _playerMovingLeft;
    }

    public int GetPlayerScore() {
        return _playerScore;
    }

    public int GetPlayerMaxHealth() {
        return _playerMaxHealth;
    }




}
