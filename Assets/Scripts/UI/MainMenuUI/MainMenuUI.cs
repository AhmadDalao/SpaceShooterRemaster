using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;


    private void Awake() {

        _playButton.onClick.AddListener(() => {
            SceneLoader.Loader(SceneLoader.Scene.MainScene);
        });

        _quitButton.onClick.AddListener(() => {
            Application.Quit();
        });

        // resetting the time scale in case player quit to main menu while the game is paused that would fix any feature problem.

        Time.timeScale = 1.0f;

    }



}
