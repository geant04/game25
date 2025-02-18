using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Sound Effects")]
    public SoundData hurtSound;
    public SoundData deathSound;

    public int health;
    public float speed, turnSpeed;
    public EnemyState state;
    [HideInInspector] public Vector3 destination;
    [HideInInspector] public UnityEngine.AI.NavMeshAgent agent;

    protected float cTurnSpeed;
    protected bool isDead;
    protected GameObject player;
    protected Rigidbody playerRB;
    protected Player playerScript;

    public void TakeDamage(int amt)
    {
        health = Mathf.Max(0, health - amt);
        SoundManager.Instance.CreateSound()
                    .WithSoundData(hurtSound)
                    .WithPosition(transform.position)
                    .Play();
        Debug.Log(transform.name + " wuz hit, hp: " + health);
        if (health <= 0)
        {
            SoundManager.Instance.CreateSound()
                    .WithSoundData(deathSound)
                    .WithPosition(transform.position)
                    .Play();
            Kill();
        }
    }

    protected void Kill()
    {
        if (isDead) return;

        Debug.Log("deceased");
        isDead = true;
        Destroy(gameObject);
    }

    protected float TimerF(float val)
    {
        if (val > 0)
        {
            val -= Time.deltaTime;
            if (val <= 0) val = 0;
        }
        return val;
    }

    protected void findPlayer() {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) {
            Debug.Log("Could not find player");
        } else {
            playerScript = player.GetComponent<Player>();
            playerRB = playerScript.myRB;
        }
    }
    protected float getDist() {
        return playerRB ? Vector3.Distance(transform.position, playerRB.position) : float.NaN;
    }
    protected bool lineOfSightCheck() {
        if (playerRB == null) {
            return false;
        }
        return !Physics.Raycast(playerRB.position, transform.position - playerRB.position, getDist() - 1, 1 << 3);
    }

    protected void turnTowardsVector(Vector3 v) {
        v.y = 0;
        v = Vector3.Normalize(v);
        float dir = Vector3.Dot(-transform.right, v);
        if (dir > 0.02f) {
            if (cTurnSpeed > 0) {
                cTurnSpeed -= 720 * Time.deltaTime;
            } else {
                cTurnSpeed = Mathf.Max(-turnSpeed, cTurnSpeed - 720 * Time.deltaTime);
            }
        } else if (dir < -0.02f) {
            if (cTurnSpeed < 0) {
                cTurnSpeed += 720 * Time.deltaTime;
            } else {
                cTurnSpeed = Mathf.Min(turnSpeed, cTurnSpeed + 720 * Time.deltaTime);
            }
        } else {
            cTurnSpeed = 0;
        }
    }

    //-------------------------navigation-----------------------------
    protected void agentSetup() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    protected Vector3 getRandomPoint(float radius) {
        for (int i = 0; i < 18; i++)
        {
            Vector3 randomPoint = transform.position + radius * UnityEngine.Random.insideUnitSphere;
            randomPoint.y = 0;
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                if (checkDestinationReachable(hit.position)) {
                    return hit.position;
                }
            }
        }
        return Vector3.zero;
    }

    protected Vector3 getPlayerPoint(float radius) {
        if (playerRB == null) {
            return getRandomPoint(6);
        }
        for (int i = 0; i < 10; i++)
        {
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(playerRB.position + radius * UnityEngine.Random.insideUnitSphere,
                 out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                if (checkDestinationReachable(hit.position)) {
                    return hit.position;
                }
            }
        }
        return getRandomPoint(6);
    }
    protected Vector3 getRandomNavPointAwayFromPlayer(float radius, float avoidRadius) {
        Vector3 p;
        int i = 0;
        do {
            p = getRandomPoint(radius);
            i++;
        } while (Vector2.Distance(new Vector2(playerRB.position.x, playerRB.position.z),
        new Vector2(p.x, p.z)) < avoidRadius && i < 8);
        return p;
    }
    protected bool hasReachedDest() {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
                    new Vector2(destination.x, destination.z)) < 1;
    }
    protected bool checkDestinationReachable(Vector3 destination) {
        var path = new UnityEngine.AI.NavMeshPath();
        agent.CalculatePath(destination, path);
        switch (path.status)
        {
            case UnityEngine.AI.NavMeshPathStatus.PathComplete:
                return true;
                break;
            default:
                return false;
                break;
        }
    }

}

public enum EnemyState {
    Idle,
    Random,
    Chase,
    Still
}
