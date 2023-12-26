using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class EnemyLaser : MonoBehaviour {



    [SerializeField] private PowerupScriptable powerupScriptable;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {


        transform.position += Vector3.down * 5f * Time.deltaTime;


        if (transform.position.y <= -6f) {
            if (this.gameObject.transform.parent != null) {
                DestroySelf();
                Destroy(transform.parent.gameObject);
            } else {
                DestroySelf();
            }
        }



    }



    private void OnTriggerEnter(Collider other) {
        if (other.transform.GetComponent<Player>() is Player player) {
            Debug.Log("i hit the player");
            player.PlayerDamaged();
            MusicManager.Instance.PlayPlayerDamageSound(transform.position);
            DestroySelf();
        }
    }





    public void DestroySelf() {
        Destroy(this.gameObject);
    }

}
