using UnityEngine;

public abstract class Gun
{
    public int maxAmmo;
    public int ammo;
    public float reloadTime;
    public bool isActive = true;

    protected float reloadTimeLeft;

    public abstract void Attack(Vector3 origin, Quaternion dir);
    public abstract void Update();
}

public abstract class HitScanWeapon : Gun
{

}

public abstract class ProjectileWeapon : Gun
{
    public GameObject projectile;
}