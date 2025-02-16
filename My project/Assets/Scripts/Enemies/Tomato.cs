using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : Enemy
{
    public GameObject BulletPrefab;
    public Transform gunShotPos;

    public float bulletSpeed = 7;
    public float reload = 2;
    private bool goUp;

    private float reloadTimer;

    // Start is called before the first frame update
    void Start()
    {
        cSpeed = 0;
        cTurnSpeed = 0;
        health = 200;
        speed = 2.5f;
        turnSpeed = 140;
        isDead = false;
        findPlayer();
        agentSetup();
        state = EnemyState.Idle;
        goUp = true;
        destination.y = -100;
        reloadTimer = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.Idle && (getDist() < 14 && lineOfSightCheck() || getDist() < 6) ) {
            state = EnemyState.Random;
        }
        if (state == EnemyState.Random) {
            if (getDist() > 14) {
                state = EnemyState.Idle;
            } else if (getDist() < 7) {
                state = EnemyState.Still;
            }

            if (destination.y < -90 || hasReachedDest()) {
                destination = getRandomPoint(5);
                agent.SetDestination(destination);
            }
            
        }
        if (state == EnemyState.Still) {
            if (getDist() > 7) {
                state = EnemyState.Random;
            }
        }

        if (state != EnemyState.Idle) {
            Shoot();
        }



        if (state == EnemyState.Idle) return;

        agent.nextPosition = transform.position;

        turnTowardsVector(playerRB.position - transform.position);

        transform.eulerAngles += cTurnSpeed * Time.deltaTime * Vector3.up;

        reloadTimer = TimerF(reloadTimer);

    }
    void FixedUpdate() {
        if (transform.position.y < playerRB.position.y) {
            goUp = true;
        } else if (transform.position.y > 4 + playerRB.position.y) {
            goUp = false;
        }
        transform.position += (goUp ? 0.9f : -0.9f)  * Time.fixedDeltaTime * Vector3.up;
        

        if (state != EnemyState.Idle && state != EnemyState.Still) {
            Vector3 v = agent.desiredVelocity;
            v.y = 0;
            transform.position += speed * Time.fixedDeltaTime * Vector3.Normalize(v);
        }

    }
    private void Shoot() {
        if (reloadTimer > 0.1f) return;

        GameObject bullet = Object.Instantiate(BulletPrefab, gunShotPos.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(10 * (Random.value - 0.5f), Vector3.up)
         * (Vector3.Normalize(playerRB.position - transform.position) * bulletSpeed);
        bullet.transform.rotation = Quaternion.LookRotation(bullet.GetComponent<Rigidbody>().velocity);

        reloadTimer = reload;
    }
}
