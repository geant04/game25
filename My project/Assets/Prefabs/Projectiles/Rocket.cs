using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // velocity shouldn't change
    public float despawnTime;
    public float speed;
    private Vector3 dir;

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
