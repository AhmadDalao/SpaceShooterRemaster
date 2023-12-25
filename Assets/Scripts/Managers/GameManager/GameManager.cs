using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChange;

    public event EventHandler OnPauseShowUIEvent;

    public event EventHandler OnPauseHideUIEvent;


    private bool _HasAsteroidBeenDestroyed;

    public enum GameState {
        waitingToPlay,
        playingGame,
        gameOver
    }

    private GameState state;


    private bool _togglePauseMenu = false;

    private void Awake() {

        if (Instance != null) {
            Debug.Log("there is more than 1 instance gameManager");
        }

        Instance = this;
    }

    private void Start() {
        state = GameState.waitingToPlay;


        GameInputManager.Instance.OnPauseEvent += Instance_OnPauseEvent;

    }

    private void Instance_OnPauseEvent(object sender, EventArgs e) {

        TogglePauseMenu();


    }

    private void Update() {
        switch (state) {
            case GameState.waitingToPlay:
                if (_HasAsteroidBeenDestroyed) {
                    state = GameState.playingGame;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                Debug.Log("waitingToPlay state");
                break;
            case GameState.playingGame:
                Debug.Log("game is in playingGame state");
                if (Player.Instance.GetPlayerHealth() == 0) {
                    state = GameState.gameOver;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.gameOver:
                Debug.Log("game is in gameOver state");
                break;
        }
    }


    public void TogglePauseMenu() {
        _togglePauseMenu = !_togglePauseMenu;

        if (_togglePauseMenu) {
            Time.timeScale = 0f;
            OnPauseShowUIEvent?.Invoke(this, EventArgs.Empty);
        } else {
            Time.timeScale = 1f;
            OnPauseHideUIEvent?.Invoke(this, EventArgs.Empty);
        }

    }


    public bool IsWaitingToPlay() {
        return state == GameState.waitingToPlay;
    }

    public bool IsGameOver() {
        return state == GameState.gameOver;
    }

    public bool IsPlayingGame() {
        return state == GameState.playingGame;
    }

    public void AsteroidBeenDestroyed() {
        _HasAsteroidBeenDestroyed = true;
    }


}
