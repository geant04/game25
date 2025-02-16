using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketAOE : MonoBehaviour
{
    private float radius;
    [SerializeField]
    private float maxBlastDamage; // say 100

    // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics.OverlapSphere.html
    void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.TryGetComponent<Enemy>(out Enemy d)) // hit an enemy
            {
                Vector3 hitPos = hitCollider.transform.position;
                float power = Vector3.Distance(hitPos, center) / radius;

                d.TakeDamage((int)(maxBlastDamage * (1.1f - power)));
            }

            else if (hitCollider.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement p)) // hit a player
            {
                Vector3 hitPos = hitCollider.transform.position;
                float power = Vector3.Distance(hitPos, center) / radius;

                p.TakeDamage(1);
            }
        }

        Destroy(this, 0.1f);
    }

    void Start()
    {
        radius = transform.localScale.x;
        ExplosionDamage(transform.position, radius);
    }
}
