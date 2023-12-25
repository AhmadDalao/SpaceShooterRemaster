using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour {

    public static event EventHandler OnOptionButtonClickedEvent;

    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _quitToMenuButton;
    [SerializeField] private Button _optionButton;


    private void Awake() {


        _resumeButton.onClick.AddListener(() => {
            GameManager.Instance.TogglePauseMenu();
        });


        _quitToMenuButton.onClick.AddListener(() => {
            SceneLoader.Loader(SceneLoader.Scene.MainMenu);
        });

        _optionButton.onClick.AddListener(() => {
            Hide();
            //  OptionUI.Instance.Show(Show);
            OnOptionButtonClickedEvent?.Invoke(this, EventArgs.Empty);
        });

    }

    private void Start() {

        GameManager.Instance.OnPauseHideUIEvent += Instance_OnPauseHideUIEvent;

        GameManager.Instance.OnPauseShowUIEvent += Instance_OnPauseShowUIEvent;

        OptionUI.OnOptionClosedEvent += OptionUI_OnOptionClosedEvent;

        Hide();

    }

    private void OnDestroy() {
        OnOptionButtonClickedEvent = null;
    }

    private void OptionUI_OnOptionClosedEvent(object sender, EventArgs e) {
        Show();
    }

    private void Instance_OnPauseShowUIEvent(object sender, System.EventArgs e) {
        Show();
    }

    private void Instance_OnPauseHideUIEvent(object sender, System.EventArgs e) {
        Hide();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }



}
