using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioRefScriptable : ScriptableObject {



    [SerializeField] private AudioClip _laser;
    [SerializeField] private AudioClip _explosion;
    [SerializeField] private AudioClip _powerup;



    public AudioClip GetLaserSound() {
        return _laser;
    }

    public AudioClip GetExplosionSound() {
        return _explosion;
    }

    public AudioClip GetPowerupSound() {
        return _powerup;
    }

}
