using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy
{
    public float reload = 2;
    private float reloadTimer, waypointTimer, freezeTimer;

    // Start is called before the first frame update
    void Start()
    {
        cTurnSpeed = 0;
        health = 320;
        speed = 3.4f;
        turnSpeed = 160;
        isDead = false;
        findPlayer();
        agentSetup();
        state = EnemyState.Idle;
        destination.y = -100;
        reloadTimer = 1;
        waypointTimer = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.Idle && (getDist() < 17 && lineOfSightCheck() || getDist() < 6) ) {
            state = EnemyState.Random;
        }
        if (state == EnemyState.Random) {
            if (getDist() > 22) {
                state = EnemyState.Idle;
            } else if (getDist() < 15) {
                state = EnemyState.Chase;
            }

            if (destination.y < -90 || hasReachedDest()) {
                destination = getRandomPoint(6);
                agent.SetDestination(destination);
            }
            
        } else if (state == EnemyState.Chase) {
            if (getDist() > 16) {
                state = EnemyState.Random;
            }

            if (destination.y < -90 || hasReachedDest() || waypointTimer < 0.1f) {
                speed = 4.4f;
                if (waypointTimer < 0.1f) {
                    waypointTimer = 5;
                }
                destination = getPlayerPoint(0.5f);
                agent.SetDestination(destination);
            }
        }

        if (state != EnemyState.Idle) {
            Attack();
        }


        freezeTimer = TimerF(freezeTimer);

        if (state == EnemyState.Idle || freezeTimer > 0.1f) return;

        agent.nextPosition = transform.position;

        turnTowardsVector(playerRB.position - transform.position);

        transform.eulerAngles += cTurnSpeed * Time.deltaTime * Vector3.up;

        reloadTimer = TimerF(reloadTimer);
        waypointTimer = TimerF(waypointTimer);

    }
    void FixedUpdate() {
        
        if (state != EnemyState.Idle) {
            Vector3 v = agent.desiredVelocity;
            v.y = 0;
            transform.position += speed * Time.fixedDeltaTime * Vector3.Normalize(v);
        }

    }
    private void Attack() {
        if (reloadTimer > 0.1f) return;

        if (getDist() < 2.3f) {
            Debug.Log("about to slash");
            StartCoroutine(Slash());
            
            reloadTimer = reload;
        } else {
            reloadTimer = 0.25f;
        }

    }

    private IEnumerator Slash() {
        //start slash animation here?
        yield return new WaitForSeconds(0.3f);
        if (getDist() < 2.7f) {
            Debug.Log("Player slashed");
            playerScript.TakeDamage(1);
            freezeTimer = 0.3f;
        }
    }
}
