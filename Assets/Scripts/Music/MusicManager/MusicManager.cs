using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class MusicManager : MonoBehaviour {

    public static MusicManager Instance { get; private set; }


    [SerializeField] private AudioRefScriptable _audioRef;

    private float _volume = 1f;

    private const string VOLUME_PLAYER_PREF = "volume";

    private const float DEFAULT_SAVED_VOLUME = 1f;


    private void Awake() {

        if (Instance != null) {
            Debug.Log("Music Manager has more than 1 instance");
        }

        Instance = this;


        if (PlayerPrefs.HasKey(VOLUME_PLAYER_PREF)) {
            _volume = PlayerPrefs.GetFloat(VOLUME_PLAYER_PREF, DEFAULT_SAVED_VOLUME);
        }
    }

    private void Start() {



    }

    private void Instance_OnAsteroidDestroyed(object sender, System.EventArgs e) {
        Asteroid asteroid = sender as Asteroid;
        PlaySound(_audioRef.GetExplosionSound(), asteroid.transform.position, 1f);
    }

    public void PlayDestroyEnemySound(Vector3 position, float volume = 1f) {
        PlaySound(_audioRef.GetExplosionSound(), position, volume);
    }

    public void PlayLaserSound(Vector3 position, float volume = 1f) {
        PlaySound(_audioRef.GetLaserSound(), position, volume);
    }


    public void PlayPowerupSound(Vector3 position, float volume = 1f) {
        PlaySound(_audioRef.GetPowerupSound(), position, volume);
    }

    public void PlayPlayerDamageSound(Vector3 position, float volume = 1f) {
        PlaySound(_audioRef.GetPlayerDamagedSound(), position, volume);

    }

    public void PlayEnemyLaserSound(Vector3 position, float volume = 1f) {
        PlaySound(_audioRef.GetEnemyLaserSound(), position, volume);
    }


    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplayer = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplayer * _volume);
    }

    public void ChangeVolume(float volume) {

        _volume = volume;

        PlayerPrefs.SetFloat(VOLUME_PLAYER_PREF, _volume);

        PlayerPrefs.Save();
    }


    public void ResetToDefaultBackgroundSound() {

        _volume = DEFAULT_SAVED_VOLUME;

        PlayerPrefs.SetFloat(VOLUME_PLAYER_PREF, _volume);

        PlayerPrefs.Save();

    }


    public float GetVolume() {
        return _volume;
    }


    public float GetDefaultVolume() {
        return DEFAULT_SAVED_VOLUME;
    }
}
