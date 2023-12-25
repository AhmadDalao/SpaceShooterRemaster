using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static System.Collections.Specialized.BitVector32;

public class GameInputManager : MonoBehaviour {

    public static GameInputManager Instance { get; private set; }

    public event EventHandler OnShootingEvent;

    public event EventHandler OnPauseEvent;

    private InputManager _inputActions;

    private const string PLAYER_KEY_BINDING = "PLAYER_KEY_BINDING";


    private const string PLAYER_MOVE_UP_PATH = "<Keyboard>/w";

    private const string PLAYER_MOVE_DOWN_PATH = "<Keyboard>/s";

    private const string PLAYER_MOVE_LEFT_PATH = "<Keyboard>/a";

    private const string PLAYER_MOVE_RIGHT_PATH = "<Keyboard>/d";





    public enum Binding {
        Move_up,
        Move_down,
        Move_left,
        Move_right,
        Shooting,
        pause
    }

    private void Awake() {

        if (Instance != null) {
            Debug.Log("there is more than 1 game input manager");
        }

        Instance = this;

        _inputActions = new InputManager();

        if (PlayerPrefs.HasKey(PLAYER_KEY_BINDING)) {
            _inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_KEY_BINDING));
        }

        _inputActions.Player.Enable();

        _inputActions.Player.Shooting.performed += Shooting_performed;

        _inputActions.Player.Pause.performed += Pause_performed;

    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPauseEvent?.Invoke(this, EventArgs.Empty);
    }

    private void Shooting_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnShootingEvent?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy() {
        // remove the subscription to the button on destroy to get a new one later on
        _inputActions.Player.Shooting.performed -= Shooting_performed;

        _inputActions.Player.Pause.performed -= Pause_performed;

        // disable the player control;
        _inputActions.Dispose();
    }


    public Vector2 GetPlayerMovementNormalized() {
        Vector2 userInput = _inputActions.Player.Move.ReadValue<Vector2>();
        return userInput.normalized;
    }


    public string GetBindingText(Binding binding) {
        switch (binding) {
            default:
            case Binding.Move_up:
                return _inputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_down:
                return _inputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_left:
                return _inputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_right:
                return _inputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Shooting:
                return _inputActions.Player.Shooting.bindings[0].ToDisplayString();
            case Binding.pause:
                return _inputActions.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindingBinding(Binding binding, Action OnActionRebound) {


        _inputActions.Player.Disable();


        InputAction inputAction;
        int bindingIndex;

        switch (binding) {
            default:
            case Binding.Shooting:
                inputAction = _inputActions.Player.Shooting;
                bindingIndex = 0;
                break;
            case Binding.Move_up:
                inputAction = _inputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_down:
                inputAction = _inputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_left:
                inputAction = _inputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_right:
                inputAction = _inputActions.Player.Move;
                bindingIndex = 4;
                break;
        }

        // that's for moving do it later
        //   _inputActions.Player.Move.PerformInteractiveRebinding(1);

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("<Keyboard>/escape")
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(callback => {

                Debug.Log(callback.action.bindings[1].path);
                Debug.Log(callback.action.bindings[1].overridePath);
                // to prevent an error just in case.
                callback.Dispose();

                _inputActions.Player.Enable();

                // action will be triggered once everything above has been complete.
                OnActionRebound();


                PlayerPrefs.SetString(PLAYER_KEY_BINDING, _inputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

            }).Start();

    }


    public void ResetBinding() {
        // _inputActions.Player.Shooting.bindings[0].overridePath.;
        _inputActions.Player.Shooting.ApplyBindingOverride("<Keyboard>/space");

        // reset move up
        InputBinding inputBindingMoveUp = _inputActions.Player.Move.bindings[1];
        inputBindingMoveUp.overridePath = PLAYER_MOVE_UP_PATH;
        _inputActions.Player.Move.ApplyBindingOverride(1, inputBindingMoveUp);

        // reset move down
        InputBinding inputBindingMoveDown = _inputActions.Player.Move.bindings[2];
        inputBindingMoveDown.overridePath = PLAYER_MOVE_DOWN_PATH;
        _inputActions.Player.Move.ApplyBindingOverride(2, inputBindingMoveDown);

        // reset move left
        InputBinding inputBindingMoveLeft = _inputActions.Player.Move.bindings[3];
        inputBindingMoveLeft.overridePath = PLAYER_MOVE_LEFT_PATH;
        _inputActions.Player.Move.ApplyBindingOverride(3, inputBindingMoveLeft);

        // reset move right
        InputBinding inputBindingMoveRight = _inputActions.Player.Move.bindings[4];
        inputBindingMoveRight.overridePath = PLAYER_MOVE_RIGHT_PATH;
        _inputActions.Player.Move.ApplyBindingOverride(4, inputBindingMoveRight);

        PlayerPrefs.SetString(PLAYER_KEY_BINDING, _inputActions.SaveBindingOverridesAsJson());
        PlayerPrefs.Save();

    }

}
