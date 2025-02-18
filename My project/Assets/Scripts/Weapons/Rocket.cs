using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // velocity shouldn't change
    public float despawnTime;
    public bool ignoreSpeed;
    public float speed;
    public GameObject sparkFX;

    private Vector3 dir;
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player" && transform.tag != "Enemy") return;

        if (transform.childCount > 0) {
            if (transform.GetChild(0).gameObject.TryGetComponent<ParticleSystem>(out ParticleSystem ps)) {
                ps.Stop();
            }
            Destroy(transform.GetChild(0).gameObject, 3);
            transform.GetChild(0).parent = null;
        }

        GameObject vfx = Instantiate(sparkFX, transform.position + 0.5f * transform.forward, Quaternion.LookRotation(collision.transform.forward));
        Destroy(vfx, 1.0f);
        Destroy(gameObject);
    }

    void Start()
    {
        dir = Vector3.Normalize(transform.forward);
        Destroy(gameObject, despawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (ignoreSpeed) return;
        transform.position += dir * speed * Time.deltaTime;
    }
}
