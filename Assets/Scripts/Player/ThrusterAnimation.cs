using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterAnimation : MonoBehaviour {


    private const string THRUSTER = "Thruster";

    private Animator _animator;



    private void Start() {
        _animator = GetComponent<Animator>();
    }


    private void Update() {
        HandleAnimation();
    }

    private void HandleAnimation() {
        _animator.SetBool(THRUSTER, Player.Instance.IsPlayerMoving());
    }


}
