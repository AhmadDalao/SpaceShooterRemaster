using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader {


    public enum Scene {
        MainMenu,
        LoadingScene,
        MainScene
    }

    private static Scene targetScene;


    public static void Loader(Scene sceneToLoad) {
        targetScene = sceneToLoad;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback() {
        SceneManager.LoadScene(targetScene.ToString());
    }





}
