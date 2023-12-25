using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI _scoreText;



    private void Update() {
        _scoreText.text = "Score: " + Player.Instance.GetPlayerScore().ToString();
    }




}
