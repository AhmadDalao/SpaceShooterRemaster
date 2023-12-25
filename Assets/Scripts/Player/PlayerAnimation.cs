using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    private Animator _animator;

    private const string MOVE_LEFT = "MoveLeft";
    private const string MOVE_RIGHT = "MoveRight";

    private void Start() {
        _animator = GetComponent<Animator>();
    }


    private void Update() {
        PlayerMovingAnimationLeftRight();
    }


    private void PlayerMovingAnimationLeftRight() {
        if (_animator != null) {
            _animator.SetBool(MOVE_LEFT, Player.Instance.IsPlayerMovingLeft());
            _animator.SetBool(MOVE_RIGHT, Player.Instance.IsPlayerMovingRight());
        }
    }


}
