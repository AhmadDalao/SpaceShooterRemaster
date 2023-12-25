using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour {

    private bool isLoading = true;

    private void Update() {
        if (isLoading) {
            isLoading = false;
            SceneLoader.LoaderCallback();
        }
    }

}
