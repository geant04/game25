using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sausage : Enemy
{
    public GameObject BulletPrefab;
    public Transform gunShotPos;

    public float bulletSpeed = 12;
    public float reload = 1.8f;
    public float meleeReload = 1;

    private float reloadTimer, waypointTimer, freezeTimer;

    // Start is called before the first frame update
    void Start()
    {
        cTurnSpeed = 0;
        health = 320;
        speed = 4f;
        turnSpeed = 160;
        isDead = false;
        findPlayer();
        agentSetup();
        state = EnemyState.Idle;
        destination.y = -100;
        reloadTimer = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.Idle && (getDist() < 18 && lineOfSightCheck() || getDist() < 6) ) {
            state = EnemyState.Random;
        }
        if (state == EnemyState.Random) {
            if (getDist() > 26) {
                state = EnemyState.Idle;
            } else if (getDist() < 7) {
                state = EnemyState.Chase;
            }
            
            if (destination.y < -90 || hasReachedDest()) {
                destination = getRandomPoint(6);
                agent.SetDestination(destination);
            }
            
        }
        if (state == EnemyState.Chase) {
            if (getDist() > 9) {
                state = EnemyState.Random;
            }
            
            if (destination.y < -90 || hasReachedDest() || waypointTimer < 0.1f) {
                if (waypointTimer < 0.1f) {
                    waypointTimer = 5;
                }
                destination = getPlayerPoint(1f);
                agent.SetDestination(destination);
            }
        }

        if (state == EnemyState.Chase) {
            Attack();
        } else if (state == EnemyState.Random) {
            Shoot();
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

        if (state != EnemyState.Idle && state != EnemyState.Still) {
            Vector3 v = agent.desiredVelocity;
            v.y = 0;
            transform.position += speed * Time.fixedDeltaTime * Vector3.Normalize(v);
        }

    }
    private void Shoot() {
        if (reloadTimer > 0.1f) return;

        if (!lineOfSightCheck()) {
            reloadTimer = 1;
            return;
        }

        GameObject bullet = Object.Instantiate(BulletPrefab, gunShotPos.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(10 * (Random.value - 0.5f), Vector3.up)
         * (Vector3.Normalize(playerRB.position - transform.position) * bulletSpeed);
        bullet.transform.rotation = Quaternion.LookRotation(bullet.GetComponent<Rigidbody>().velocity);

        reloadTimer = reload;
    }
    private void Attack() {
        if (reloadTimer > 0.1f) return;

        if (getDist() < 2.2f) {
            Debug.Log("about to chomp");
            StartCoroutine(Chomp());
            
            reloadTimer = meleeReload;
        } else {
            reloadTimer = 0.25f;
        }

    }

    private IEnumerator Chomp() {
        //start attack animation here?
        yield return new WaitForSeconds(0.3f);
        if (getDist() < 2.5f) {
            Debug.Log("Player chomped on");
            playerScript.TakeDamage(1);
            freezeTimer = 0.3f;
        }
    }
}
