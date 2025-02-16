using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    // TO DO: if you want to abstract this further, just make it base class inherit from a weapon class
    public Gun weapon;
    public PlayerController playerController;
    public PlayerWeaponManager playerWeaponManager;
    public GameObject originPoint;
    public GameObject viewAnim;
    public GameObject flash;

    void Awake()
    { }

    private void Update()
    {
        weapon = playerWeaponManager.weapon.GetComponent<Gun>();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            bool fired = weapon.Attack(originPoint.transform.position, originPoint.transform.forward);
            if (fired)
            {
                weapon.Animate(viewAnim, this);
                GameObject fx = Instantiate(flash, originPoint.transform.position, Quaternion.LookRotation(originPoint.transform.forward));
                fx.transform.Rotate(transform.forward, Random.Range(0.0f, 360.0f));
                Destroy(fx, 0.1f); // do a pool system honestly
            }
        }
            if (Input.GetKeyDown(KeyCode.R))
        {
            weapon.Reload();
        }

        Debug.DrawRay(originPoint.transform.position, originPoint.transform.forward, Color.cyan);
        weapon.GunUpdate();
    }   
}

