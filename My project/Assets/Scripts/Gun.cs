using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject viewModel; // assign this in inspector, needed to change model

    [HideInInspector] public int maxAmmo, ammo;
    [HideInInspector] public float reloadTime, cooldownTime;
    [HideInInspector] public bool isActive = true;
    [HideInInspector] public GameObject viewAnim;
    [HideInInspector] public Vector3 localPos;

    protected float reloadTimeLeft;
    protected float cooldownTimeLeft;

    public virtual void Initialize() { }
    public virtual bool Attack(Vector3 origin, Vector3 dir) 
    {
        return true;
    }
    public virtual void Animate(GameObject viewAnim, MonoBehaviour mono) {}
    public virtual bool Reload() 
    {
        return false;
    }

    public virtual void GunUpdate() { }

    public void RefillAmmo(int amt)
    {
        ammo = Mathf.Min(maxAmmo, amt + ammo);
        Debug.Log("Refilled: " + ammo);
    }
    public void SetOrigin(Vector3 origin)
    {
        localPos = origin;
    }
}

public abstract class HitScanWeapon : Gun
{
    public int bulletDmg;
    protected LineRenderer laserLine;
    private float range = 100;
    
    protected RaycastHit hit;

    public Enemy RayCastFire(Vector3 origin, Vector3 forward)
    {
        if (Physics.Raycast(origin, forward, out hit, range))
        {
            return hit.collider.GetComponent<Enemy>();
        }

        return null;
    }
}

public abstract class ProjectileWeapon : Gun
{
    public GameObject projectile;
}