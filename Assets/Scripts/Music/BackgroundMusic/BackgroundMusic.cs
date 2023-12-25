using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {

    public static BackgroundMusic Instance { get; private set; }

    private float _volume = 0.2f;

    private AudioSource _audioSource;

    private const string BACKGROUND_MUSIC_PLAYER_PREF = "BackgroundVolume";

    private const float DEFAULT_SAVED_VOLUME = 0.2f;

    private void Awake() {

        if (Instance != null) {
            Debug.Log("there is more than 1 Instance of background music");
        }

        Instance = this;

        _audioSource = GetComponent<AudioSource>();

        if (PlayerPrefs.HasKey(BACKGROUND_MUSIC_PLAYER_PREF)) {
            _volume = PlayerPrefs.GetFloat(BACKGROUND_MUSIC_PLAYER_PREF, DEFAULT_SAVED_VOLUME);
        }

        _audioSource.volume = _volume;

    }


    public void ChangeVolume(float volume) {

        _volume = volume;

        _audioSource.volume = _volume;

        PlayerPrefs.SetFloat(BACKGROUND_MUSIC_PLAYER_PREF, _volume);

        PlayerPrefs.Save();
    }

    public void ResetToDefaultBackgroundSound() {

        _volume = DEFAULT_SAVED_VOLUME;

        _audioSource.volume = _volume;

        PlayerPrefs.SetFloat(BACKGROUND_MUSIC_PLAYER_PREF, _volume);

        PlayerPrefs.Save();

    }

    public float GetVolume() {
        return _volume;
    }

    public float GetDefaultVolume() {
        return DEFAULT_SAVED_VOLUME;
    }

}
