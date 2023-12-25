using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {


    private float _rotateSpeed = 12f;

    private void Update() {
        RotateObject();
    }

    private void RotateObject() {
        transform.Rotate(Vector3.back * Time.deltaTime * _rotateSpeed);
    }

    public void DestroySelf() {
        Destroy(transform.gameObject);
    }

}
