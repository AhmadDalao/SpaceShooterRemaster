using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour {



    [SerializeField] private PowerupScriptable powerupScriptable;
    // _laser speed is the speed of which the laser move toward the player
    private float _laserSpeed = 8f;
    // we need the position of the player to let the laser move toward the player
    private Vector3 _targetPlayerPosition;

    private Vector3 _positionDistance;
    private float _atan2;

    // Start is called before the first frame update
    void Start() {
        // find the player object and gets its position saved.
        _targetPlayerPosition = GameObject.FindObjectOfType<Player>().transform.position;

        RotateEnemy();

    }

    // Update is called once per frame
    void Update() {


        // first transform.position ======= is our current position " laser "
        // _targetPlayer.position ======= is where we want to go after in this case  " Player "
        // _laserSpeed ===== is the speed we want our laser to move toward the player.

        transform.position = Vector2.MoveTowards(transform.position, _targetPlayerPosition, _laserSpeed * Time.deltaTime);


        if (transform.position == _targetPlayerPosition) {
            // I already made the logic for this but just for testing.
            Destroy(gameObject);
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



    private void RotateEnemy() {
        _positionDistance = (_targetPlayerPosition - transform.position);
        _atan2 = Mathf.Atan2(_positionDistance.y, _positionDistance.x);
        transform.rotation = Quaternion.Euler(0f, 0f, _atan2 * Mathf.Rad2Deg + 90f);
    }




    public void DestroySelf() {
        Destroy(this.gameObject);
    }


}
