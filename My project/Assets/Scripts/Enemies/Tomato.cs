using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : Enemy
{
    public GameObject BulletPrefab;
    public Transform gunShotPos;
    private bool goUp;

    private float reloadTimer;

    // Start is called before the first frame update
    void Start()
    {
        cSpeed = 0;
        cTurnSpeed = 0;
        health = 200;
        speed = 1;
        isDead = false;
        findPlayer();
        agentSetup();
        state = Idle;
        goUp = true;
        destination.y = -100;
        reloadTimer = 3;
    }

    // Update is called once per frame
    void Update()
    {

        if (state == Idle && (getDist() < 14 && lineOfSightCheck() || getDist() < 6) ) {
            state = Random;
        }
        if (state == Random) {
            if (getDist() > 14) {
                state = Idle;
            } else if (getDist() < 7) {
                state = Still;
            }

            if (destination.y < -90 || hasReachedDest()) {
                destination = getRandomPoint(5);
                agent.SetDestination(destination);
            }
            
        }
        if (state == Still) {
            if (getDist() > 7) {
                state = Random;
            }
        }

        if (state != Idle) {
            Shoot();
        }



        if (state == Idle) return;

        agent.nextPosition = transform.position;

        turnTowardsVector(Vector3.Normalize(playerRB.position - transform.position));

        transform.eulerAngles += cTurnSpeed * Time.deltaTime * Vector3.up;

        reloadTimer = TimerF(reloadTimer);

    }
    void FixedUpdate() {
        if (transform.y < playerRB.y) {
            goUp = true;
        } else if (transform.y > 3 * playerRB.y) {
            goUp = false;
        }
        transform.position += (goUp ? 1 : -1)  * Time.fixedDeltaTime * Vector3.up;
        

        if (state != Idle && state != Still) {
            Vector3 v = enemy.agent.desiredVelocity;
            v.y = 0;
            transform.position += speed * Time.fixedDeltaTime * Vector3.Normalize(v);
        }

    }
    private void Shoot() {
        if (reloadTimer > 0.1f) return;

        GameObject bullet = Object.Instantiate(BulletPrefab, gunShotPos.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(10 * (Random.value - 0.5f), Vector3.up)
         * (Vector3.Normalize(playerRB.position - transform.position) * 3);
        bullet.transform.rotation = Quaternion.LookRotation(bullet.GetComponent<Rigidbody>().velocity);

        reloadTimer = 4;
    }
}
