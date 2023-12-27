using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour {

    // target which enemy will follow in  this case the player
    [SerializeField] private Transform _targetPlayer;
    // speed which enemy moves at.
    private float _speed = 4f;
    // distance which is used to stop the enemy movement once it reach this distance
    [SerializeField] private float _minimumDistance = 2f;


    private Vector3 _positionDistance;
    private float _atan2;

    public enum EnemyState {
        FollowPlayer,
        EnemyRetreat,
    }

    public EnemyState _enemyState;

    private void Start() {
        _targetPlayer = GameObject.FindObjectOfType<Player>().GetComponent<Transform>();

        if (_targetPlayer == null) {
            Debug.Log("target is null");
        }
    }

    private void Update() {

        switch (_enemyState) {


            case EnemyState.FollowPlayer:

                Debug.Log(_enemyState);

                // enemy follow player

                // we need to check the distance between our enemy and player
                // first param is our current position
                // second param is the player current position 
                // we check if the distance between these two is greater than the distance to stop the enemy from moving.
                if (Vector2.Distance(transform.position, _targetPlayer.position) > _minimumDistance) {

                    // first transform.position ======= is our current position " enemy "
                    // _targetPlayer.position ======= is where we want to go after in this case  " Player "
                    // _speed ===== is the speed we want our enemy to move.

                    transform.position = Vector2.MoveTowards(transform.position, _targetPlayer.position, _speed * Time.deltaTime);

                    RotateEnemy();


                } else {
                    // enemy attack should be done here.
                }

                break;


            case EnemyState.EnemyRetreat:

                Debug.Log(_enemyState);

                // enemy run away from player

                // we need to check the distance between our enemy and player
                // first param is our current position
                // second param is the player current position 
                // we check if the distance between these two is less than the distance to stop the enemy from moving.

                // While retreating this is the best distance as I found.
                //   _minimumDistance = 4f; 

                if (Vector2.Distance(transform.position, _targetPlayer.position) < _minimumDistance) {

                    // first transform.position ======= is our current position " enemy "
                    // _targetPlayer.position ======= is where we want to go after in this case  " Player "
                    // make sure to ad ( - ) _speed ===== is the speed we want our enemy to move while falling back.

                    transform.position = Vector2.MoveTowards(transform.position, _targetPlayer.position, -_speed * Time.deltaTime);
                } else {
                    // enemy attack should be done here.
                }

                break;

        }



    }


    private void RotateEnemy() {
        _positionDistance = (_targetPlayer.transform.position - transform.position);
        _atan2 = Mathf.Atan2(_positionDistance.y, _positionDistance.x);
        transform.rotation = Quaternion.Euler(0f, 0f, _atan2 * Mathf.Rad2Deg + 90f);
    }

}
