using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : ProjectileWeapon
{
    public RocketLauncher(GameObject projectile)
    {
        reloadTime = 0.4f;
        this.projectile = projectile;
    }

    public override void Attack(Vector3 origin, Quaternion dir)
    {
        if (reloadTimeLeft > 0)
        {
            return;
        }

        reloadTimeLeft = reloadTime;
        Object.Instantiate(projectile, origin, dir);
    }

    public override void Update()
    {
        reloadTimeLeft = Mathf.Max(0.0f, reloadTimeLeft - Time.deltaTime);
    }
}
