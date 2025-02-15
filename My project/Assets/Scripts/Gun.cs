using UnityEngine;

public abstract class Gun
{
    public int maxAmmo;
    public int ammo;
    public float reloadTime;
    public float cooldownTime;
    public bool isActive = true;
    public GameObject viewport;

    protected float reloadTimeLeft;

    public abstract bool Attack(Vector3 origin, Quaternion dir);
    public abstract void Animate(GameObject viewAnim, MonoBehaviour mono);
    public abstract bool Reload();
    public abstract void Update();

    public void refillAmmo(int amt)
    {
        ammo = Mathf.Min(maxAmmo, amt + ammo);
    }
}

public abstract class HitScanWeapon : Gun
{

}

public abstract class ProjectileWeapon : Gun
{
    public GameObject projectile;
}