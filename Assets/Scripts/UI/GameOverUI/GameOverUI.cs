using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {

    private const string BEST_PLAYER_SCORE = "BEST_PLAYER_SCORE";

    [SerializeField] private TextMeshProUGUI _scoreText;

    [SerializeField] private TextMeshProUGUI _bestScoreNumber;

    [SerializeField] private Button _playAgainButton;

    [SerializeField] private Button _quitToMainMenu;


    private int _bestScore = 0;

    private void Awake() {

        _playAgainButton.onClick.AddListener(() => {
            SceneLoader.Loader(SceneLoader.Scene.MainScene);
        });

        _quitToMainMenu.onClick.AddListener(() => {
            SceneLoader.Loader(SceneLoader.Scene.MainMenu);
        });

        if (PlayerPrefs.HasKey(BEST_PLAYER_SCORE)) {
            _bestScore = PlayerPrefs.GetInt(BEST_PLAYER_SCORE);
        }

    }

    private void Start() {
        GameManager.Instance.OnStateChange += Instance_OnStateChange;
        Hide();
    }

    private void Instance_OnStateChange(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsGameOver()) {
            _scoreText.text = Player.Instance.GetPlayerScore().ToString();

            if (Player.Instance.GetPlayerScore() > _bestScore) {
                _bestScore = Player.Instance.GetPlayerScore();
                PlayerPrefs.SetInt(BEST_PLAYER_SCORE, _bestScore);
                PlayerPrefs.Save();

            }
            _bestScoreNumber.text = _bestScore.ToString();

            Show();
        } else {
            Hide();
        }
    }



    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }




}
