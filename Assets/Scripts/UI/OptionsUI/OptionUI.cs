using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour {

    public static event EventHandler OnOptionClosedEvent;


    public static OptionUI Instance { get; private set; }


    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _resetDefaultButton;
    [SerializeField] private Slider SoundEffectSoundSlider;
    [SerializeField] private Slider BackgroundSoundSlider;
    [SerializeField] private TextMeshProUGUI _textPercentageSoundEffect;
    [SerializeField] private TextMeshProUGUI _textPercentageBackground;

    [Header("shooting button")]
    [SerializeField] private Button _shootingButton;
    [SerializeField] private TextMeshProUGUI _shootingButtonText;
    [SerializeField] private TextMeshProUGUI _waitingForInputText;

    [Header("move up button")]
    [SerializeField] private Button _playerMoveUpButton;
    [SerializeField] private TextMeshProUGUI _playerMoveUpButtonText;
    [SerializeField] private TextMeshProUGUI _moveUpWaitingForInputText;

    [Header("move down button")]
    [SerializeField] private Button _playerMoveDownButton;
    [SerializeField] private TextMeshProUGUI _playerMoveDownButtonText;
    [SerializeField] private TextMeshProUGUI _moveDownWaitingForInputText;

    [Header("move left button")]
    [SerializeField] private Button _playerMoveLeftButton;
    [SerializeField] private TextMeshProUGUI _playerMoveLeftButtonText;
    [SerializeField] private TextMeshProUGUI _moveLeftWaitingForInputText;

    [Header("move right button")]
    [SerializeField] private Button _playerMoveRightButton;
    [SerializeField] private TextMeshProUGUI _playerMoveRightButtonText;
    [SerializeField] private TextMeshProUGUI _moveRightWaitingForInputText;



    private void Awake() {

        if (Instance != null) {
            Debug.Log("There more than 1 instance of options ui ");
        }

        Instance = this;

        _closeButton.onClick.AddListener(() => {
            Hide();
            OnOptionClosedEvent?.Invoke(this, EventArgs.Empty);
        });

        SoundEffectSoundSlider.onValueChanged.AddListener(delegate {
            SoundEffectSoundSliderDelegate();
        });

        BackgroundSoundSlider.onValueChanged.AddListener(delegate {
            BackgroundMusicDelegate();
        });

        _resetDefaultButton.onClick.AddListener(() => {
            ResetDefaultSettings();
        });

        _shootingButton.onClick.AddListener(() => {
            ShowWaitingForInput(_shootingButtonText, _waitingForInputText);
            RebindingBinding(GameInputManager.Binding.Shooting);
        });

        _playerMoveUpButton.onClick.AddListener(() => {
            ShowWaitingForInput(_playerMoveUpButtonText, _moveUpWaitingForInputText);
            RebindingBinding(GameInputManager.Binding.Move_up);
        });

        _playerMoveDownButton.onClick.AddListener(() => {
            ShowWaitingForInput(_playerMoveDownButtonText, _moveDownWaitingForInputText);
            RebindingBinding(GameInputManager.Binding.Move_down);
        });

        _playerMoveLeftButton.onClick.AddListener(() => {
            ShowWaitingForInput(_playerMoveLeftButtonText, _moveLeftWaitingForInputText);
            RebindingBinding(GameInputManager.Binding.Move_left);
        });

        _playerMoveRightButton.onClick.AddListener(() => {
            ShowWaitingForInput(_playerMoveRightButtonText, _moveRightWaitingForInputText);
            RebindingBinding(GameInputManager.Binding.Move_right);
        });





    }

    private void Start() {


        SoundEffectSoundSlider.value = MusicManager.Instance.GetVolume();
        BackgroundSoundSlider.value = BackgroundMusic.Instance.GetVolume();

        GameManager.Instance.OnPauseHideUIEvent += Instance_OnPauseHideUIEvent;


        PauseUI.OnOptionButtonClickedEvent += PauseUI_OnOptionButtonClickedEvent;

        UpdateShootingButtonVisual();

        HideWaitingForInput();

        Hide();
    }

    private void PauseUI_OnOptionButtonClickedEvent(object sender, EventArgs e) {

        Show();

    }

    private void Update() {

        BackgroundMusicDelegate();

        SoundEffectSoundSliderDelegate();

    }

    private void OnDestroy() {
        OnOptionClosedEvent = null;
    }

    private void UpdateShootingButtonVisual() {

        _shootingButtonText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Shooting);

        _playerMoveUpButtonText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Move_up);

        _playerMoveDownButtonText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Move_down);

        _playerMoveLeftButtonText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Move_left);

        _playerMoveRightButtonText.text = GameInputManager.Instance.GetBindingText(GameInputManager.Binding.Move_right);

    }

    private void ShowWaitingForInput(TextMeshProUGUI buttonText, TextMeshProUGUI waitingText) {
        buttonText.gameObject.SetActive(false);
        waitingText.gameObject.SetActive(true);
    }


    private void HideWaitingForInput() {
        // Shooting
        _shootingButtonText.gameObject.SetActive(true);
        _waitingForInputText.gameObject.SetActive(false);

        // move up
        _playerMoveUpButtonText.gameObject.SetActive(true);
        _moveUpWaitingForInputText.gameObject.SetActive(false);

        // move down
        _playerMoveDownButtonText.gameObject.SetActive(true);
        _moveDownWaitingForInputText.gameObject.SetActive(false);

        // move left
        _playerMoveLeftButtonText.gameObject.SetActive(true);
        _moveLeftWaitingForInputText.gameObject.SetActive(false);

        // move right
        _playerMoveRightButtonText.gameObject.SetActive(true);
        _moveRightWaitingForInputText.gameObject.SetActive(false);
    }


    private void BackgroundMusicDelegate() {
        BackgroundMusic.Instance.ChangeVolume(BackgroundSoundSlider.value);
        _textPercentageBackground.text = (Mathf.Round(BackgroundSoundSlider.value * 100f)).ToString();
    }

    private void SoundEffectSoundSliderDelegate() {
        MusicManager.Instance.ChangeVolume(SoundEffectSoundSlider.value);
        _textPercentageSoundEffect.text = (Mathf.Round(SoundEffectSoundSlider.value * 100f)).ToString();
    }

    private void Instance_OnPauseHideUIEvent(object sender, EventArgs e) {
        Hide();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void ResetDefaultSettings() {

        // reset background slider and the percentage number
        BackgroundSoundSlider.value = BackgroundMusic.Instance.GetDefaultVolume();
        _textPercentageBackground.text = (Mathf.Round(BackgroundMusic.Instance.GetDefaultVolume() * 100f)).ToString();

        // reset sound slider and the percentage number.

        SoundEffectSoundSlider.value = MusicManager.Instance.GetDefaultVolume();
        _textPercentageSoundEffect.text = (Mathf.Round(MusicManager.Instance.GetDefaultVolume() * 100f)).ToString();

        // reset binding

        GameInputManager.Instance.ResetBinding();
        UpdateShootingButtonVisual();

    }


    private void RebindingBinding(GameInputManager.Binding binding) {

        //  ShowWaitingForInput();

        GameInputManager.Instance.RebindingBinding(binding, () => {
            HideWaitingForInput();
            UpdateShootingButtonVisual();
        });

    }


    private void Show() {
        gameObject.SetActive(true);
    }





}
