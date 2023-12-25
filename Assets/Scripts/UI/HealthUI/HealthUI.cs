using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {


    public static HealthUI Instance { get; private set; }

    [SerializeField] private Transform _healthContainer;
    [SerializeField] private GameObject _heartIcon;
    [SerializeField] private Color _damagedColor;


    private List<GameObject> _hearts;
    private List<GameObject> _damagedIconList;


    private void Awake() {

        _hearts = new List<GameObject>();
        _damagedIconList = new List<GameObject>();

        _heartIcon.transform.gameObject.SetActive(false);

        if (Instance != null) {
            Debug.Log("Health UI there is more than 1 instance");
        }

        Instance = this;
    }

    private void Start() {
        SpawnHealthBar();
    }



    public void SpawnHealthBar() {
        if (GameManager.Instance.IsWaitingToPlay() || GameManager.Instance.IsPlayingGame()) {
            if (Player.Instance != null) {
                for (var i = 0; i < Player.Instance.GetPlayerHealth(); i++) {
                    GameObject icon = Instantiate(_heartIcon, _healthContainer);
                    icon.gameObject.SetActive(true);
                    _hearts.Add(icon);
                }
            }
        }
    }

    public void UpdateHealthVisualOnDamage() {
        // i might need to clean the list later.
        GameObject icon = _hearts[Player.Instance.GetPlayerHealth()];
        icon.GetComponent<Image>().color = _damagedColor;
        _hearts.Remove(icon);
        _damagedIconList.Add(icon);

    }

    public void UpdateHealthVisualOnPowerup() {

        if (Player.Instance.GetPlayerHealth() == Player.Instance.GetPlayerMaxHealth()) {
            _damagedIconList.Clear();

        } else {

            if (_damagedIconList.Count > 0) {

                GameObject damagedFixed = _damagedIconList[_damagedIconList.Count - 1];

                damagedFixed.GetComponent<Image>().color = new Color(255, 255, 255, 255);

                _hearts.Add(damagedFixed); // add the object in the _heart list

                _damagedIconList.Remove(damagedFixed); // now player healed up remove the object from the damage list

                Player.Instance.PlayerHealthIncreased(); // give the player the extra health ++;

            }
        }


    }
}
