using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : HitScanWeapon
{
    public GameObject spark, tracer;
    [HideInInspector] public GameObject bundle, flash;

    public override void Initialize()
    {
        Debug.Log("Initialize MachineGun");
        bulletDmg = 20;
        maxAmmo = 500;
        ammo = maxAmmo;
        reloadTime = 0.1f;
        cooldownTime = 0.2f;
        localPos = new Vector3(0, 0, 0.5f);
        laserLine = GetComponent<LineRenderer>();
    }

    public override bool Attack(Vector3 origin, Vector3 dir)
    {
        if (reloadTimeLeft > 0 || ammo <= 0)
        {
            return false;
        }

        ammo--;
        reloadTimeLeft = reloadTime;

        Enemy target = RayCastFire(origin, dir);

        GameObject trace = Instantiate(tracer);
        float mag = Mathf.Max(Vector3.Distance(hit.point, origin), 40.0f);
        trace.GetComponent<LineRenderer>().SetPosition(0, origin);
        trace.GetComponent<LineRenderer>().SetPosition(1, origin + mag * dir);
        Destroy(trace, 0.2f);

        if (hit.collider != null)
        {
            if (target != null)
            {
                target.TakeDamage(bulletDmg);
            }

            // sus code for hit
            GameObject sp = Object.Instantiate(spark, hit.point, Quaternion.LookRotation(dir));
            Object.Destroy(sp, 0.2f);
        }

        return true;
    }

    public override void Animate(GameObject viewAnim, MonoBehaviour mono)
    {
        mono.StartCoroutine(Recoil(viewAnim, reloadTime));
    }

    public IEnumerator Recoil(GameObject viewAnim, float waitTime)
    {
        Transform rot = null;

        if (bundle == null)
        {
            bundle = GameObject.Find("Bundle"); // this is so ass
        }
        if (bundle != null)
        {
            rot = bundle.transform;
        }

        float time = 0;

        while (time < waitTime)
        {
            // move by local forward
            float t = (time / waitTime); // [0 1]

            if (bundle)
            {
                float dr = Mathf.Lerp(0.0f, 45.0f, Mathf.Pow(t, 3.0f));
                rot.Rotate(Vector3.forward, dr);
                bundle.transform.localRotation = rot.localRotation;
            }

            t = Mathf.Pow(t, 6.0f);
            Vector3 delta = new Vector3(0.0f, 0.0f, -1.0f) * Mathf.Lerp(0.01f, 0.0f, t);
            viewAnim.transform.localPosition = localPos + delta;
            time += Time.deltaTime;
            yield return null;
        }

        viewAnim.transform.localPosition = localPos; // reset
        yield return null;
    }

    public override bool Reload()
    {
        return true;
    }

    public override void GunUpdate()
    {
        //cooldownTime = Mathf.Max(0.0f, cooldownTime - Time.deltaTime);
        reloadTimeLeft = Mathf.Max(0.0f, reloadTimeLeft - Time.deltaTime);
    }
}
