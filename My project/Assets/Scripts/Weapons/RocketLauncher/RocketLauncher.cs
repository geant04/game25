using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : ProjectileWeapon
{
    float bobTime = 0.3f;

    public override void Initialize()
    {
        Debug.Log("Initialize Rocket");
        ammo = 20;
        reloadTime = 0.4f;
        localPos = new Vector3(0, 0, 0.5f);
    }

    public override bool Attack(Vector3 origin, Vector3 dir)
    {
        if (reloadTimeLeft > 0 || ammo <= 0)
        {
            return false;
        }

        ammo--;
        reloadTimeLeft = reloadTime;
        Object.Instantiate(projectile, origin, Quaternion.LookRotation(dir));

        return true;
    }

    public override void Animate(GameObject viewAnim, MonoBehaviour mono)
    {
        mono.StartCoroutine(Recoil(viewAnim, bobTime));
    }

    public IEnumerator Recoil(GameObject viewAnim, float waitTime)
    {
        float time = 0;

        while (time < waitTime)
        {
            // move by local forward
            float t = (time / waitTime); // [0 1]
            t = Mathf.Pow(t, 6.0f);

            Vector3 delta = new Vector3(0.0f, 0.0f, -1.0f) * Mathf.Lerp(0.3f, 0.0f, t);
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
        reloadTimeLeft = Mathf.Max(0.0f, reloadTimeLeft - Time.deltaTime);
    }
}
