using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    // TO DO: if you want to abstract this further, just make it base class inherit from a weapon class
    public Gun weapon;
    public PlayerController playerController;
    public GameObject originPoint;
    public GameObject viewport;
    public GameObject viewAnim;
    public GameObject[] projectiles; // separate projectiles to dictionary thing, but very overkill

    void Awake()
    {
        weapon = new RocketLauncher(projectiles[0], viewport);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            bool fired = weapon.Attack(originPoint.transform.position, originPoint.transform.rotation);
            if (fired) weapon.Animate(viewAnim, this);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            weapon.Reload();
        }

        weapon.Update();
    }
}

