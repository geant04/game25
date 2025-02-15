using UnityEngine;

public abstract class Gun
{
    public int maxAmmo;
    public int ammo;
    public float reloadTime;
    public bool isActive = true;
    public GameObject viewport;

    protected float reloadTimeLeft;

    public abstract bool Attack(Vector3 origin, Quaternion dir);
    public abstract void Animate(GameObject viewAnim, MonoBehaviour mono);
    public abstract void Update();
}

public abstract class HitScanWeapon : Gun
{

}

public abstract class ProjectileWeapon : Gun
{
    public GameObject projectile;
}