using UnityEngine;

public abstract class Gun
{
    public int maxAmmo;
    public int ammo;
    public float reloadTime;
    public float cooldownTime;
    public bool isActive = true;
    public GameObject viewAnim;
    public Vector3 localPos; // only assigned in Recoil! dangerous

    protected float reloadTimeLeft;
    protected float cooldownTimeLeft;

    public abstract bool Attack(Vector3 origin, Vector3 dir);
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
    public int bulletDmg;

    private LineRenderer laserLine;
    private float range = 100;
    
    protected RaycastHit hit;

    public Enemy RayCastFire(Vector3 origin, Vector3 forward)
    {
        if (Physics.Raycast(origin, forward, out hit, range))
        {
            return hit.collider.GetComponent<Enemy>();
        }

        Debug.Log("Miss");
        return null;
    }
}

public abstract class ProjectileWeapon : Gun
{
    public GameObject projectile;
}