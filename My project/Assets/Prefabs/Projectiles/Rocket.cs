using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // velocity shouldn't change
    public float despawnTime;
    public float speed;
    public GameObject sparkFX;

    private Vector3 dir;
    
    void OnCollisionEnter(Collision collision)
    {
        GameObject vfx = Instantiate(sparkFX, transform.position, Quaternion.LookRotation(collision.transform.forward));
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
        transform.position += dir * speed * Time.deltaTime;
    }
}
